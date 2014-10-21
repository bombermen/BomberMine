using UnityEngine;
using System.Collections;

public class InitialiserMapScript : MonoBehaviour {

	[SerializeField]
	private MapScript _map;
	
	void Start ()
	{
		ManagerScript.Instance.Map = _map;
	}
}
