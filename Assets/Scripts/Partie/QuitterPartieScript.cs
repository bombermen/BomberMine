using UnityEngine;
using System.Collections;

public class QuitterPartieScript : MonoBehaviour
{

    private ManagerScript _manager;
    private bool _mouseOver = false;

    void Start()
    {
        _manager = ManagerScript.Instance;
    }

    void OnMouseEnter()
    {
        _mouseOver = true;
    }

    void OnMouseExit()
    {
        _mouseOver = false;
    }

    void OnMouseUp()
    {
        if (_mouseOver)
        {
            if (Network.isServer)
                _manager.networkView.RPC("RetournerALEcranTitre", RPCMode.All);
            else
            {
                _manager.networkView.RPC("DeconnecterJoueur", RPCMode.Server, _manager.PlayerIndex);
                _manager.RetournerALEcranTitre();
            }
        }
    }
}
