using UnityEngine;
using System.Collections;

public class InitializeSettings : MonoBehaviour {

    private ManagerScript _manager;
    private LoadAndSaveDataScript _loadSaveData;

    void Start()
    {
        //Get Data From XML
        _manager = ManagerScript.Instance;
        _loadSaveData = LoadAndSaveDataScript.Instance;
        _loadSaveData.LoadXml();

        _manager.Avancer = _loadSaveData.Avancer;
        _manager.Reculer = _loadSaveData.Reculer;
        _manager.Droite = _loadSaveData.Droite;
        _manager.Gauche = _loadSaveData.Gauche;
        _manager.Courir = _loadSaveData.Courir;
        _manager.Bombe = _loadSaveData.Bombe;
        _manager.Mine = _loadSaveData.Mine;
        _manager.Pause = _loadSaveData.Pause;
        _manager.Chat = _loadSaveData.Chat;
        _manager.Points = _loadSaveData.PlayerCredits;

        Resolution resolution = _loadSaveData.Resolution;
        bool fullscreen = _loadSaveData.FullScreen;
        int qualityLevel = _loadSaveData.Quality;

        Screen.SetResolution(resolution.width, resolution.height, fullscreen);
        QualitySettings.SetQualityLevel(qualityLevel);
    }
}
