using UnityEngine;
using System.Collections;

public class QuitterScript : MonoBehaviour {

    [SerializeField]
    private GameObject _boutonAnnuler;
    [SerializeField]
    private GameObject _boutonQuitter;
    [SerializeField]
    private GlassPaneScript _glassPane;
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
        _glassPane.StartGlassPane();
        _boutonQuitter.SetActive(true);
        _boutonAnnuler.SetActive(true);
    }
}
