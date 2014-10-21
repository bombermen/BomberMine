using UnityEngine;
using System.Collections;

public class RadioButtonScript : MonoBehaviour {
	
	public RadioButtonScript[] _autresRadioButtonScripts;
	
	[SerializeField]
	private GameObject _checked;
	[SerializeField]
	private bool _selectionne = false;
	
	private HostData _data;

	public HostData Data {
		get {
			return this._data;
		}
		set {
			_data = value;
		}
	}	
	
	void Start()
	{
		_checked.SetActive(Selectionne);
	}

	public bool Selectionne {
		get { return this._selectionne; }
		set
		{
			_selectionne = value;
			_checked.SetActive(Selectionne);
		}
	}
	
	void OnMouseDown()
	{
		for(int i=0; i<_autresRadioButtonScripts.Length; i++)
		{
			_autresRadioButtonScripts[i].Selectionne = false;
		}
		Selectionne = true;
	}
}
