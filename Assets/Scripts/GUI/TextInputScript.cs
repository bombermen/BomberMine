using UnityEngine;
using System.Collections;

public class TextInputScript : MonoBehaviour {
	
	private bool _focus = false;
	private TextAreaScript _tas;
	
	void Awake()
	{
		_tas = GetComponent<TextAreaScript>();
	}
	
	void Update()
	{
		if(Focus)
		{
			string inputString = Input.inputString;
			if(Input.GetKeyUp(KeyCode.Backspace))
			{
				_tas.EffacerDerniereLettre();
			}
			else if(inputString != "" && inputString != "\b" && inputString != "\n"  && inputString != "\r")
			{
				_tas.Append(inputString);
			}
		}
	}
	
	public bool Focus {
		get { return this._focus; }
		set
		{
			_focus = value;
			_tas.SetFocused(_focus);
		}
	}
}
