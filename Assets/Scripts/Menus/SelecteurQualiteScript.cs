using UnityEngine;
using System.Collections;

public class SelecteurQualiteScript : MonoBehaviour
{
	[SerializeField]
	private bool _increment;
	[SerializeField]
	private static string[] _values = { "Plus rapide", "Rapide", "Simple", "Bonne", "Belle", "Fantastique" };
	[SerializeField]
	private TextAreaScript _tas;
	[SerializeField]
	private ManagerScript _manager;

    private static int _value = 0;

    public static int Value
    {
        get { return SelecteurQualiteScript._value; }
        set { SelecteurQualiteScript._value = value; }
    }

    void Start()
    {
        _value = QualitySettings.GetQualityLevel();
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
