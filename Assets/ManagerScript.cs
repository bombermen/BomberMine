using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ManagerScript : Singleton<ManagerScript>
{
    private TextAreaScript[] _playerInfo = new TextAreaScript[4];
    private PlayerScript[] _players = new PlayerScript[4];
    private int _playerIndex = -1;
    private string _playerName;
    private string _hostAddress;
    private bool _Equipe;
    private bool _tirAmi;
    private int _nbJoueurConnecte = 0;
    private MapScript _map;
    private int choix;
    private PlayerScript _player;
    private bool _inGame = false;
    private NetworkScript _networkScript;
    //Variable qui permet de connaître le mode de jeu choisi par le joueur 
    private bool _modeDeJeuxSolo = false;
    private CameraGUIMutliMenuScript.Position _menuAAtteindreAuChargement = CameraGUIMutliMenuScript.Position.MENU_ECRAN_TITRE;
    private bool _managerLoaded;
    private int points = 0;
    private int _score;

    public int Score
    {
        get { return _score; }
        set { _score = value; }
    }

    public int Points
    {
        get { return points; }
        set { points = value; }
    }

    public void InitializePlayerInfo(int index, TextAreaScript textArea)
    {
        _playerInfo[index] = textArea;
        if (Player != null && Players[index] != null)
            textArea.SetText(Players[index].Nom);
    }

    public bool ModeDeJeuxSolo
    {
        get { return _modeDeJeuxSolo; }
        set { _modeDeJeuxSolo = value; }
    }

    public MapScript Map
    {
        get { return _map; }
        set { _map = value; }
    }

    public CameraGUIMutliMenuScript.Position MenuAAtteindreAuChargement
    {
        get { return _menuAAtteindreAuChargement; }
        set { _menuAAtteindreAuChargement = value; }
    }

    public NetworkScript NetworkScript
    {
        get { return _networkScript; }
        set { _networkScript = value; }
    }

    public bool InGame
    {
        get { return _inGame; }
        set { _inGame = value; }
    }

    public PlayerScript Player
    {
        get { return _player; }
        set { _player = value; }
    }

    //touches
    private KeyCode _avancer;
    private KeyCode _reculer;
    private KeyCode _droite;
    private KeyCode _gauche;
    private KeyCode _courir;
    private KeyCode _bombe;
    private KeyCode _mine;
    private KeyCode _pause;
    private KeyCode _chat;

    protected ManagerScript() { }

    public int NbJoueurConnecte
    {
        get { return _nbJoueurConnecte; }
        set { _nbJoueurConnecte = value; }
    }

    public KeyCode Avancer
    {
        get { return _avancer; }
        set { _avancer = value; }
    }

    public KeyCode Reculer
    {
        get { return _reculer; }
        set { _reculer = value; }
    }

    public KeyCode Droite
    {
        get { return _droite; }
        set { _droite = value; }
    }

    public KeyCode Gauche
    {
        get { return _gauche; }
        set { _gauche = value; }
    }

    public KeyCode Courir
    {
        get { return _courir; }
        set { _courir = value; }
    }

    public KeyCode Bombe
    {
        get { return _bombe; }
        set { _bombe = value; }
    }

    public KeyCode Mine
    {
        get { return _mine; }
        set { _mine = value; }
    }

    public KeyCode Pause
    {
        get { return _pause; }
        set { _pause = value; }
    }

    public KeyCode Chat
    {
        get { return _chat; }
        set { _chat = value; }
    }

    public bool Equipe
    {
        get { return _Equipe; }
        set { _Equipe = value; }
    }

    public bool TirAmi
    {
        get { return _tirAmi; }
        set { _tirAmi = value; }
    }

    public PlayerScript[] Players
    {
        get { return _players; }
        set { _players = value; }
    }

    public string PlayerName
    {
        get { return _playerName; }
        set { _playerName = value; }
    }

    public int PlayerIndex
    {
        get { return _playerIndex; }
        set { _playerIndex = value; }
    }

    public List<NetworkPlayer> GetPlayersByTeam(int team)
    {
        List<NetworkPlayer> result = new List<NetworkPlayer>();

        for (int i = 0; i < _players.Length; i++)
        {
            if (_players[i] != null && _players[i].Team == team)
            {
                result.Add(_players[i].NetPlayer);
            }
        }
        return result;
    }

    void Start()
    {
        //Recreation du manager -> suppression du nouveau
        if (_managerLoaded != ManagerScript.Instance._managerLoaded)
        {
            //Destroy(gameObject);
        }
        //Création du manager : initialisation du manager
        else if (!_managerLoaded)
        {
            DontDestroyOnLoad(this);
            PlayerIndex = -1;
            NbJoueurConnecte = 0;
            Players = new PlayerScript[4];
            Player = new PlayerScript();
            InGame = false;
            this.gameObject.name = "aaaManagerLoaded";
            _managerLoaded = true;
            LoadAndSaveDataScript _loadAndSaveData = LoadAndSaveDataScript.Instance;
        }
    }

    void OnServerInitialized()
    {
        TraitementJoueurConnecte(Network.player, _playerName);
    }

    void OnConnectedToServer()
    {
        this.networkView.RPC("TraitementJoueurConnecte", RPCMode.Server, Network.player, _playerName);
    }
	
    void OnPlayerDisconnected(NetworkPlayer player)
    {
		int playerIndex = -1;
        for (int i = 0; i < Players.Length; i++)
            if (Players[i] != null && Players[i].NetPlayer == player)
                playerIndex = i;
		
        if (Application.loadedLevelName == "Menu")
        {
            Players[playerIndex] = null;
            SharePlayersInfoToAll();
        }
		else
			Players[playerIndex].Connected = false;
    }

    [RPC]
    void TraitementJoueurConnecte(NetworkPlayer player, string name)
    {
        PlayerScript p = new PlayerScript();

        for (int i = 0; i < _players.Length; i++)
        {
            //premiere connexion du joueur
            if (_players[i] == null)
            {
                EnregistrerJoueur(player, name, p, i);
                break;
            }
            //reconnexion du joueur
            else if (Players[i].Ip == player.ipAddress && Players[i].Nom == name)
            {
                ReconnecterJoueur(i, player);
                break;
            }
        }
        NbJoueurConnecte++;
    }

    private void ReconnecterJoueur(int playerIndex, NetworkPlayer player)
    {
        Players[playerIndex].Connected = true;
        //recupération de l'etat de la partie
        if (InGame)
        {
            this.networkView.RPC("ChangerScene", player, Application.loadedLevel);
            _networkScript.SynchroniserEtatPartie(player);
        }
    }

    [RPC]
    private void ChangerScene(int idScene)
    {
        Application.LoadLevel(idScene);
    }

    [RPC]
    public void RetournerEnSalleDAttente()
    {
        _menuAAtteindreAuChargement = CameraGUIMutliMenuScript.Position.SALLE_ATTENTE;
        Application.LoadLevel("Menu");
    }

    [RPC]
    public void RetournerALEcranTitre()
    {
        if (Network.isServer)
        {
            for (int i = 0; i < 4; i++)
            {
                if (Players[i] != null && Players[i].Connected == false)
                    Players[i] = null;
            }
			NbJoueurConnecte = 0;
            this.networkView.RPC("RetournerALEcranTitre", RPCMode.Others);
        }

        Deconnecter();
        _menuAAtteindreAuChargement = CameraGUIMutliMenuScript.Position.MENU_ECRAN_TITRE;
        Application.LoadLevel("Menu");
    }

    [RPC]
    public void DeconnecterJoueur(int playerIndex)
    {
        if (Players[playerIndex] != null && Players[playerIndex].Connected == false)
            Players[playerIndex] = null;
        
    }

    public void Deconnecter()
    {
        Network.Disconnect(250);
        Players = new PlayerScript[4];
        PlayerIndex = -1;
    }

    private void EnregistrerJoueur(NetworkPlayer netPlayer, string name, PlayerScript player, int indiceJoueur)
    {
        player.PlayerIndex = indiceJoueur;
        player.Initialized = true;
        player.NetPlayer = netPlayer;
        player.Nom = name;
        player.Ip = netPlayer.ipAddress;
        _players[indiceJoueur] = player;

        if (Network.player == netPlayer)
            SetPlayerIndex(player.PlayerIndex);
        else
            this.networkView.RPC("SetPlayerIndex", netPlayer, player.PlayerIndex);
        SharePlayersInfoToAll();
    }

    [RPC]
    void SetPlayerIndex(int playerIndex)
    {
        _playerIndex = playerIndex;
    }

    public void ChangeTeam(int team)
    {
        if (Network.isServer)
        {
            SetTeam(_playerIndex, team);
        }
        else
        {
            this.networkView.RPC("SetTeam", RPCMode.Server, _playerIndex, team);
        }
    }

    [RPC]
    void SetTeam(int playerIndex, int team)
    {
        _players[playerIndex].Team = team;
        SharePlayersInfoToAll();
    }

    void SharePlayersInfoToAll()
    {
        for (int i = 0; i < _players.Length; i++)
        {
            if (_players[i] != null)
            {
                this.networkView.RPC("NotifyPlayer", RPCMode.All, i, _players[i].Nom, _players[i].Team);
            }
        }
    }

    [RPC]
    void NotifyPlayer(int playerIndex, string name, int team)
    {
        string info = name + "\nGroupe : " + team;
        _playerInfo[playerIndex].SetText(info);
    }

    /// <summary>
    /// Lance la scène de jeu choisie
    /// </summary>
    [RPC]
    void LancerPartie(string scene)
    {
        Application.LoadLevel(scene);
    }

    /// <summary>
    /// Envoie l'indice de texture choisi pour les bombes
    /// </summary>
    [RPC]
    public void IndiceSkinBombs(int _playerIndex, int _value)
    {
        Players[_playerIndex].IndiceSkinBomb = _value;
    }

    /// <summary>
    /// Envoie l'indice de texture choisi pour la mine
    /// </summary>
    [RPC]
    public void IndiceSkinMines(int _playerIndex, int _value)
    {
        Players[_playerIndex].IndiceSkinMines = _value;
    }

    /// <summary>
    /// Envoie l'indice de texture choisi pour le corps
    /// </summary>
    [RPC]
    public void IndiceSkinCorp(int _playerIndex, int _value)
    {
        Players[_playerIndex].IndiceSkinCorp = _value;
    }

    /// <summary>
    /// Envoie l'indice de texture choisi pour le visage
    /// </summary>
    [RPC]
    public void IndiceSkinVisage(int _playerIndex, int _value)
    {
        Players[_playerIndex].IndiceSkinVisage = _value;
    }

    /// <summary>
    /// Envoie l'indice de texture choisi pour les bombes en mode solo
    /// </summary>
    public void IndiceSkinBombsSolo(int _playerIndex, int _value)
    {
        Player.IndiceSkinBomb = _value;
    }

    /// <summary>
    /// Envoie l'indice de texture choisi pour la mine en mode solo
    /// </summary>
    public void IndiceSkinMinesSolo(int _playerIndex, int _value)
    {
        Player.IndiceSkinMines = _value;
    }

    /// <summary>
    /// Envoie l'indice de texture choisi pour le corps en mode solo 
    /// </summary>
    public void IndiceSkinCorpSolo(int _playerIndex, int _value)
    {
        Player.IndiceSkinCorp = _value;
    }

    /// <summary>
    /// Envoie l'indice de texture choisi pour le visage en mode solo
    /// </summary>
    public void IndiceSkinVisageSolo(int _playerIndex, int _value)
    {
        Player.IndiceSkinVisage = _value;
    }

    [RPC]
    public void AjouterAuxBombesDuJoueur(int indiceJoueur, int nbBombesAAjouter)
    {
        if (Network.isServer)
        {
            Players[indiceJoueur].NbBombs += nbBombesAAjouter;
            if(indiceJoueur == 0)
                Player.NbBombs += nbBombesAAjouter;
        }
        else
        {
            Player.NbBombs += nbBombesAAjouter;
            this.networkView.RPC("AjouterAuxBombesDuJoueur", RPCMode.Server, indiceJoueur, nbBombesAAjouter);
        }
    }
}
