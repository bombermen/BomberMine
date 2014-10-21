using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetworkScript : MonoBehaviour
{
    private ManagerScript _manager;

    private float _speed = 5.0f;
    private float _speedCourse = 5.0f;
    private float _mouseSensitivity;
    private float _rotLeftRight;
    private float _rotLeftRightPrecedent;

    ///Joueurs
    //Tableau des contenant les 4 persos dans l'arène
    [SerializeField]
    private GameObject[] _players;
    //Tableau des 4 transforms des persos
    [SerializeField]
    private Transform[] _playerTransforms;
    //Tableau des 4 rigidbody des persos
    [SerializeField]
    private Rigidbody[] _playerRigidbodies;
    //Tableau des 4 position de dépots des objets pour les persos
    [SerializeField]
    private Transform[] _positionDepot;
    //Tableau contenant les 4 cameras des persos
    [SerializeField]
    private GameObject[] _cameras;
    //Tableau contenant les 4 boucliers des persos
    [SerializeField]
    private GameObject[] _boucliersJoueurs;
    //Rigidbody du joueur
    private Rigidbody _rigidbodyPlayer;
    //Transform du joueur
    private Transform _transformPlayer;
    //Tableau associant pour chaque capsule le joueur en son sein, -1 indique qu'elle est libre
    private int[] _joueurDansCapsule = { -1, -1, -1 };
    //Tableau indiquant pour chaque mine à quel joueur elle appartient
    private int[] _minesDuJoueur = { 0, 1, 2, 3 };
    //Indice du joueur ayant eu le bouclier
    private int _joueurAyantLeBouclier = -1;
    //Indice du dernier joueur ayant eu le bouclier
    private int _dernierJoueurAyantEuLeBouclier = -1;
    //Indice du joueur
    private int _monIndiceJoueur;
    //Etat du joueur
    private bool _freezed = false;
    //nombre de morts
    private int _nbMorts = 0;

    ///Items (Bombes et mines)
    //Tableau contenant les 4 bombes de l'arène
    [SerializeField]
    private GameObject[] _bombs;
    //Script des 4 bombes
    [SerializeField]
    private BombScript[] _bombScripts;
    //Tableau contenant les 4 mines de l'arène
    [SerializeField]
    private GameObject[] _mines;
    //Scripts des 4 mines
    [SerializeField]
    private MineDeclenchementScript[] _mineScripts;
    //Lien sur le bouclier propre à l'arène
    [SerializeField]
    private GameObject _bouclier;

    [SerializeField]
    private HUDScript _hud;

    [SerializeField]
    private GameObject[] _capsules;
    [SerializeField]
    private CapsuleScript[] _capsuleScripts;

    //Script de la minimap
    [SerializeField]
    private MiniMapScript _miniMap;
    //NetworkView de l'objet aaaNetwork
    private NetworkView _networkView;
    //private NetworkPlayer[] _playerNetworks;

    ///Objets destructibles
    //Objets destructibles de la scène (tous ceux qui sont sur le calque "Destructible")
    [SerializeField]
    private GameObject[] _objetsDestrucibles;
    //Dictionnaire qui pour un objet destructible donne son indice dans le tableau _objetsDestructibles
    private Dictionary<GameObject, int> _dictObjetsDestructibles;

    private bool _inGame;

    private LoadAndSaveDataScript _loadAndSaveData;

    //Tableau contenant les matériaux pour les mines, bombes, corps du personnage
    [SerializeField]
    private Material[] _tableauDeSkinsAutre;
    //Tableau contenant les matériaux pour le visage
    [SerializeField]
    private Material[] _tableauDeSkinsVisage;

    private Material[] _modelBombs = new Material[3];
    private Material[] _modelMines = new Material[2];
    private Material[] _modelCorp = new Material[2];
    private Material _modelVisage;

    //Intention de déplacement des joueurs (joueur1 > [0], joueur2 > [1], ...)
    private bool[] avance = { false, false, false, false };
    private bool[] recule = { false, false, false, false };
    private bool[] droite = { false, false, false, false };
    private bool[] gauche = { false, false, false, false };
    private bool[] courir = { false, false, false, false };
    private float[] _rotations = { 0f, 0f, 0f, 0f };

    public bool InGame
    {
        get { return _inGame; }
        set
        {
            _inGame = value;
            _manager.InGame = value;
        }
    }


    void Start()
    {
        _manager = ManagerScript.Instance;
        _manager.NetworkScript = this;
        InGame = true;
        _networkView = this.networkView;
        _monIndiceJoueur = _manager.PlayerIndex;

        _mouseSensitivity = 60.0f;
        Application.runInBackground = true;
        Application.targetFrameRate = 60;
        
        
        if (Network.isServer)
        {
            ActiverJoueurs();
            //Initialisation des textures choisies par tous les joueurs
            InitialiserCapsules();
            InitialiserSkinMines();
            InitialiserSkinBombs();
            InitialiserSkinCorp();
            InitialiserSkinVisage(); 
        }
        InitialiserObjetsDestructibles();


        _rigidbodyPlayer = _playerRigidbodies[_monIndiceJoueur];
        _transformPlayer = _playerTransforms[_monIndiceJoueur];
    }

    private void InitialiserCapsules()
    {
        for (int i = 0; i < 3; i++)
            _capsuleScripts[i].IdCapsule = i;
    }
    /// <summary>
    /// Envoie du choix du matériau à tous les joueurs
    /// </summary>
    [RPC]
    private void InitialiserMines(int renderersIndex, int i)
    {
        _modelMines[0] = _mines[i].transform.GetChild(2).renderer.materials[0];
        _modelMines[1] = _tableauDeSkinsAutre[renderersIndex];
        _mines[i].transform.GetChild(2).renderer.materials = _modelMines;
    }
    /// <summary>
    /// 1.Boucle sur tous les joueurs présents
    /// 2.Envoie du choix du matériau pour la mine
    /// du joueur à tous les autres joueurs (lui y compris)  
    /// </summary>
    private void InitialiserSkinMines()
    {
        int renderersIndex;
        for (int i = 0; i < _manager.Players.Length; i++)
        {
            if (_manager.Players[i] != null)
            {
                renderersIndex = _manager.Players[i].IndiceSkinMines;
                _networkView.RPC("InitialiserMines", RPCMode.All, renderersIndex, i);
            }
        }
    }
    /// <summary>
    /// Envoie du choix du matériau à tous les joueurs
    /// </summary>
    [RPC]
    private void InitialiserBombs(int renderersIndex, int i)
    {
        _modelBombs[0] = _bombs[i].transform.renderer.materials[0];
        _modelBombs[1] = _bombs[i].transform.renderer.materials[1];
        _modelBombs[2] = _tableauDeSkinsAutre[renderersIndex];
        _bombs[i].transform.renderer.materials = _modelBombs;
    }

    /// <summary>
    /// 1.Boucle sur tous les joueurs présents
    /// 2.Envoie du choix du matériau pour la bombe
    /// du joueur à tous les autres joueurs (lui y compris)  
    /// </summary>
    private void InitialiserSkinBombs()
    {
        int renderersIndex ;
        for (int i = 0; i < _manager.Players.Length; i++)
        {
            if (_manager.Players[i] != null)
            {
                renderersIndex = _manager.Players[i].IndiceSkinBomb;
                _networkView.RPC("InitialiserBombs", RPCMode.All, renderersIndex, i);
            }
        }  
    }

    /// <summary>
    /// Envoie du choix du matériau à tous les joueurs
    /// </summary>
    [RPC]
    private void InitialiserCorp(int renderersIndex, int i)
    {
        _modelCorp[0] = _players[i].transform.GetChild(2).GetChild(2).renderer.materials[0];
        _modelCorp[1] = _tableauDeSkinsAutre[renderersIndex];
        _players[i].transform.GetChild(2).GetChild(2).renderer.materials = _modelCorp;
    }

    /// <summary>
    /// 1.Boucle sur tous les joueurs présents
    /// 2.Envoie du choix du matériau pour le corps du personnage
    /// du joueur à tous les autres joueurs (lui y compris)  
    /// </summary>
    private void InitialiserSkinCorp()
    {
        int renderersIndex;
        for (int i = 0; i < _manager.Players.Length; i++)
        {
            if (_manager.Players[i] != null)
            {
                renderersIndex = _manager.Players[i].IndiceSkinCorp;
                _networkView.RPC("InitialiserCorp", RPCMode.All, renderersIndex, i);
            }
        }
    }

    /// <summary>
    /// Envoie du choix du matériau à tous les joueurs
    /// </summary>
    [RPC]
    private void InitialiserVisage(int renderersIndex, int i)
    {
        _modelVisage = _tableauDeSkinsVisage[renderersIndex];
        _players[i].transform.GetChild(2).GetChild(3).renderer.material = _modelVisage;
    }

    /// <summary>
    /// 1.Boucle sur tous les joueurs présents
    /// 2.Envoie du choix du matériau pour le visage
    /// du joueur à tous les autres joueurs (lui y compris)  
    /// </summary>
    private void InitialiserSkinVisage()
    {
        int renderersIndex;
        for (int i = 0; i < _manager.Players.Length; i++)
        {
            if (_manager.Players[i] != null)
            {
                renderersIndex = _manager.Players[i].IndiceSkinVisage;
                _networkView.RPC("InitialiserVisage", RPCMode.All, renderersIndex, i);
            }
        }
    }

    /// <summary>
    /// Initilise le dictionnaire permettant de retrouver un indice d'objet destructible
    /// (dans la liste _dictObjetsDestructibles) en fonction de son GameObject
    /// </summary>
    private void InitialiserObjetsDestructibles()
    {
        _dictObjetsDestructibles = new Dictionary<GameObject, int>();
        for (int i = 0; i < _objetsDestrucibles.Length; i++)
        {
            _dictObjetsDestructibles.Add(_objetsDestrucibles[i], i);
        }
    }

    /// <summary>
    /// Active les joueurs connectés sur la scène
    /// </summary>
    private void ActiverJoueurs()
    {
        PlayerScript[] players = _manager.Players;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null)
            {
                _networkView.RPC("ActiverJoueur", RPCMode.All, i);
                _manager.Players[i].Score = 0;
            }
        }
    }

    /// <summary>
    /// Fin de la partie
    /// Envoie a tous les joueurs l'état de fin de partie
    /// </summary>
    public void FinPartie()
    {
        LoadAndSaveDataScript loadAndSaveData = LoadAndSaveDataScript.Instance;
        loadAndSaveData.LoadXml();
        loadAndSaveData.PlayerCredits += _manager.Player.Points;
        loadAndSaveData.SaveXml();

        if (Network.isServer)
        {
            int gagnant = 0;
            for (int i = 0; i < _manager.NbJoueurConnecte; i++)
            {
                for (int y = 0; y < _manager.NbJoueurConnecte; y++)
                {
                    if (_manager.Players[i].Score > _manager.Players[y].Score)
                    {
                        gagnant = i;
                    }
                }
            }
            string NomJoueurGagnant = _manager.Players[gagnant].Nom;
            _networkView.RPC("ConfirmerFinPartie", RPCMode.All, NomJoueurGagnant);
        }
    }

    /// <summary>
    /// Confirmation de fin de partie
    /// affichage de l'ecran de fin de partie
    /// </summary>
    [RPC]
    private void ConfirmerFinPartie(string NomJoueurGagnant)
    {
        InGame = false;
        _hud.AfficherEcranFinPartie(NomJoueurGagnant);
    }

    /// <summary>
    /// Récupération des Inputs
    /// Les données sont ensuite traitées parallèlement
    /// sur le serveur et chez le client et le client est
    /// régulièrement mis à jour par rapport au serveur.
    /// </summary>
    void Update()
    {
        if (_manager.InGame)
        {
            if (Network.isServer)
            {
                _rotLeftRightPrecedent = _rotLeftRight;
                _rotLeftRight = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
                if (_rotLeftRightPrecedent != _rotLeftRight)
                {
                    Souris(_monIndiceJoueur, _rotLeftRight, Network.player);
                }

                if (Input.GetKeyDown(_manager.Bombe))
                {
                    Bombs(_monIndiceJoueur);
                }

                if (Input.GetKeyDown(_manager.Mine))
                {
                    DepotOuRepriseMines(_monIndiceJoueur);
                }

                if (Input.GetKeyDown(_manager.Avancer))
                {
                    Avancer(_monIndiceJoueur, true);
                }
                else if (Input.GetKeyUp(_manager.Avancer))
                {
                    Avancer(_monIndiceJoueur, false);
                }

                if (Input.GetKeyDown(_manager.Reculer))
                {
                    Reculer(_monIndiceJoueur, true);
                }
                else if (Input.GetKeyUp(_manager.Reculer))
                {
                    Reculer(_monIndiceJoueur, false);
                }

                if (Input.GetKeyDown(_manager.Gauche))
                {
                    Gauche(_monIndiceJoueur, true);
                }
                else if (Input.GetKeyUp(_manager.Gauche))
                {
                    Gauche(_monIndiceJoueur, false);
                }

                if (Input.GetKeyDown(_manager.Droite))
                {
                    Droite(_monIndiceJoueur, true);
                }
                else if (Input.GetKeyUp(_manager.Droite))
                {
                    Droite(_monIndiceJoueur, false);
                }
                if (Input.GetKeyDown(_manager.Courir))
                {
                    Courir(_monIndiceJoueur, true);
                }
                else if (Input.GetKeyUp(_manager.Courir))
                {
                    Courir(_monIndiceJoueur, false);
                }
            }
            else if (Network.isClient)
            {
                _rotLeftRightPrecedent = _rotLeftRight;
                _rotLeftRight = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
                if (_rotLeftRightPrecedent != _rotLeftRight)
                {
                    _networkView.RPC("Souris", RPCMode.Server, _monIndiceJoueur, _rotLeftRight, Network.player);
                }

                if (Input.GetKeyDown(_manager.Bombe))
                    _networkView.RPC("Bombs", RPCMode.Server, _monIndiceJoueur);

                if (Input.GetKeyDown(_manager.Mine))
                    _networkView.RPC("DepotOuRepriseMines", RPCMode.Server, _monIndiceJoueur);

                if (Input.GetKeyDown(_manager.Avancer))
                    _networkView.RPC("Avancer", RPCMode.Server, _monIndiceJoueur, true);
                else if (Input.GetKeyUp(_manager.Avancer))
                    _networkView.RPC("Avancer", RPCMode.Server, _monIndiceJoueur, false);

                if (Input.GetKeyDown(_manager.Reculer))
                    _networkView.RPC("Reculer", RPCMode.Server, _monIndiceJoueur, true);
                else if (Input.GetKeyUp(_manager.Reculer))
                    _networkView.RPC("Reculer", RPCMode.Server, _monIndiceJoueur, false);

                if (Input.GetKeyDown(_manager.Gauche))
                    _networkView.RPC("Gauche", RPCMode.Server, _monIndiceJoueur, true);
                else if (Input.GetKeyUp(_manager.Gauche))
                    _networkView.RPC("Gauche", RPCMode.Server, _monIndiceJoueur, false);

                if (Input.GetKeyDown(_manager.Droite))
                    _networkView.RPC("Droite", RPCMode.Server, _monIndiceJoueur, true);
                else if (Input.GetKeyUp(_manager.Droite))
                    _networkView.RPC("Droite", RPCMode.Server, _monIndiceJoueur, false);
                if (Input.GetKeyDown(_manager.Courir))
                    _networkView.RPC("Courir", RPCMode.Server, _monIndiceJoueur, true);
                else if (Input.GetKeyUp(_manager.Courir))
                    _networkView.RPC("Courir", RPCMode.Server, _monIndiceJoueur, false);
            }
        }
    }
    /// <summary>
    /// Calcul de toutes les vitesses des persos chez le serveur
    /// Calcul de la position de son perso chez les client
    /// </summary>
    void FixedUpdate()
    {
        if (InGame)
        {
            if (Network.isServer)
            {

                for (int i = 0; i < 4; i++)
                {
                    if (_manager.Players[i] != null && !_manager.Players[i].Freezed)
                    {
                        Rigidbody rigidbodyPlayer = _playerRigidbodies[i];
                        Transform transformPlayer = _playerTransforms[i];

                        if (rigidbodyPlayer != null && transformPlayer != null)
                        {
                            //Vitesse du point de vue du perso
                            Vector3 localSpeed = Vector3.zero;
                            //Vitesse globale (incluant la rotation du perso)
                            Vector3 speed = Vector3.zero;
                            _playerTransforms[i].Rotate(0, _rotations[i], 0);

                            if (avance[i])
                            {
                                localSpeed.z += 1;
                            }
                            if (recule[i])
                            {
                                localSpeed.z -= 1;
                            }
                            if (droite[i])
                            {
                                localSpeed.x += 1;
                            }
                            if (gauche[i])
                            {
                                localSpeed.x -= 1;
                            }

                            //Normalisation du vecteur (pour éviter le problème du straff)
                            if (courir[i])
                                localSpeed = localSpeed.normalized * (_speed + _speedCourse);
                            else
                                localSpeed = localSpeed.normalized * _speed;

                            //Retour aux coordonées globales
                            speed = transformPlayer.TransformDirection(localSpeed);
                            //Déplacement du personnage
                            rigidbodyPlayer.velocity = new Vector3(speed.x, rigidbodyPlayer.velocity.y, speed.z);
                        }
                    }
                }
            }
            else if (!_freezed)
            {

                //Vitesse du point de vue du perso
                Vector3 localSpeed = Vector3.zero;
                //Vitesse globale (incluant la rotation du perso)
                Vector3 speed = Vector3.zero;
                _playerTransforms[_monIndiceJoueur].Rotate(0, _rotations[_monIndiceJoueur], 0);

                if (avance[_monIndiceJoueur])
                {
                    localSpeed.z += 1;
                }
                if (recule[_monIndiceJoueur])
                {
                    localSpeed.z -= 1;
                }
                if (droite[_monIndiceJoueur])
                {
                    localSpeed.x += 1;
                }
                if (gauche[_monIndiceJoueur])
                {
                    localSpeed.x -= 1;
                }

                //Normalisation du vecteur (pour éviter le problème du straff)
                if (courir[_monIndiceJoueur])
                    localSpeed = localSpeed.normalized * (_speed + _speedCourse);
                else
                    localSpeed = localSpeed.normalized * _speed;

                //Retour aux coordonées globales
                speed = _transformPlayer.TransformDirection(localSpeed);
                //Déplacement du personnage
                _rigidbodyPlayer.velocity = new Vector3(speed.x, _rigidbodyPlayer.velocity.y, speed.z);
            }
        }
    }

    /// <summary>
    /// Active un joueur dans l'arène
    /// </summary>
    /// <param name="playerIndex">joueur à activer</param>
    [RPC]
    void ActiverJoueur(int playerIndex)
    {
        _players[playerIndex].SetActive(true);
        if (_monIndiceJoueur != playerIndex)
        {
            _cameras[playerIndex].SetActive(false);
        }
    }

    /// <summary>
    /// Active ou désactive l'état de course d'un joueur
    /// Rend invisible les mines
    /// Méthode appelée uniquement par le SERVEUR
    /// </summary>
    /// <param name="playerIndex">joueur considéré</param>
    /// <param name="keyDown">activation (true) ou desctivation (false)</param>
    [RPC]
    void Courir(int playerIndex, bool keyDown)
    {
        courir[playerIndex] = keyDown;
        if (Network.isServer)
            _networkView.RPC("Courir", RPCMode.Others, playerIndex, keyDown);

        if (keyDown)
        {
            _cameras[playerIndex].camera.cullingMask &= ~(1 << 10);
        }
        else
        {
            _cameras[playerIndex].camera.cullingMask |= 1 << 10;
        }
    }

    /// <summary>
    /// Confirmation par le serveur du dépôt d'une bombe devant un joueur
    /// </summary>
    /// <param name="playerIndex">indice du joueur posant la bombe</param>
    /// <param name="positionDepot">position de dépôt de la bombe</param>
    [RPC]
    void ConfirmerBombe(int playerIndex, Vector3 positionDepot)
    {
		_bombs[playerIndex].rigidbody.velocity = Vector3.zero;
        _bombs[playerIndex].transform.position = positionDepot;
        _bombs[playerIndex].SetActive(true);
        _bombScripts[playerIndex].Poser = false;
        _bombScripts[playerIndex].NetworkScript = this;
        _bombScripts[playerIndex].BombIndex = playerIndex;
    }

    /// <summary>
    /// Demande au serveur du dépôt de la bombe
    /// Méthode appelée uniquement chez le SERVEUR
    /// </summary>
    /// <param name="playerIndex">indice du joueur désirant poser la bombe</param>
    [RPC]
    void Bombs(int playerIndex)
    {
        if (_bombScripts[playerIndex].Poser && !_manager.Players[playerIndex].Item)
        {
            Vector3 positionDepot = _positionDepot[playerIndex].position;
            _networkView.RPC("ConfirmerBombe", RPCMode.All, playerIndex, positionDepot);
        }
    }

    /// <summary>
    /// Confirmation par le serveur d'un dépôt de mine devant un joueur
    /// </summary>
    /// <param name="playerIndex">indice du joueur déposant la mine</param>
    /// <param name="mine">indice de la mine déposée</param>
    [RPC]
    void ConfirmerMines(int playerIndex, int mine)
    {
        _mines[mine].transform.position = _positionDepot[playerIndex].position;
        _mines[mine].SetActive(true);
        if (playerIndex == _monIndiceJoueur)
        {
            _hud.RetirerMine();
        }
    }

    /// <summary>
    /// Confirmation par le serveur d'une reprise de mine
    /// </summary>
    /// <param name="playerIndex">joueur ayant repris la mine</param>
    /// <param name="mine">mine reprise</param>
    [RPC]
    void ConfirmerMinesReprise(int playerIndex, int mine)
    {
        _mineScripts[mine]._minePoser = false;
        _mineScripts[mine]._mineRecuperable = false;
        _mines[mine].SetActive(false);
        _mineScripts[mine]._leJoueur = null;
        if (playerIndex == _monIndiceJoueur)
        {
            _hud.AjouterMine();
            _mineScripts[playerIndex]._idMine = playerIndex;
        }
    }

    /// <summary>
    /// Demande de dépôt ou de reprise de mine
    /// Si le joueur est dans la zone de récupération d'une mine, il récupère cette dernière
    /// Sinon, s'il possède une mine, il la dépose
    /// Méthode uniquement appelée chez le SERVEUR
    /// </summary>
    /// <param name="playerIndex">indice du joueur</param>
    [RPC]
    void DepotOuRepriseMines(int playerIndex)
    {
        int nb = JoueurSurUneMines(playerIndex);

        if (nb > -1)
        {
            _networkView.RPC("ConfirmerMinesReprise", RPCMode.All, playerIndex, nb);
            RepriseMines(playerIndex, nb);
        }
        else
        {
            int jaiUneMine = PoserUneMines(playerIndex);
            if (jaiUneMine > -1)
                _networkView.RPC("ConfirmerMines", RPCMode.All, playerIndex, jaiUneMine);
        }
    }

    /// <summary>
    /// Si le joueur à un bouclier, il le perd
    /// Sinon il meurt
    /// </summary>
    /// <param name="playerIndex">joueur impacté</param>
    public void ImpacterJoueur(int playerIndex)
    {
        //Le joueur impacté a un bouclier -> il le perd
        if (_joueurAyantLeBouclier == playerIndex)
        {
            _networkView.RPC("RendreBouclierDisponible", RPCMode.All);
        }
        //Le joueur meurt
        else
        {
            //Fin de la partie si tous les joueurs sont morts
            if (++_nbMorts >= _manager.NbJoueurConnecte - 1)
                FinPartie();
            _networkView.RPC("TuerJoueur", RPCMode.All, playerIndex);
        }
    }

    public void ImpacterJoueur(Transform joueur)
    {
        int playerIndex = GetPlayerIndexByTransform(joueur);
        if (playerIndex >= 0)
            ImpacterJoueur(playerIndex);
    }

    public void RecupererBouclier(Transform joueurTransform)
    {
        if (Network.isServer)
        {
            int playerIndex = GetPlayerIndexByTransform(joueurTransform);
            if (playerIndex != _dernierJoueurAyantEuLeBouclier)
            {
                BouclierRecupere(playerIndex);
            }
        }
    }

    /// <summary>
    /// Confirmation par le serveur de la récupération du bouclier
    /// </summary>
    /// <param name="playerIndex">joueur qui a récupéré le bouclier</param>
    void BouclierRecupere(int playerIndex)
    {
        if (Network.isServer)
        {
            string NomJoueur = _manager.Players[playerIndex].Nom;
            _networkView.RPC("ConfirmerBouclierRecupere", RPCMode.All, playerIndex, NomJoueur);
            if (playerIndex == 0)
                ConfirmerScore(100);
            else
                _networkView.RPC("ConfirmerScore", _manager.Players[playerIndex].NetPlayer, 100);
        }
    }

    [RPC]
    void ConfirmerBouclierRecupere(int playerIndex, string NomJoueur)
    {
        _boucliersJoueurs[playerIndex].SetActive(true);
        _joueurAyantLeBouclier = playerIndex;
        _bouclier.SetActive(false);
        _hud.AfficherJoueurAuBouclier(NomJoueur);
    }

    /// <summary>
    /// Confirmation de la perte du bouclier par le serveur
    /// </summary>
    [RPC]
    void RendreBouclierDisponible()
    {
        if (_joueurAyantLeBouclier != -1)
        {
            _boucliersJoueurs[_joueurAyantLeBouclier].SetActive(false);
            _bouclier.transform.position = _playerTransforms[_joueurAyantLeBouclier].position;
            _dernierJoueurAyantEuLeBouclier = _joueurAyantLeBouclier;
            _joueurAyantLeBouclier = -1;
            _bouclier.SetActive(true);
        }
    }

    [RPC]
    public void DemanderFoudroiement(int playerIndex)
    {
        ConfirmerFoudroiement(true, playerIndex);
        StartCoroutine(AttendreTempsFoudroiement(4.0f, playerIndex));
    }


    /// <summary>
    /// Confirmation de l'activation ou de la désactivation du foudroiement
    /// pour le joueur spécifié
    /// Appelée uniquement par le serveur
    /// </summary>
    /// <param name="activate">activation (true) ou désactivation du foudroiement</param>
    /// <param name="playerIndex">indice du joueur concerné</param>
    [RPC]
    void ConfirmerFoudroiement(bool activate, int playerIndex)
    {
        if (Network.isServer && playerIndex != 0)
        {
            _manager.Players[playerIndex].Item = activate;
            NetworkPlayer player = _manager.Players[playerIndex].NetPlayer;
            _networkView.RPC("ConfirmerFoudroiement", player, activate, playerIndex);
        }
        else
        {
            if (playerIndex == 0)
                _manager.Players[0].Item = activate;
            _manager.Player.Item = activate;
            _miniMap.FoudroiementActif = activate;
        }

        //Si le foudroiement est désactivé, le nombre de bombes du joueur revient à la normale
        if (!activate && playerIndex == _monIndiceJoueur)
        {
            _manager.Player.NbBombs = _bombs[playerIndex].activeInHierarchy ? 0 : 1;
            _hud.AfficherBombesDispo();
        }
    }

    private IEnumerator AttendreTempsFoudroiement(float seconds, int playerIndex)
    {
        yield return new WaitForSeconds(seconds);
        ConfirmerFoudroiement(false, playerIndex);
    }

    /// <summary>
    /// Confirmation du serveur que le joueur est mort
    /// </summary>
    /// <param name="playerIndex">joueur décédé</param>
    [RPC]
    void TuerJoueur(int playerIndex)
    {
        _cameras[playerIndex].transform.parent = null;
        _players[playerIndex].SetActive(false);
    }

    /// <summary>
    /// Mise à joueur de la rotation du perso par rapport à la position de
    /// sa souris sur l'écran
    /// </summary>
    /// <param name="playerIndex">indice du joueur</param>
    /// <param name="_rotLeftRight">valeur de la rotation</param>
    [RPC]
    void Souris(int playerIndex, float rotLeftRight, NetworkPlayer player)
    {
        if(Network.isServer && playerIndex != 0)
            _networkView.RPC ("Souris", player, playerIndex, rotLeftRight, player);
        _rotations[playerIndex] = rotLeftRight;
    }

    /// <summary>
    /// Indique l'état "Avancer" du perso
    /// Cet état permet la mise à jour de sa position dans le FixedUpdate
    /// </summary>
    /// <param name="playerIndex">indice du joueur</param>
    /// <param name="keyDown">état</param>
    [RPC]
    void Avancer(int playerIndex, bool keyDown)
    {
        avance[playerIndex] = keyDown;
        if (Network.isServer)
            _networkView.RPC("Avancer", RPCMode.Others, playerIndex, keyDown);
    }

    /// <summary>
    /// Indique l'état "Reculer" du perso
    /// Cet état permet la mise à jour de sa position dans le FixedUpdate
    /// </summary>
    /// <param name="playerIndex">indice du joueur</param>
    /// <param name="keyDown">état</param>
    [RPC]
    void Reculer(int playerIndex, bool keyDown)
    {
        recule[playerIndex] = keyDown;
        if (Network.isServer)
            _networkView.RPC("Reculer", RPCMode.Others, playerIndex, keyDown);
    }

    /// <summary>
    /// Indique l'état "Gauche" du perso
    /// Cet état permet la mise à jour de sa position dans le FixedUpdate
    /// </summary>
    /// <param name="playerIndex">indice du joueur</param>
    /// <param name="keyDown">état</param>
    [RPC]
    void Gauche(int playerIndex, bool keyDown)
    {
        gauche[playerIndex] = keyDown;
        if (Network.isServer)
            _networkView.RPC("Gauche", RPCMode.Others, playerIndex, keyDown);
    }

    /// <summary>
    /// Indique l'état "Droite" du perso
    /// Cet état permet la mise à jour de sa position dans le FixedUpdate
    /// </summary>
    /// <param name="playerIndex">indice du joueur</param>
    /// <param name="keyDown">état</param>
    [RPC]
    void Droite(int playerIndex, bool keyDown)
    {
        droite[playerIndex] = keyDown;
        if (Network.isServer)
            _networkView.RPC("Droite", RPCMode.Others, playerIndex, keyDown);
    }

    /// <summary>
    /// Retourne l'indice du joueur par rapport à son Transform
    /// </summary>
    /// <param name="joueurTransform">Transform du joueur</param>
    /// <returns>indice du joueur</returns>
    public int GetPlayerIndexByTransform(Transform joueurTransform)
    {
        for (int i = 0; i < _playerTransforms.Length; i++)
        {
            if (joueurTransform == _playerTransforms[i])
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Retourne la première mine appartenant au joueur
    /// et -1 s'il n'en a pas
    /// </summary>
    /// <param name="playerIndex">indice du joueur</param>
    /// <returns>indice de la première mine appartenant au joueur</returns>
    public int PoserUneMines(int playerIndex)
    {
        for (int i = 0; i < 4; i++)
        {
            if (_minesDuJoueur[i] == playerIndex)
            {
                _minesDuJoueur[i] = -1;
                return i;
            }
        }
        return -1;
    }

    public void RepriseMines(int playerIndex, int mine)
    {
        _minesDuJoueur[mine] = playerIndex;
    }

    public void EntrerDansCapsule(Transform playerTransform, int capsuleIndex)
    {
        int playerIndex = GetPlayerIndexByTransform(playerTransform);
        if (Network.isServer)
        {
            if (CapsuleLibre(capsuleIndex))
            {
                _joueurDansCapsule[capsuleIndex] = playerIndex;
            }
        }
    }

    /// <summary>
    /// Réinitialise le tableau des joueurs contenus dans les capsules
    /// </summary>
    public void ReinitialiserCapsules()
    {
        CapsuleScript._nbCapsules--;

        for (int i = 0; i < 3; i++)
        {
            _joueurDansCapsule[i] = -1;
        }
    }

    /// <summary>
    /// Retourne si une capsule est libre
    /// </summary>
    /// <param name="capsuleIndex">indice de la capsule</param>
    /// <returns>true si la capsule est libre</returns>
    private bool CapsuleLibre(int capsuleIndex)
    {
        return _joueurDansCapsule[capsuleIndex] < 0;
    }

    public void ExploserBombeH()
    {
        if (Network.isServer)
        {
            for (int i = 0; i < _manager.NbJoueurConnecte; i++)
            {
                if (!JoueurSauf(i))
                {
                    ImpacterJoueur(i);
                }
            }
        }
    }

    private bool JoueurSauf(int playerIndex)
    {
        for (int i = 0; i < 3; i++)
        {
            if (_joueurDansCapsule[i] == playerIndex)
                return true;
        }

        return false;
    }

    public void JoueurMort(Transform playerTransform)
    {
        if (Network.isServer)
        {
            int playerIndex = GetPlayerIndexByTransform(playerTransform);
            ImpacterJoueur(playerIndex);
        }
    }

    private int JoueurSurUneMines(int playerIndex)
    {
        for (int i = 0; i < 4; i++)
        {
            if (_mineScripts[i]._leJoueur == _players[playerIndex])
                return i;
        }
        return -1;
    }

    /// <summary>
    /// Détruit la liste des objets indiqués et la met à jour chez les clients
    /// </summary>
    /// <param name="hitObjects">objets à détruire</param>
    public void ExploserBombe(int indiceBombe, List<GameObject> hitObjects)
    {
        Exploser(hitObjects, indiceBombe, 1);
    }

    public void ExploserMine(int _idMine, List<GameObject> hitObjects)
    {
        int indiceMine = _minesDuJoueur[_idMine];
        Exploser(hitObjects, indiceMine, 2);
    }

    private void Exploser(List<GameObject> hitObjects, int indiceTueur, int type)
    {
        //Destruction des objets et personnages touchés
        foreach (GameObject ob in hitObjects)
        {
            if (ob.layer == 8)
            {
                int indiceTuer = GetPlayerIndexByTransform(ob.transform);
                if (type == 2)
                    indiceTueur -= 1;
                if (_manager.Players[indiceTuer] != null && _manager.Players[indiceTueur] != null)
                {
                    string NomJoueurTuer = _manager.Players[indiceTuer].Nom;
                    string NomJoueurTueur = _manager.Players[indiceTueur].Nom;

                    if (_joueurAyantLeBouclier != indiceTuer)
                    {

                        if (Network.isServer)
                        {
                            _networkView.RPC("PointEnTuant", RPCMode.All, NomJoueurTuer, NomJoueurTueur, indiceTueur, type);
                            
                        }
                    }
                }
                ImpacterJoueur(ob.transform);
            }
            else
            {
                try
                {
                    int indiceObjet = _dictObjetsDestructibles[ob];
                    _networkView.RPC("DetruireObjet", RPCMode.All, indiceObjet);
                    ob.SetActive(false);
                }
                catch (KeyNotFoundException) { }
            }
        }
    }

    [RPC]
    public void PointEnTuant(string NomJoueurTuer, string NomJoueurTueur, int indiceTueur, int type)
    {
        switch (type)
        {
            case 1:
                _hud.AfficherJoueurTuer(NomJoueurTuer, NomJoueurTueur, true, 50);
                if (Network.isServer && indiceTueur == 0)
                {
                    ConfirmerScore(50);
                    _manager.Players[indiceTueur].Score += 50;
                }
                else
                {
                    _networkView.RPC("ConfirmerScore", _manager.Players[indiceTueur].NetPlayer, 50);
                    _manager.Players[indiceTueur].Score += 50;
                }
                break;
            case 2:
                _hud.AfficherJoueurTuer(NomJoueurTuer, NomJoueurTueur, true, 150);
                if (Network.isServer && indiceTueur == 0)
                {
                    ConfirmerScore(150);
                    _manager.Players[indiceTueur].Score += 150;
                }
                else
                {
                    _networkView.RPC("ConfirmerScore", _manager.Players[indiceTueur].NetPlayer, 150);
                    _manager.Players[indiceTueur].Score += 150;
                }
                break;
            default:
                break;
        }
    }

    [RPC]
    public void ConfirmerScore(int score)
    {
        _hud.Score(score);
    }

    public void DebutDePhase2()
    {
        if (Network.isServer)
            _networkView.RPC("ComfirmerDebutDePhase2", RPCMode.All);
    }

    [RPC]
    public void ComfirmerDebutDePhase2()
    {
        _hud.AfficherPhase2();
    }

    [RPC]
    public void JoueurSaufApresPhase2()
    {
        if (Network.isServer)
        {
            for (int i = 0; i < _manager.NbJoueurConnecte; i++)
            {
                if (JoueurSauf(i))
                {
                    _hud.AfficherPhasePasse();
                    if (i == 0)
                    {
                        ConfirmerScore(100);
                        _manager.Players[i].Score += 100;
                    }
                    else
                    {
                        _networkView.RPC("ConfirmerScore", _manager.Players[i].NetPlayer, 100);
                        _manager.Players[i].Score += 100;
                    }
                }
            }
        }
    }
    /// <summary>
    /// Confirmation par le serveur de la destruction d'un objet
    /// </summary>
    /// <param name="indiceObjet">indice de l'objet détruit</param>
    [RPC]
    private void DetruireObjet(int indiceObjet)
    {
        GameObject ob = _objetsDestrucibles[indiceObjet];
        ob.SetActive(false);
    }

    /// <summary>
    /// Synchronise l'ensemble des éléments de la partie avec le joueur
    /// A utiliser par le serveur uniquement
    /// </summary>
    /// <param name="player"></param>
    public void SynchroniserEtatPartie(NetworkPlayer player)
    {
        Vector3[] positionJoueurs = new Vector3[4];
        bool[] joueurActif = new bool[4];
        List<int> indiceDesObjetsDetruits = new List<int>();
        Vector3[] positionDesBombes = new Vector3[4];
        Vector3[] positionDesBombesFoudroyees = new Vector3[5];
        bool[] bombesActivees = new bool[4];
        bool[] bombesFoudroyeesActivees = new bool[5];

        for (int i = 0; i < 4; i++)
        {
            joueurActif[i] = _players[i].activeInHierarchy;
            positionJoueurs[i] = _playerTransforms[i].position;
        }

        for (int i = 0; i < _objetsDestrucibles.Length; i++)
        {
            if (_objetsDestrucibles[i].activeInHierarchy)
                indiceDesObjetsDetruits.Add(i);
        }

        for (int i = 0; i < 4; i++)
        {
            bombesActivees[i] = _bombs[i].activeInHierarchy;
            positionDesBombes[i] = _bombs[i].transform.position;
        }

        GameObject[] bombsAFoudroyer = _miniMap.BombesAFoudroyer;
        for (int i = 0; i < 5; i++)
        {
            bombesFoudroyeesActivees[i] = bombsAFoudroyer[i].activeInHierarchy;
            positionDesBombesFoudroyees[i] = bombsAFoudroyer[i].transform.position;
        }

        _networkView.RPC("RecupererEtatPartie", player, positionJoueurs, joueurActif, indiceDesObjetsDetruits, positionDesBombes, bombesActivees, positionDesBombesFoudroyees, bombesFoudroyeesActivees);
    }

    /// <summary>
    /// Récupération de l'état de l'ensemble des éléments de la partie
    /// </summary>
    /// <param name="positionJoueurs">position des joueurs</param>
    /// <param name="joueursActifs">indique pour chaque joueur si le gameobject du joueur est actif</param>
    /// <param name="indiceDesObjetsDetruits">liste de tous les indices des objets détruits</param>
    /// <param name="positionDesBombes">position des bombes</param>
    /// <param name="bombesActivees">bombes activees</param>
    /// <param name="positionDesBombesFoudroyees">position des bombes foudroyees</param>
    /// <param name="bombesFoudroyeesActivees">indique les bombes foudroyees actives</param>
    [RPC]
    private void RecupererEtatPartie(Vector3[] positionJoueurs, bool[] joueursActifs, List<int> indiceDesObjetsDetruits, Vector3[] positionDesBombes, bool[] bombesActivees, Vector3[] positionDesBombesFoudroyees, bool[] bombesFoudroyeesActivees)
    {
        for (int i = 0; i < 4; i++)
        {
            _players[i].SetActive(joueursActifs[i]);
            _playerTransforms[i].position = positionJoueurs[i];
        }

        foreach (int indiceObjet in indiceDesObjetsDetruits)
            _objetsDestrucibles[indiceObjet].SetActive(false);

        for (int i = 0; i < 4; i++)
        {
            _bombs[i].SetActive(bombesActivees[i]);
            _bombs[i].transform.position = positionDesBombes[i];
        }

        GameObject[] bombsAFoudroyer = _miniMap.BombesAFoudroyer;
        for (int i = 0; i < 5; i++)
        {
            bombsAFoudroyer[i].SetActive(bombesFoudroyeesActivees[i]);
            bombsAFoudroyer[i].transform.position = positionDesBombesFoudroyees[i];
        }
    }

    public void PlacerJoueurDansCapsule(bool freeze, Transform playerTransform, int idCapsule)
    {
        int playerIndex = GetPlayerIndexByTransform(playerTransform);
        _joueurDansCapsule[idCapsule] = playerIndex;
        DemanderFreezePlayer(playerIndex, freeze);
    }

    [RPC]
    private void DemanderFreezePlayer(int playerIndex, bool freeze)
    {
        _manager.Players[playerIndex].Freezed = freeze;
        if (freeze)
            _playerRigidbodies[_monIndiceJoueur].constraints = RigidbodyConstraints.FreezeRotation;
        else
            _playerRigidbodies[_monIndiceJoueur].constraints = RigidbodyConstraints.FreezeAll;
        if (Network.isServer)
            ConfirmerFreezePlayer(freeze);
        else
            _networkView.RPC("ConfirmerFreezePlayer", _manager.Players[playerIndex].NetPlayer, freeze);
    }

    private void UnfreezePlayers()
    {
        for (int i = 0; i < _manager.NbJoueurConnecte; i++)
        {
            if (_manager.Players[i].Freezed)
                DemanderFreezePlayer(i, false);
        }
    }

    [RPC]
    private void ConfirmerFreezePlayer(bool freeze)
    {
        _freezed = freeze;
        if (freeze)
            _rigidbodyPlayer.constraints = RigidbodyConstraints.FreezeAll;
        else
            _rigidbodyPlayer.constraints = RigidbodyConstraints.FreezeRotation;
    }


    /// <summary>
    /// Ordonne une transition de phase a tous les joueurs
    /// Méthode à utiliser par le serveur
    /// </summary>
    /// <param name="phase">phase (1 ou 2)</param>
    /// <param name="round">round (1 à 4)</param>
    public void ChangerDePhase(int phase, int round)
    {
        string nomMethode = "InitialiserPhase" + phase;
        this.networkView.RPC(nomMethode, RPCMode.All, round);
    }

    /// <summary>
    /// Initialisation de la phase 1 :
    /// -explosion de la bombe H pour clore la phase 2
    /// -disparition des capsules
    /// </summary>
    /// <param name="round">round actuel (de 1 à 4)</param>
    [RPC]
    private void InitialiserPhase1(int round)
    {
        if (round != 1)
        {
            ExploserBombeH();
            UnfreezePlayers();
        }
        for (int i = 0; i < 3; i++)
        {
            _capsules[i].SetActive(false);
        }
    }

    /// <summary>
    /// Initialisation de la phase 2 :
    /// -Reinitialisation de l'occupation des capsules
    /// -Apparition des capsules
    /// </summary>
    /// <param name="round">round actuel (de 1 à 4)</param>
    [RPC]
    private void InitialiserPhase2(int round)
    {
        int nbCapsules = 4 - round;
        CapsuleScript._nbCapsules = nbCapsules;
        ReinitialiserCapsules();
        _hud.LancerDecomptePhase2();
        for (int i = 0; i < nbCapsules; i++)
        {
            _capsules[i].SetActive(true);
        }
        for (int i = 0; i < nbCapsules; i++)
        {
			_capsuleScripts[i].Reset();
            _capsuleScripts[i].OpenDoors();
			_capsuleScripts[i].IdCapsule = i;
        }
    }
}