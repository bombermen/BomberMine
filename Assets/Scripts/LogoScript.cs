using UnityEngine;
using System.Collections;

public class LogoScript : MonoBehaviour {

	
	void OnTriggerEnter()
	{
		Application.LoadLevel("Menu");	
	}
}
