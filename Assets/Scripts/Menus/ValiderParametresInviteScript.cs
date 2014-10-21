using UnityEngine;
using System.Collections;

public class ValiderParametresInviteScript : MonoBehaviour
{

    [SerializeField]
    private GlassPaneScript _glassPane;
    [SerializeField]
    private TextAreaScript _textAreaError;
    private ManagerScript _manager;
    [SerializeField]
    private TextAreaScript _ipServeur;
    [SerializeField]
    private TextAreaScript _inputNomJoueur;
    [SerializeField]
    private GoToScript _boutonRetourSalleAttente;
    [SerializeField]
    private GameObject _boutonLancerPartie;
    [SerializeField]
    private GameObject _selecteurMap;
    [SerializeField]
    private CameraGUIMutliMenuScript _multiMenu;
    private bool _mouseOver = false;

    void OnMouseEnter()
    {
        _mouseOver = true;
    }

    void OnMouseExit()
    {
        _mouseOver = false;
    }
	
	void Start()
	{
		_manager = ManagerScript.Instance;	
	}

    void OnMouseUp()
    {
        if (_mouseOver)
        {
			_manager.PlayerName = _inputNomJoueur.GetText();
            Network.Connect(_ipServeur.GetText(), 6600);
            _glassPane.StartGlassPane();
        }
    }

    void OnFailedToConnect(NetworkConnectionError error)
    {
        _textAreaError.SetText("<FF0000>La connexion au serveur est impossible");
        _glassPane.StopGlassPane();
    }

    void OnConnectedToServer()
    {
        _textAreaError.SetText("");
        _boutonRetourSalleAttente.SubMenu = CameraGUIMutliMenuScript.Position.PARAMETRAGE_INVITE;
        _boutonLancerPartie.SetActive(false);
        _selecteurMap.SetActive(false);
        _multiMenu.GoToPosition(CameraGUIMutliMenuScript.Position.SALLE_ATTENTE);
        _glassPane.StopGlassPane();
    }
}
