using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelecteurToucheScript : MonoBehaviour
{
    private static List<SelecteurToucheScript> _touches;
    private bool _focus = false;
    private TextAreaScript _tas;
    private KeyCode _key;

    public KeyCode Key
    {
        get { return _key; }
        set
        {
            _key = value;
            _tas.SetText(_key.ToString());
        }
    }

    void Start()
    {
        if (_touches == null)
        {
            _touches = new List<SelecteurToucheScript>();
        }
        _touches.Add(this);
        _tas = GetComponent<TextAreaScript>();
    }

    /// <summary>
    /// Retrouve une touche appuyée quand le focus est sur l'élément
    /// Retrouve le KeyCode de la touche pressée
    /// Si cette touche est déjà assignée, les deux touches des actions sont échangées
    /// </summary>
    void Update()
    {
        if (Focus)
        {
            if (Input.anyKeyDown)
            {
                MiseAJourTouche();
            }
        }
    }

    void MiseAJourTouche()
    {
        KeyCode key = FetchKey();
        if (key != KeyCode.None)
        {
            foreach (SelecteurToucheScript touche in _touches)
            {
                if (touche.Key == key)
                {
                    touche.Key = _key;
                }
            }
            Key = key;
        }
    }

    /// <summary>
    /// Retourne le KeyCode de la touche pressée
    /// </summary>
    /// <returns>KeyCode de la touche pressée</returns>
    private KeyCode FetchKey()
    {
        for (int i = 0; i < 429; i++)
        {
            if (Input.GetKey((KeyCode)i))
            {
                return (KeyCode)i;
            }
        }

        return KeyCode.None;
    }

    /// <summary>
    /// Donne le focus à l'élément
    /// </summary>
    public bool Focus
    {
        get { return this._focus; }
        set
        {
            _focus = value;
            if (_focus)
            {
                _tas.SetText("Appuyez");
            }
            else
            {
                _tas.SetText(Key.ToString());
            }
            _tas.SetFocused(_focus);
        }
    }
}
