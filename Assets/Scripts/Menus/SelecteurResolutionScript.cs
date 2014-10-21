using UnityEngine;
using System.Collections;

public class SelecteurResolutionScript : MonoBehaviour
{
	
	[SerializeField]
	private bool _increment;
	[SerializeField]
	private string[] _values;
	[SerializeField]
	private TextAreaScript _tas;
	[SerializeField]
	private ManagerScript _manager;

    private Resolution[] _resolutions;
    private static int _value = 0;

    public static int Value
    {
        get { return SelecteurResolutionScript._value; }
        set { SelecteurResolutionScript._value = value; }
    }

    void Start()
    {
        _values = new string[Screen.resolutions.Length];
        _resolutions = Screen.resolutions;

        for (int i = 0; i < _resolutions.Length; i++)
        {
            _values[i] = _resolutions[i].width.ToString() + " x " + _resolutions[i].height.ToString();
        }

        _tas.SetText("<000000>" + _values[_value]);
    }

    void OnMouseUp()
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

		_tas.SetText("<000000>" + _values[_value]);
	}
}
