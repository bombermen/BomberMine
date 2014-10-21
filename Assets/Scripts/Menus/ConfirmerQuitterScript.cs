using UnityEngine;
using System.Collections;

public class ConfirmerQuitterScript : MonoBehaviour {

    [SerializeField]
    private GameObject _boutonAnnuler;
    [SerializeField]
    private GameObject _boutonQuitter;
    private bool _mouseOver = false;

    void OnMouseEnter()
    {
        _mouseOver = true;
    }

    void OnMouseExit()
    {
        _mouseOver = false;
    }

    void OnMouseUp()
    {
        Application.Quit();
    }
}
