using UnityEngine;
using System.Collections;

public class DeconnexionScript : MonoBehaviour {

	void OnMouseUp ()
	{
		Network.Disconnect(250);
	}
}
