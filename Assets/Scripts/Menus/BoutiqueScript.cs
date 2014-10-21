using UnityEngine;
using System.Collections;

public class BoutiqueScript : MonoBehaviour
{

    //GameObject qui permet de montrer au joueur qu'un matériau est disponible qu'après achat
    [SerializeField]
    private GameObject _cache;
    //Boolean utilisé pour identifier le sélecteur possitif du sélecteur négatif
    [SerializeField]
    private bool _increment = false;
    //Boolean utilisé pour identifier les sélecteurs du visage
    [SerializeField]
    private bool _persoF = false;
    [SerializeField]
    private TextAreaScript _nomSkin;
    
    [SerializeField]
    private skinsScript _color;

    public skinsScript Color
    {
        get { return _color; }
        set { _color = value; }
    }
    [SerializeField]
    private GameObject[] _objet = new GameObject[3];
    private ManagerScript _manager;
    //Tableau de matériaux des modèles 
    private Material[] _modelBombs = new Material[3];
    private Material[] _modelMines = new Material[2];
    private Material[] _modelPersoCorp = new Material[2];

    //Variable indiquant l'indice du matérial visible sur la scène
    static private int _value = 0;

    public static int Value
    {
        get { return BoutiqueScript._value; }
        set { BoutiqueScript._value = value; }
    }
    /// <summary>
    /// Initialisation des modèles dans la scène
    /// appelle des fonctions pour voir la matériau de base(noire)
    /// </summary>
    void Start()
    {
        if(_persoF)
            _nomSkin.SetText(_color.NameVisage[Value] + " (" + _color.Prix[Value] + " crédits)");
        else
            _nomSkin.SetText(_color.Name[Value] + " (" + _color.Prix[Value] + " crédits)");
        if (_persoF)
            PersoSkinsVisage();
        if (!_persoF)
            BombsMinesSkins();
    }
    /// <summary>
    /// Changement de valeur dans la variable _valeur
    /// à chaque clic de souris pour faire défiler les textures
    /// </summary>
    void OnMouseUp()
    {
        if (_persoF)
        {
            if (_increment)
            {
                if (_value == _color.TextureVisage.Length - 1)
                    _value = 0;
                else
                    _value += 1;
            }
            else
            {
                if (_value == 0)
                    _value = _color.TextureVisage.Length - 1;
                else
                    _value -= 1;
            }
        }
        else
        {
            if (_increment)
            {
                if (_value == _color.Texture.Length - 1)
                    _value = 0;
                else
                    _value += 1;
            }
            else
            {
                if (_value == 0)
                    _value = _color.Texture.Length - 1;
                else
                    _value -= 1;
            }
        }

        _nomSkin.SetText(_color.Name[Value] + " (" + _color.Prix[Value] + " crédits)");
        /// <summary>
        /// Appel des fonctions pour modifier les textures
        /// </summary>
        if (_persoF)
        {
            PersoSkinsVisage();
            _nomSkin.SetText(_color.NameVisage[Value] + " (" + _color.Prix[Value] + " crédits)");
        }
        if (!_persoF)
        {
            BombsMinesSkins();
            _nomSkin.SetText(_color.Name[Value] + " (" + _color.Prix[Value] + " crédits)");
        }
    }
    /// <summary>
    /// Application des matériaux sur les bombes 
    /// mines et corps du personnage de présentation 
    /// </summary>
    void BombsMinesSkins()
    {
        _modelBombs[0] = _objet[0].renderer.materials[0];
        _modelBombs[1] = _objet[0].renderer.materials[1];
        _modelBombs[2] = _color.Texture[_value];
        _modelMines[0] = _objet[1].renderer.materials[0];
        _modelMines[1] = _color.Texture[_value];
        _modelPersoCorp[0] = _objet[2].renderer.materials[0];
        _modelPersoCorp[1] = _color.Texture[_value];

        _objet[0].renderer.materials = _modelBombs;
        _objet[1].renderer.materials = _modelMines;
        _objet[2].renderer.materials = _modelPersoCorp;

        if (_color.AcheteTexture[_value])
            _cache.renderer.enabled = false;
        else
            _cache.renderer.enabled = true;
    }

    /// <summary>
    /// Application du matériau de visage sur la tête 
    /// du personnage de présentation
    /// </summary>
    void PersoSkinsVisage()
    {
        _objet[0].renderer.material = _color.TextureVisage[_value];

        if (_color.AcheteTextureVisage[_value])
            _cache.renderer.enabled = false;
        else
            _cache.renderer.enabled = true;
    }

}
