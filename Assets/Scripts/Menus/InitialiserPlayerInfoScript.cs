using UnityEngine;
using System.Collections;

public class InitialiserPlayerInfoScript : MonoBehaviour {

	[SerializeField]
	private int _index;
	[SerializeField]
	private TextAreaScript _tas;
	
	void Start ()
	{
		ManagerScript.Instance.InitializePlayerInfo(_index, _tas);
	}
}
