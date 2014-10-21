using UnityEngine;
using System.Collections;

public class SelecteurGroupeScript : MonoBehaviour {
	
	[SerializeField]
	private bool _increment;
	[SerializeField]
	private string[] _values;
	[SerializeField]
	private TextAreaScript _tas;
	[SerializeField]
	private ManagerScript _manager;
	
	private int _value = 0;

	void OnMouseUp ()
	{
		if(_increment)
		{
			if(_value == _values.Length - 1)
			{
				_value = 0;
			}
			else
			{
				_value++;
			}
		}
		else
		{
			if(_value == 0)
			{
				_value = _values.Length - 1;
			}
			else
			{
				_value--;
			}
		}
		_tas.SetText(_values[_value]);
		_manager.ChangeTeam(_value);
	}
}
