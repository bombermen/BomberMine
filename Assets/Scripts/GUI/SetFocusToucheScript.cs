using UnityEngine;
using System.Collections;

public class SetFocusToucheScript : MonoBehaviour {

    SelecteurToucheScript _sts;

    void Start()
    {
        _sts = this.GetComponent<SelecteurToucheScript>();
    }

    void OnMouseDown()
    {
        _sts.Focus = true;
    }

    void OnMouseExit()
    {
        _sts.Focus = false;
    }
}
