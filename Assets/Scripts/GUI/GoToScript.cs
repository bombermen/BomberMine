using UnityEngine;
using System.Collections;

public class GoToScript : MonoBehaviour
{

    [SerializeField]
    private CameraGUIMutliMenuScript _menuScript;
    [SerializeField]
    public CameraGUIMutliMenuScript.Position _subMenu;
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
        if (_mouseOver)
        {
            _menuScript.GoToPosition(_subMenu);
        }
    }

    public CameraGUIMutliMenuScript.Position SubMenu
    {
        get { return _subMenu; }
        set { _subMenu = value; }
    }
}
