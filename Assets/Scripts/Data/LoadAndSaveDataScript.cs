/// <summary>
/// Récupération et sauvegarde des données du joueur
/// gère son nom et le nombre de crédits au moyen d'un fichier xml spécifique au joueur
/// </summary>

using System.Collections;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;
using System;

public class LoadAndSaveDataScript : Singleton<LoadAndSaveDataScript>
{
    //Répertoire contenant le fichier de données du joueur
    private const string DATA_PATH = "Data";
    //Nom du fichier contenant les données joueur
    private const string DATA_FILENAME = "Data.xml";

    ManagerScript _manager;
    //Dernier nom utilisé par le joueur
    private string _playerName = "";
    //Nombre de crédits du joueur
    private int _playerCredits = 0;

    private Resolution _resolution;
    private int _quality;
    private bool _fullScreen;

    //touches
    private KeyCode _avancer;
    private KeyCode _reculer;
    private KeyCode _droite;
    private KeyCode _gauche;
    private KeyCode _courir;
    private KeyCode _bombe;
    private KeyCode _mine;
    private KeyCode _pause;
    private KeyCode _chat;


    /// <summary>
    /// Récupère les valeurs stockées dans le fichier
    /// contenant les données joueur <see cref="LoadAndSaveDataScript.DATA_PATH"/>/<see cref="LoadAndSaveDataScript.DATA_FILE"/>
    /// Ces valeurs sont ensuite récupérables via les accesseurs de cette classe :
    /// <see cref="PlayerName"/>
    /// </summary>
    public void LoadXml()
    {
        string filepath = DATA_PATH + "\\" + DATA_FILENAME;
        if (System.IO.File.Exists(filepath))
        {
            StreamReader r = File.OpenText(filepath);
            XmlTextReader xmlReader = new XmlTextReader(r);
            while (xmlReader.Read())
            {
                if (xmlReader.IsStartElement())
                {
                    if (xmlReader.Name == "player")
                    {
                        string playerName = xmlReader.GetAttribute("name");
                        string playerCredits = xmlReader.GetAttribute("credits");
                        _playerName = playerName;
                        _playerCredits = int.Parse(playerCredits);
                    }

                    if (xmlReader.Name == "inputs")
                    {
                        string avancer = xmlReader.GetAttribute("avancer");
                        string reculer = xmlReader.GetAttribute("reculer");
                        string droite = xmlReader.GetAttribute("droite");
                        string gauche = xmlReader.GetAttribute("gauche");
                        string courir = xmlReader.GetAttribute("courir");
                        string bombe = xmlReader.GetAttribute("bombe");
                        string mine = xmlReader.GetAttribute("mine");
                        string pause = xmlReader.GetAttribute("pause");
                        string chat = xmlReader.GetAttribute("chat");

                        _avancer = StringToKeyCode(avancer);
                        _reculer = StringToKeyCode(reculer);
                        _droite = StringToKeyCode(droite);
                        _gauche = StringToKeyCode(gauche);
                        _courir = StringToKeyCode(courir);
                        _bombe = StringToKeyCode(bombe);
                        _mine = StringToKeyCode(mine);
                        _pause = StringToKeyCode(pause);
                        _chat = StringToKeyCode(chat);
                    }

                    if (xmlReader.Name == "graphics")
                    {
                        string fullscreen = xmlReader.GetAttribute("fullscreen");
                        string quality = xmlReader.GetAttribute("quality_level");

                        _fullScreen = bool.Parse(fullscreen);
                        _quality = int.Parse(quality);
                    }

                    if (xmlReader.Name == "resolution")
                    {
                        string width = xmlReader.GetAttribute("width");
                        string height = xmlReader.GetAttribute("height");

                        _resolution.width = int.Parse(width);
                        _resolution.height = int.Parse(height);
                    }
                }
            }
            r.Close();
        }
        else
        {
            _playerName = "";
            _playerCredits = 0;
            SaveXml();
        }
    }

