using UnityEngine;
using System.Collections;

public class ActiveModeScript : MonoBehaviour {

    [SerializeField]
    private GameObject _Network;
    [SerializeField]
    private GameObject _NetworkSolo;

    private ManagerScript _manager;

	void Start () 
    {
        _manager = ManagerScript.Instance;

        if (_manager.ModeDeJeuxSolo)
            _NetworkSolo.SetActive(true);
        else
            _Network.SetActive(true);
	}
	
}
