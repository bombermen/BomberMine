using UnityEngine;
using System.Collections;

public class AchatScript : MonoBehaviour
{

    [SerializeField]
    private ManagerScript _manager;
    [SerializeField]
    private skinsScript _skins;
    //Bonton de retour 
    [SerializeField]
    private GameObject _boutonAnnuler;
    //Bonton de comfirmation
    [SerializeField]
    private GameObject _boutonValider;
    //fond d'écran
    [SerializeField]
    private GlassPaneScript _glassPane;
    [SerializeField]
    private ValiderAchat _validerAchat;
    [SerializeField]
    //Variable de vérification de bonton
    private bool _AchatVisage = false;

    void Start()
    {
        _manager = ManagerScript.Instance;
    }

    void OnMouseUp()
    {

        /// <summary>
        /// Vérifie sur quel bouton achat le joueur a appuyé
        /// en fonction du contenu de _AchatVisage
        /// </summary>
        _validerAchat.AchatVisage = _AchatVisage;
        //si le joueur n'a pas assez d'argent 
        if (_manager.Points < _skins.Prix[BoutiqueScript.Value])
        {
            //écrire une phrase au joueur pour lui indiquer 
            //qu'il n'a pas assez d'argent 
        }
        /// <summary>
        /// Initialisation de l'arène
        /// </summary>
        else if (_skins.AcheteTexture[BoutiqueScript.Value])
        {
            //écrire une phrase au joueur pour lui indiquer 
            //qu'il a déja acheté cette texture
            print("deja achete !!!"); 
        }

        /// <summary>
        /// Activation des boutons pour valider ou annuler
        /// </summary>
        else
        {
            _glassPane.StartGlassPane();
            _boutonValider.SetActive(true);
            _boutonAnnuler.SetActive(true);
        }

    }
}
