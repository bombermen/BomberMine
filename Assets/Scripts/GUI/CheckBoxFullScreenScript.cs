using UnityEngine;
using System.Collections;

public class CheckBoxFullScreenScript : MonoBehaviour {

	private bool _coche = false;

	public bool Coche {
		get { return this._coche; }
		set {
			_coche = value;
			_checked.SetActive(_coche);
		}
	}

	[SerializeField]
	private GameObject _checked;
	
	void Start ()
    {
        Coche = Screen.fullScreen;
	}
	
	void OnMouseDown()
	{
		Coche = !Coche;
	}
}
