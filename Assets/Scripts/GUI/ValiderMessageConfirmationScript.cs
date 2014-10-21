using UnityEngine;
using System.Collections;

public class ValiderMessageConfirmationScript : MonoBehaviour {

    [SerializeField]
    private GameObject _messageConfirmation;
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
        _messageConfirmation.SetActive(false);
        _glassPane.StopGlassPane();
    }
}
