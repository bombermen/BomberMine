using UnityEngine;
using System.Collections;
using System;

public class TimerScript : MonoBehaviour {

    [SerializeField]
    private TextAreaScript _textArea;
    private bool _stop = false;
    private int _seconds = 0;

    public bool Stop
    {
        get { return _stop; }
        set { _stop = value; }
    }

	void Start ()
    {
        StartCoroutine(RefreshTime());
	}

    private IEnumerator RefreshTime()
    {
        TimeSpan ts = TimeSpan.FromSeconds(_seconds++);
        _textArea.SetText(FormatToTimer(ts));
        yield return new WaitForSeconds(1);
        if(!Stop)
            StartCoroutine(RefreshTime());
    }

    string FormatToTimer(TimeSpan ts)
    {
        return string.Format("{0:D2}:{1:D2}", ts.Minutes, ts.Seconds);
    }
}
