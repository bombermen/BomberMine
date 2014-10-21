using UnityEngine;
using System.Collections;

public class SetFocusScript : MonoBehaviour {

    TextInputScript _tis;

    void Start()
    {
        _tis = this.GetComponent<TextInputScript>();
    }

    void OnMouseDown()
    {
        _tis.Focus = true;
    }

    void OnMouseExit()
    {
        _tis.Focus = false;
    }
}
