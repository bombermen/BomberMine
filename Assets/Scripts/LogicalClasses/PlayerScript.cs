using UnityEngine;
using System.Collections;

public class PlayerScript
{
    private int _team = 0;
    private int _playerIndex;
    private NetworkPlayer _networkPlayer;
    private string _nom;
    private bool _initialized = false;
    private bool _connected = false;
    private string _ip;
    //Indique si le joueur possède un item (type foudroiement) bloquant le clic de la souris
    private bool _item = false;
    private int _indiceSkinMines = 0;
    private int _indiceSkinBomb = 0;
    private int _indiceSkinCorp = 0;
    private int _indiceSkinVisage = 0;
    private bool _freezed = false;
    private int _nbBombs = 1;
    private int points = 0;
    private int score = 0;

    public int Score
    {
        get { return score; }
        set { score = value; }
    }

    public int Points
    {
        get { return points; }
        set { points = value; }
    }

    public int IndiceSkinVisage
    {
        get { return _indiceSkinVisage; }
        set { _indiceSkinVisage = value; }
    }

    public int IndiceSkinCorp
    {
        get { return _indiceSkinCorp; }
        set { _indiceSkinCorp = value; }
    }

    public int NbBombs
    {
        get { return _nbBombs; }
        set { _nbBombs = value; }
    }

    public bool Freezed
    {
        get { return _freezed; }
        set { _freezed = value; }
    }

    public string Ip
    {
        get { return _ip; }
        set { _ip = value; }
    }

    public int IndiceSkinBomb
    {
        get { return _indiceSkinBomb; }
        set { _indiceSkinBomb = value; }
    }

    public int IndiceSkinMines
    {
        get { return _indiceSkinMines; }
        set { _indiceSkinMines = value; }
    }

    public int PlayerIndex
    {
        get { return _playerIndex; }
        set { _playerIndex = value; }
    }

    public bool Item
    {
        get { return _item; }
        set { _item = value; }
    }

    public bool Connected
    {
        get
        {
            return this._connected;
        }
        set
        {
            _connected = value;
        }
    }

    public bool Initialized
    {
        get
        {
            return this._initialized;
        }
        set
        {
            _initialized = value;
        }
    }

    public NetworkPlayer NetPlayer
    {
        get
        {
            return this._networkPlayer;
        }
        set
        {
            _networkPlayer = value;
        }
    }

    public string Nom
    {
        get
        {
            return this._nom;
        }
        set
        {
            _nom = value;
        }
    }

    public int Team
    {
        get
        {
            return this._team;
        }
        set
        {
            _team = value;
        }
    }
}
