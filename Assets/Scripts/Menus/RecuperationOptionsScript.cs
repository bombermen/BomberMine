using UnityEngine;
using System.Collections;

/// <summary>
/// Met les touches à jour par rapport au xml
/// </summary>
public class RecuperationOptionsScript : MonoBehaviour
{
    private LoadAndSaveDataScript _loadSaveData;
    private ManagerScript _manager;
    private bool _mouseOver = false;

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

    /// <summary>
    /// Instanciation de LoadAndSaveDataScript
    /// </summary>
    void Start()
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

    /// <summary>
    /// Recupération des données du fichier xml et application au champ
    /// </summary>
    void OnMouseDown()
    {
        //Get Data From XML
        _loadSaveData.LoadXml();
        
        _avancer.Key = _loadSaveData.Avancer;
        _reculer.Key = _loadSaveData.Reculer;
        _droite.Key = _loadSaveData.Droite;
        _gauche.Key = _loadSaveData.Gauche;
        _courir.Key = _loadSaveData.Courir;
        _bombe.Key = _loadSaveData.Bombe;
        _mine.Key = _loadSaveData.Mine;
        _pause.Key = _loadSaveData.Pause;
        _chat.Key = _loadSaveData.Chat;

        _manager.Avancer = _loadSaveData.Avancer;
        _manager.Reculer = _loadSaveData.Reculer;
        _manager.Droite = _loadSaveData.Droite;
        _manager.Gauche = _loadSaveData.Gauche;
        _manager.Courir = _loadSaveData.Courir;
        _manager.Bombe = _loadSaveData.Bombe;
        _manager.Mine = _loadSaveData.Mine;
        _manager.Pause = _loadSaveData.Pause;
        _manager.Chat = _loadSaveData.Chat;
    }
}
