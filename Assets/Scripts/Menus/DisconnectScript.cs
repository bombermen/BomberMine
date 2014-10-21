using UnityEngine;
using System.Collections;

public class DisconnectScript : MonoBehaviour {
	
	private ManagerScript _manager;
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
	
	void OnMouseUp ()
	{
		_manager.Deconnecter();
	}
}
