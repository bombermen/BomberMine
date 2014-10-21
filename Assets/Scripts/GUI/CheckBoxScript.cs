using UnityEngine;
using System.Collections;

public class CheckBoxScript : MonoBehaviour {
	
	[SerializeField]
	private bool _coche = false;

	public bool Coche {
		get { return this._coche; }
		set {
			_coche = value;
			_checked.SetActive(Coche);
		}
	}

	[SerializeField]
	private GameObject _checked;
	
	// Use this for initialization
	void Start () {
		_checked.SetActive(Coche);
	}
	
	void OnMouseDown()
	{
		Coche = !Coche;
	}
}
