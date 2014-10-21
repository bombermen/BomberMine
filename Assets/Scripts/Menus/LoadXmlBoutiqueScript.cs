using UnityEngine;
using System.Collections;

public class LoadXmlBoutiqueScript : MonoBehaviour
{

    [SerializeField]
    private TextAreaScript _textCredit;
    private ManagerScript _manager;
    private LoadAndSaveDataScript _loadSaveData;
    private bool _mouseOver = false;

    void Awake()
    {
        _loadSaveData = LoadAndSaveDataScript.Instance;
         _manager = ManagerScript.Instance;
    }

    void OnMouseEnter()
    {
        _mouseOver = true;
    }

    void OnMouseExit()
    {
        _mouseOver = false;
    }

    void OnMouseDown()
    {
        _loadSaveData.LoadXml();
        _textCredit.SetText("Crédits : " + _loadSaveData.PlayerCredits);
    }
}
