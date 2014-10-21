using UnityEngine;
using System.Collections;

public class ValiderAchat : MonoBehaviour
{

    private ManagerScript _manager;
    [SerializeField]
    private BoutiqueScript _boutique;
    [SerializeField]
    private GameObject _boutonAnnuler;
    [SerializeField]
    private GameObject _boutonValider;
    [SerializeField]
    private GlassPaneScript _glassPane;
    [SerializeField]
    private skinsScript _skins;

    //Si la variable est vrai on va acheter une texture de visage 
    //sinon c'est que l'on va acheter une texture pour les bombes, mines et le corps du personnage
    private bool _AchatVisage = false;

    [SerializeField]
    private GameObject _visageBloque;
    [SerializeField]
    private GameObject _bombeMineBloquee;

    public bool AchatVisage
    {
        get { return _AchatVisage; }
        set { _AchatVisage = value; }
    }

    void Start()
    {
        _manager = ManagerScript.Instance;
    }

    void OnMouseUp()
    {
        /// <summary>
        /// 1.Mise à jour du nombre de points du joueur
        /// 2.On passe à "Vrai" l'achat pour savoir que la texture a été débloquée 
        /// 3.On passe à "Vrai" la variable "MiseAJour" pour savoir que le tableau des achats a changé
        /// </summary>
        if (_AchatVisage)
        {
            _manager.Points -= _boutique.Color.PrixVisage[BoutiqueScript.Value];
            _skins.AcheteTextureVisage[BoutiqueScript.Value] = true;
            _visageBloque.renderer.enabled = false;

        }
        else
        {
            _manager.Points -= _boutique.Color.Prix[BoutiqueScript.Value];
            _skins.AcheteTexture[BoutiqueScript.Value] = true;
            _bombeMineBloquee.renderer.enabled = false;
        }
        _skins.MiseAJour = true;
        _glassPane.StopGlassPane();
        _boutonValider.SetActive(false);
        _boutonAnnuler.SetActive(false);
    }
}