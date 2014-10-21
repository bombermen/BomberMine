using UnityEngine;
using System.Collections;

public class FoudroimentScript : MonoBehaviour {

    [SerializeField]
    private GameObject _map;
    [SerializeField]
    private NetworkScript _networkScript;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 8)
        {
            int playerIndex = _networkScript.GetPlayerIndexByTransform(col.transform);
            if (Network.isServer)
                _networkScript.DemanderFoudroiement(playerIndex);
            //else
                //_networkScript.networkView.RPC("DemanderFoudroiement", RPCMode.Server, playerIndex);

            this.gameObject.SetActive(false);
        }
    }

}
