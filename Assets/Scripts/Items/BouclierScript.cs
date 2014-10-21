using UnityEngine;
using System.Collections;

public class BouclierScript : MonoBehaviour {
	
	[SerializeField]
	private NetworkScript _networkScript;
	
	void OnTriggerEnter(Collider col)
	{
		if(Network.isServer && col.gameObject.layer == 8)
		{
			_networkScript.RecupererBouclier(col.transform);
		}
	}
}
