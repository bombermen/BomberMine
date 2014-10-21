using UnityEngine;
using System.Collections;

public class ValiderOptionsScript : MonoBehaviour {

    [SerializeField]
    private GlassPaneScript _glassPane;
    [SerializeField]
    private GameObject _messageConfirmation;
    [SerializeField]
    private CheckBoxFullScreenScript _fullScreen;
 
    [SerializeField]
    private SelecteurToucheScript _avancer;
    [SerializeField]
    private SelecteurToucheScript _reculer;
    [SerializeField]
    private SelecteurToucheScript _droite;
    [SerializeField]
    private SelecteurToucheScript _gauche;
    [SerializeField]
    private SelecteurToucheScript _courir;
    [SerializeField]
    private SelecteurToucheScript _bombe;
    [SerializeField]
    private SelecteurToucheScript _mine;
    [SerializeField]
    private SelecteurToucheScript _pause;
    [SerializeField]
    private SelecteurToucheScript _chat;

    private ManagerScript _manager;
    private LoadAndSaveDataScript _loadSaveData;

    private bool _mouseOver = false;

    void Start()
    {
        _manager = ManagerScript.Instance;
        _loadSaveData = LoadAndSaveDataScript.Instance;
    }

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

        Resolution resolution = Screen.resolutions[SelecteurResolutionScript.Value];
        bool fullScreen = _fullScreen.Coche;
        Screen.SetResolution(resolution.width, resolution.height, fullScreen);
        QualitySettings.SetQualityLevel(SelecteurQualiteScript.Value);

        _loadSaveData.LoadXml();

        _loadSaveData.Avancer = _avancer.Key;
        _loadSaveData.Reculer = _reculer.Key;
        _loadSaveData.Droite = _droite.Key;
        _loadSaveData.Gauche = _gauche.Key;
        _loadSaveData.Courir = _courir.Key;
        _loadSaveData.Bombe = _bombe.Key;
        _loadSaveData.Mine = _mine.Key;
        _loadSaveData.Pause = _pause.Key;
        _loadSaveData.Chat = _chat.Key;

        _loadSaveData.FullScreen = _fullScreen.Coche;
        _loadSaveData.Quality = SelecteurQualiteScript.Value;
        _loadSaveData.Resolution = Screen.GetResolution[SelecteurResolutionScript.Value];

        _loadSaveData.SaveXml();

        _messageConfirmation.SetActive(true);
    }
}
