using UnityEngine;
using System.Collections;

public class SelecteurskinMulti : MonoBehaviour
{

    [SerializeField]
    private bool _increment = false;
    [SerializeField]
    private bool _bomb = false;
    [SerializeField]
    private bool _modeSolo = false;
    [SerializeField]
    private bool _mine = false;
    [SerializeField]
    private bool _corp = false;
    [SerializeField]
    private bool _visage = false;
    [SerializeField]
    private skinsScript _color;
    [SerializeField]
    private GameObject _objet;
    private ManagerScript _manager;
    [SerializeField]
    private MapScript _choixMap;
    [SerializeField]
    private LancerPartieScript _lancer;
    //Tableaux avec les matériaux pour les modèles vus par le joueur quand il choisit
    private Material[] _modelBombs = new Material[3];
    private Material[] _modelMines = new Material[2];
    private Material[] _modelCorp = new Material[2];
    private Material _modelVisage;
    //Tableau des indices des textures achetées
    private int[] _tabIdSkin;
    private int[] _tabIdSkinVisage;
    //ArrayList des matériaux achetés
    private ArrayList choix;
    private ArrayList choix2;
    
    static private int _value = 0;
    static private int _value2 = 0;
    static private int _value3 = 0;

    public static int Value
    {
        get { return SelecteurskinMulti._value; }
        set { SelecteurskinMulti._value = value; }
    }

    public int[] TabIdSkinVisage
    {
        get { return _tabIdSkinVisage; }
        set { _tabIdSkinVisage = value; }
    }

    public int[] TabIdSkin
    {
        get { return _tabIdSkin; }
        set { _tabIdSkin = value; }
    }

    public skinsScript Color
    {
        get { return _color; }
        set { _color = value; }
    }


    void Start()
    {
        _manager = ManagerScript.Instance;
        choix = new ArrayList();
        choix2 = new ArrayList();
        TableauDachat();
    }

    void Update()
    {
        //Vérification des deux tablaux d'achat
        if (_color.MiseAJour)
            TableauDachat();
    }

    void OnMouseUp()
    {
        TableauDachat();
        if (_visage)
        {
            if (_increment)
            {
                if (_value2 == choix2.Count - 1)
                    _value2 = 0;
                else
                    _value2 += 1;
            }
            else
            {
                if (_value2 == 0)
                    _value2 = choix2.Count - 1;
                else
                    _value2 -= 1;
            }
        }
        else if (_bomb || _mine || _corp)
        {
            if (_increment)
            {
                if (_value == choix.Count - 1)
                    _value = 0;
                else
                    _value += 1;
            }
            else
            {
                if (_value == 0)
                    _value = choix.Count - 1;
                else
                    _value -= 1;
            }
        }
        else
        {
            if (_increment)
            {
                if (_value3 == _choixMap.Texture.Length - 1)
                    _value3 = 0;
                else
                    _value3 += 1;
            }
            else
            {
                if (_value3 == 0)
                    _value3 = _choixMap.Texture.Length - 1;
                else
                    _value3 -= 1;
            }
        }
        if (_bomb)
            AppBombs();
        if (_mine)
            AppMines();
        if (_corp)
            AppCorp();
        if (_visage)
            AppVisage();
        if (!_mine && !_bomb && !_corp && !_visage)
        {
            _objet.renderer.material = _choixMap.Texture[_value3];
            _lancer.Choix = _value3;
        }
    }

