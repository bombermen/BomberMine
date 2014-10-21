using UnityEngine;
using System.Collections;

public class CapsuleEntreeColliderScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _colliderCapsuleFermee;
    [SerializeField]
    private GameObject _colliderCapsuleOuverte;
    [SerializeField]
    private CapsuleScript _capsuleScript;
    [SerializeField]
    NetworkScript _networkScript;

    public void Reset()
    {
        _colliderCapsuleFermee.SetActive(false);
        _colliderCapsuleOuverte.SetActive(true);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == 8)
        {
            if (Network.isServer)
            {
                _colliderCapsuleFermee.SetActive(true);
                _colliderCapsuleOuverte.SetActive(false);
                //freeze du joueur
                _networkScript.PlacerJoueurDansCapsule(true, col.transform, _capsuleScript.IdCapsule);
                _capsuleScript.CloseDoors();
            }
        }
    }
}
