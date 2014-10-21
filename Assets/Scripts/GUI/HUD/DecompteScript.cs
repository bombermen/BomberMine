using UnityEngine;
using System.Collections;
using System;

public class DecompteScript : MonoBehaviour {

    [SerializeField]
    private TextAreaScript _textArea;
	[SerializeField]
	private GestionPartieScript _gestionPartie;
    private int _seconds;

	public void StartDecompte ()
    {
		_seconds = Mathf.RoundToInt(_gestionPartie.TempsPhase2);
        StartCoroutine(RefreshTime());
	}

    private IEnumerator RefreshTime()
    {
        _textArea.SetText("<FF0000>" + (_seconds--).ToString());
        yield return new WaitForSeconds(1);
        if (_seconds < 0)
            _textArea.SetText("");
        else
            StartCoroutine(RefreshTime());
    }

}
