/*
 * project : Out Of Breath
 * Groupe :  LRStudio
 * Created by : Reminy Pascal
 * Edited by : Reminy Pascal & Laurent Bastien 
 * Description : script for capture the mouse vertical move
*/

using UnityEngine;
using System.Collections;

public class mouse : MonoBehaviour {
	
	private float _verticalRotation;	//Variable to stock the value of the vertical rotation
	private float _mouseSensitivity;	//Variable to stock the value of the mouse sensibility
	private float _upDownRange;			//Variable to stock the value of the limit angle of the vertical rotation
	//private float _rotLeftRight;
	
	// Use this for initialization
	void Start () {
	
		_verticalRotation = 0;
		_mouseSensitivity = 80.0f;	//Sensibility of the mouse
		_upDownRange = 90.0f;		//Limit of the vertical rotation 
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		//_rotLeftRight = Input.GetAxis ("Mouse X") * _mouseSensitivity;
		//transform.Rotate(0,_rotLeftRight,0);
		
		_verticalRotation -= Input.GetAxis ("Mouse Y") * _mouseSensitivity * Time.deltaTime;	//Take the value of the vertical rotation
		_verticalRotation = Mathf.Clamp (_verticalRotation,-_upDownRange,_upDownRange);		//Limit the angle of the rotation
		transform.localEulerAngles = new Vector3(_verticalRotation, 0, 0);	//Store the value of the angle rotation
	}
}