    void TableauDachat()
    {
        int y = 0;
        int x = 0;
        _tabIdSkin = new int[_color.AcheteTexture.Length];
        _tabIdSkinVisage = new int[_color.AcheteTextureVisage.Length];
        choix.Clear();
        choix2.Clear();
        /// <summary>
        /// Boucle sur toutes les textures
        /// Vérification des matériaux déjà achetés
        /// Ajout dans l'ArrayList des matériaux achetés
        /// On remplit un tableau, avec pour indice le nombre des matériaux achetés 
        /// </summary>
        for (int i = 0; i < _color.AcheteTexture.Length; i++)
        {
            if (_color.AcheteTexture[i])
            {
                choix.Add(_color.Texture[i]);
                _tabIdSkin[y] = i;
                y++;
            }
        }
        for (int i = 0; i < _color.AcheteTextureVisage.Length; i++)
        {
            if (_color.AcheteTextureVisage[i])
            {
                choix2.Add(_color.TextureVisage[i]);
                _tabIdSkinVisage[x] = i;
                x++;
            }
        }
        _color.MiseAJour = false;
    }
    /// <summary>
    /// 1.Application de la texture choisie sur le modèle dans le menu
    /// 2.Envoi du choix au manager 
    /// en fontion de si on est serveur client ou en mode solo 
    /// </summary>
    void AppBombs()
    {
        _modelBombs[0] = _objet.renderer.materials[0];
        _modelBombs[1] = _objet.renderer.materials[1];
        _modelBombs[2] = (Material)choix[_value];
        _manager.Player.IndiceSkinBomb = _value;
        _objet.renderer.materials = _modelBombs;
        if (_modeSolo)
            _manager.IndiceSkinBombsSolo(_manager.PlayerIndex, _tabIdSkin[_value]);
        if (Network.isServer)
            _manager.IndiceSkinBombs(_manager.PlayerIndex, _tabIdSkin[_value]);
        if (Network.isClient && !_modeSolo)
            _manager.networkView.RPC("IndiceSkinBombs", RPCMode.Server, _manager.PlayerIndex, _tabIdSkin[_value]);

    }
    /// <summary>
    /// 1.Application de la texture choisie sur le modèle dans le menu
    /// 2.Envoi du choix au manager 
    /// en fontion de si on est serveur client ou en mode solo 
    /// </summary>
    void AppMines()
    {
        _modelMines[0] = _objet.renderer.materials[0];
        _modelMines[1] = (Material)choix[_value];
        _objet.renderer.materials = _modelMines;
        if (_modeSolo)
            _manager.IndiceSkinMinesSolo(_manager.PlayerIndex, _tabIdSkin[_value]);
        if (Network.isServer)
            _manager.IndiceSkinMines(_manager.PlayerIndex, _tabIdSkin[_value]);
        if (Network.isClient && !_modeSolo)
            _manager.networkView.RPC("IndiceSkinBombs", RPCMode.Server, _manager.PlayerIndex, _tabIdSkin[_value]);
    }
    /// <summary>
    /// 1.Application de la texture choisie sur le modèle dans le menu
    /// 2.Envoi du choix au manager 
    /// en fontion de si on est serveur client ou en mode solo 
    /// </summary>
    void AppCorp()
    {
        _modelCorp[0] = _objet.renderer.materials[0];
        _modelCorp[1] = (Material)choix[_value];
        _objet.renderer.materials = _modelCorp;
        if (_modeSolo)
            _manager.IndiceSkinCorpSolo(_manager.PlayerIndex, _tabIdSkin[_value]);
        if (Network.isServer)
            _manager.IndiceSkinCorp(_manager.PlayerIndex, _tabIdSkin[_value]);
        if (Network.isClient && !_modeSolo)
            _manager.networkView.RPC("IndiceSkinCorp", RPCMode.Server, _manager.PlayerIndex, _tabIdSkin[_value]);
    }
    /// <summary>
    /// 1.Application de la texture choisie sur le modèle dans le menu
    /// 2.Envoi du choix au manager 
    /// en fontion de si on est serveur client ou en mode solo 
    /// </summary>
    void AppVisage()
    {
        _modelVisage = (Material)choix2[_value2];
        _objet.renderer.material = _modelVisage;
        if (_modeSolo)
            _manager.IndiceSkinVisageSolo(_manager.PlayerIndex, _tabIdSkinVisage[_value2]);
        if (Network.isServer)
            _manager.IndiceSkinVisage(_manager.PlayerIndex, _tabIdSkinVisage[_value2]);
        if (Network.isClient && !_modeSolo)
            _manager.networkView.RPC("IndiceSkinVisage", RPCMode.Server, _manager.PlayerIndex, _tabIdSkinVisage[_value2]);
    }
}