    /// <summary>
    /// Sauvegarde les valeurs stockées dans cette classe
    /// contenant les données joueur <see cref="LoadAndSaveDataScript.DATA_PATH"/>/<see cref="LoadAndSaveDataScript.DATA_FILE"/>
    /// Ces valeurs sont ensuite récupérables via les accesseurs de cette classe :
    /// <see cref="PlayerName"/>
    /// </summary>
    /// <param name="playerName">Nom du joueur</param>
    /// <param name="credits">Total de ses crédits (écrase la valeur précédente)</param>
    public void SaveXml()
    {
        XmlDocument doc = new XmlDocument();

        XmlElement root = (XmlElement)doc.AppendChild(doc.CreateElement("bombermine"));
        XmlElement player = (XmlElement)root.AppendChild(doc.CreateElement("player"));
        player.SetAttribute("name", _playerName);
        player.SetAttribute("credits", _playerCredits.ToString());

        XmlElement inputs = (XmlElement)root.AppendChild(doc.CreateElement("inputs"));
        inputs.SetAttribute("avancer", _avancer.ToString());
        inputs.SetAttribute("reculer", _reculer.ToString());
        inputs.SetAttribute("droite", _droite.ToString());
        inputs.SetAttribute("gauche", _gauche.ToString());
        inputs.SetAttribute("courir", _courir.ToString());
        inputs.SetAttribute("bombe", _bombe.ToString());
        inputs.SetAttribute("mine", _mine.ToString());
        inputs.SetAttribute("pause", _pause.ToString());
        inputs.SetAttribute("chat", _chat.ToString());

        XmlElement graphics = (XmlElement)root.AppendChild(doc.CreateElement("graphics"));
        graphics.SetAttribute("fullscreen", FullScreen.ToString());
        graphics.SetAttribute("quality_level", Quality.ToString());

        XmlElement resolution = (XmlElement)graphics.AppendChild(doc.CreateElement("resolution"));
        resolution.SetAttribute("width", _resolution.width.ToString());
        resolution.SetAttribute("height", _resolution.height.ToString());

        System.IO.File.WriteAllText("Data/Data.xml", doc.OuterXml);
    }

    /// <summary>
    /// Crée le répertoire <see cref="LoadAndSaveDataScript.DATA_PATH"/> s'il n'existe pas
    /// et le fichier initialisé <see cref="LoadAndSaveDataScript.DATA_FILE"/> s'il n'existe pas
    /// </summary>
    private void CheckIfFileExists()
    {
        string filepath = DATA_PATH + "\\" + DATA_FILENAME;
        //Création du répertoire Data s'il n'existe pas
        if (!System.IO.Directory.Exists(DATA_PATH))
        {
            System.IO.Directory.CreateDirectory(DATA_PATH);
        }

        if (!System.IO.File.Exists(filepath))
        {
            _playerName = "";
            _playerCredits = 0;
        }
    }

    private KeyCode StringToKeyCode(string keyString)
    {
        try
        {
            return (KeyCode)System.Enum.Parse(typeof(KeyCode), keyString);
        }
        catch (ArgumentNullException)
        {
            return KeyCode.None;
        }
    }

    public string PlayerName
    {
        get { return _playerName; }
        set { _playerName = value; }
    }

    public int PlayerCredits
    {
        get { return _playerCredits; }
        set { _playerCredits = value; }
    }

    public KeyCode Reculer
    {
        get { return _reculer; }
        set { _reculer = value; }
    }

    public KeyCode Droite
    {
        get { return _droite; }
        set { _droite = value; }
    }

    public KeyCode Gauche
    {
        get { return _gauche; }
        set { _gauche = value; }
    }

    public KeyCode Courir
    {
        get { return _courir; }
        set { _courir = value; }
    }

    public KeyCode Bombe
    {
        get { return _bombe; }
        set { _bombe = value; }
    }

    public KeyCode Mine
    {
        get { return _mine; }
        set { _mine = value; }
    }

    public KeyCode Pause
    {
        get { return _pause; }
        set { _pause = value; }
    }

    public KeyCode Chat
    {
        get { return _chat; }
        set { _chat = value; }
    }

    public KeyCode Avancer
    {
        get { return _avancer; }
        set { _avancer = value; }
    }

    public Resolution Resolution
    {
        get { return _resolution; }
        set { _resolution = value; }
    }

    public int Quality
    {
        get { return _quality; }
        set { _quality = value; }
    }

    public bool FullScreen
    {
        get { return _fullScreen; }
        set { _fullScreen = value; }
    }
}
