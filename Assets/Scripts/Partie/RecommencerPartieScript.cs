using UnityEngine;
using System.Collections;

public class RecommencerPartieScript : MonoBehaviour {

    private bool _mouseOver = false;

    void Start()
    {
        if (Network.isClient)
        {
            this.gameObject.SetActive(false);
        }
    }

    void OnMouseEnter()
    {
        _mouseOver = true;
    }

    void OnMouseExit()
    {
        _mouseOver = false;
    }

    void OnMouseDown()
	{
        if (_mouseOver)
        {
			if(Network.isServer)
				ManagerScript.Instance.networkView.RPC("RetournerEnSalleDAttente", RPCMode.All);
			else
				ManagerScript.Instance.RetournerEnSalleDAttente();
        }
    }
}
