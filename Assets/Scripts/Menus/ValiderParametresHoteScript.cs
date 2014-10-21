using UnityEngine;
using System.Collections;

public class ValiderParametresHoteScript : MonoBehaviour {
	
	private ManagerScript _manager;
	[SerializeField]
	private TextAreaScript _inputNomJoueur;
	[SerializeField]
	private GoToScript _boutonRetourSalleAttente;
	[SerializeField]
	private CameraGUIMutliMenuScript _menuScript;
	[SerializeField]
	private TextAreaScript _errorOutput;
	[SerializeField]
	private GameObject _boutonLancerPartie;
    [SerializeField]
    private GameObject _selecteurMap;
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
            NetworkConnectionError error = Network.InitializeServer(4, 6600, false);

            if (error == NetworkConnectionError.NoError)
            {
                _boutonRetourSalleAttente.SubMenu = CameraGUIMutliMenuScript.Position.MULTIMENU;
                _boutonLancerPartie.SetActive(true);
                _selecteurMap.SetActive(true);
                _menuScript.GoToPosition(CameraGUIMutliMenuScript.Position.SALLE_ATTENTE);
                _errorOutput.SetText("");
            }
            else
            {
                _errorOutput.SetText("<FF0000>Impossible de créer le serveur");
            }
        }
	}
}
