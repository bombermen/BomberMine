using UnityEngine;
using System.Collections;

public class LancerPartieScript : MonoBehaviour {

    //Variable pour réccupérer les différentes maps disponibles
    [SerializeField]
    private MapScript _map;
    private ManagerScript _manager;
    [SerializeField]
    private GameObject _boutonFond;
    //Bonton de comfirmation
    [SerializeField]
    private GameObject _boutonOk;
    //fond d'écran
    [SerializeField]
    private GlassPaneScript _glassPane;

    private int _choix;

    void Start()
    {
        if (Network.isClient)
            this.transform.parent.gameObject.SetActive(false);
        _manager = ManagerScript.Instance;
    }

    public int Choix
    {
        get { return _choix; }
        set { _choix = value; }
    }

    /// <summary>
    /// Charge la scène choisie
    /// </summary>
    void OnMouseUp()
    {
        if (Network.isServer)
            _manager.networkView.RPC("LancerPartie", RPCMode.All, _map.Name[Choix]);
        else
        {
            _manager.ModeDeJeuxSolo = true;
            Application.LoadLevel(_map.Name[_choix]);
           // _glassPane.StartGlassPane();
            //_boutonFond.SetActive(true);
            //_boutonOk.SetActive(true);
        }
    }
}
