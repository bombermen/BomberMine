using UnityEngine;
using System.Collections;
using System;

public class MessageScript
{

    private int _group;
    private string _text;
    private string _playerName;

    private DateTime _datePublication;

    public const int TO_ALL = -1;

    public void SetMessageToGroup(int groupe, string msg, string playerName)
    {
        _group = groupe;
        _text = msg;
        _datePublication = DateTime.Now;
        _playerName = playerName;
    }

    public void SetMessageToAll(string msg, string playerName)
    {
        _group = TO_ALL;
        _text = msg;
        _datePublication = DateTime.Now;
        _playerName = playerName;
    }

    public bool LisibleParEquipe(int groupe)
    {
        return (_group == TO_ALL) || (_group == groupe);
    }

    public string PlayerName
    {
        get { return _playerName; }
        set { _playerName = value; }
    }

    public int Group
    {
        get { return this._group; }
        set { _group = value; }
    }

    public DateTime DatePublication
    {
        get { return _datePublication; }
        set { _datePublication = value; }
    }

    public string Text
    {
        get { return this._text; }
        set { _text = value; }
    }
}
