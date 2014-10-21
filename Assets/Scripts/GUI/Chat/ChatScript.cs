using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChatScript : MonoBehaviour
{
    private ManagerScript _manager;
    [SerializeField]
    private TextAreaScript _textInput;
    [SerializeField]
    private TextAreaScript _textAreaMessages;
    [SerializeField]
    private TextInputScript _tis;
    private GameObject _textInputGameObject;
    
    private int MAX_MESSAGES = 9;
    private bool _chatActif;

    private List<MessageScript> _AllMessages;
    private List<MessageScript> _messages;

    void Start()
    {
        _manager = ManagerScript.Instance;
        _messages = new List<MessageScript>();
        _textInputGameObject = _tis.gameObject;
        ChatActif = false;
    }

    public bool ChatActif
    {
        get { return _chatActif; }
        set
        {
            _textInputGameObject.SetActive(value);
            _chatActif = value;
            _tis.Focus = value;
        }
    }

    void Update()
    {
        if (ChatActif)
        {
            if (Input.GetKeyDown(KeyCode.Return) && _textInput.GetText() != "")
            {
                if (Network.isClient)
                {
                    this.networkView.RPC("EnvoyerMessage", RPCMode.Server, _manager.PlayerIndex, _textInput.GetText());
                }
                else
                {
                    EnvoyerMessage(_manager.PlayerIndex, _textInput.GetText());
                }
                _textInput.SetText("");
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                ChatActif = false;
            }
        }
        else if(Input.GetKeyUp(KeyCode.T))
        {
            ChatActif = true;
        }
    }

    string MessagesToText()
    {
        string resultat = "";

        foreach (MessageScript message in _messages)
        {
            resultat += "<0000AA>";
            resultat += message.DatePublication.ToString("'['HH':'mm']'");
            resultat += "<00AA00>";
            resultat += message.PlayerName + ":";
            if (message.Group == MessageScript.TO_ALL)
            {
                resultat += "<FFFFFF>";
            }
            else
            {
                resultat += "<FFAAAA>";
            }
            resultat += message.Text + "\n";
        }

        return resultat;
    }

    [RPC]
    void EnvoyerMessage(int playerIndex, string message)
    {
        if (Network.isServer)
        {
            MessageScript msg = new MessageScript();

            //Recuperation de commande /
            string[] mots = message.Split(new char[] { ' ' });

            PlayerScript joueur = _manager.Players[playerIndex];
            string playerName = joueur.Nom;

            //groupe
            if (mots[0] == "/g")
            {
                int team = joueur.Team;
                string corpsDuMessage = message.Substring(mots[0].Length + 1);
                
                List<NetworkPlayer> players = _manager.GetPlayersByTeam(team);
                foreach (NetworkPlayer p in players)
                {
                    if (Network.player == p)
                    {
                        AjouterMessage(corpsDuMessage, team, playerName);
                    }
                    else
                    {
                        this.networkView.RPC("AjouterMessage", p, corpsDuMessage, team, playerName);
                    }
                }
            }
            else
            {
                msg.SetMessageToAll(message, joueur.Nom);
                this.networkView.RPC("AjouterMessage", RPCMode.All, msg.Text, msg.Group, playerName);
            }
        }
    }

    [RPC]
    void AjouterMessage(string message, int groupe, string playerName)
    {
        MessageScript msg = new MessageScript();
        msg.SetMessageToGroup(groupe, message, playerName);
        if (_messages.Count >= MAX_MESSAGES)
        {
            _messages.RemoveAt(0);
        }
        _messages.Add(msg);
        _textAreaMessages.SetText(MessagesToText());
        _textInput.SetText("");
    }
}
