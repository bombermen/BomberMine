using UnityEngine;
using System.Collections;

public class NetworkSoloScript : MonoBehaviour {

    private ManagerScript _manager;

    private float _speed = 5.0f;
    private float _speedCourse = 5.0f;
    private float _mouseSensitivity;
    private float _rotLeftRight;
    private float _rotLeftRightPrecedent;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private Transform _positionDepot;
    [SerializeField]
    private GameObject _bombs;
    [SerializeField]
    private BombScript _bombScripts;
    [SerializeField]
    private GameObject _mines;
    [SerializeField]
    private MineDeclenchementScript _mineScripts;
    [SerializeField]
    private GameObject _camera;
    [SerializeField]
    private Transform _playerTransforms;
    [SerializeField]
    private Rigidbody _playerRigidbodies;
    private Rigidbody _rigidbodyPlayer;
    private Transform _transformPlayer;
    private bool avance = false;
    private bool recule = false;
    private bool droite = false;
    private bool gauche = false;
    private bool courir = false;
    private bool _inGame;

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
        _rigidbodyPlayer = _playerRigidbodies;
        _transformPlayer = _playerTransforms;
        InGame = true;
        _mouseSensitivity = 60.0f;
        Application.runInBackground = true;
        Application.targetFrameRate = 60;
        _manager.Score = 0;
    }

    void Update()
    {
        if (_manager.InGame)
        {
            _rotLeftRightPrecedent = _rotLeftRight;
            _rotLeftRight = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
            if (_rotLeftRightPrecedent != _rotLeftRight)
            {
                Souris(_rotLeftRight);
            }

            if (Input.GetKeyDown(_manager.Bombe))
            {
                Bombs();
            }

            if (Input.GetKeyDown(_manager.Mine))
            {
                //DepotOuRepriseMines();
            }
            if (Input.GetKeyDown(_manager.Avancer))
            {
                avance = true;
            }
            else if (Input.GetKeyUp(_manager.Avancer))
            {
                avance = false;
            }

            if (Input.GetKeyDown(_manager.Reculer))
            {
                recule = true;
            }
            else if (Input.GetKeyUp(_manager.Reculer))
            {
                recule = false;
            }

            if (Input.GetKeyDown(_manager.Gauche))
            {
                gauche = true;
            }
            else if (Input.GetKeyUp(_manager.Gauche))
            {
                gauche = false;
            }

            if (Input.GetKeyDown(_manager.Droite))
            {
                droite = true;
            }
            else if (Input.GetKeyUp(_manager.Droite))
            {
                droite = false;
            }
            if (Input.GetKeyDown(_manager.Courir))
            {
                Courir(true);
            }
            else if (Input.GetKeyUp(_manager.Courir))
            {
                Courir(false);
            }
            
        }
    }
    void FixedUpdate()
    {
        //Vitesse du point de vue du perso
        Vector3 localSpeed = Vector3.zero;
        //Vitesse globale (incluant la rotation du perso)
        Vector3 speed = Vector3.zero;

        if (avance)
        {
            localSpeed.z += 1;
        }
        if (recule)
        {
            localSpeed.z -= 1;
        }
        if (droite)
        {
            localSpeed.x += 1;
        }
        if (gauche)
        {
            localSpeed.x -= 1;
        }

        //Normalisation du vecteur (pour éviter le problème du straff)
        if (courir)
            localSpeed = localSpeed.normalized * (_speed + _speedCourse);
        else
            localSpeed = localSpeed.normalized * _speed;

        //Retour aux coordonées globales
        speed = _transformPlayer.TransformDirection(localSpeed);
        //Déplacement du personnage
        _rigidbodyPlayer.velocity = speed;

    }

    void Courir(bool keyDown)
    {
        bool courir = keyDown;

        if (keyDown)
        {
            _camera.camera.cullingMask &= ~(1 << 10);
        }
        else
        {
            _camera.camera.cullingMask |= 1 << 10;
        }
    }

    void Souris(float rotLeftRight)
    {
        _playerTransforms.Rotate(0, rotLeftRight, 0);
    }

    void Bombs()
    {
        if (_bombScripts.Poser && !_manager.Player.Item)
        {
            Vector3 positionDepot = _positionDepot.position;
            ConfirmerBombe();
        }
    }
    void ConfirmerBombe()
    {
        _bombs.transform.position = _positionDepot.position;
        _bombs.SetActive(true);
        _bombScripts.Poser = false;
    }

   
}
