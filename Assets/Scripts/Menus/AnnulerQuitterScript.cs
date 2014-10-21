using UnityEngine;
using System.Collections;

public class AnnulerQuitterScript : MonoBehaviour {

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
        _glassPane.StopGlassPane();
        _boutonQuitter.SetActive(false);
        _boutonAnnuler.SetActive(false);
    }
}
