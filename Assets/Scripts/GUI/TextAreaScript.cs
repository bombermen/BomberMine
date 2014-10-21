using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public class TextAreaScript : MonoBehaviour
{
    public int _caracteresParLigne = 20;
    public int _nbLignes = 10;
    public float _espacement = .2f;
    public bool _afficherFond = false;
    public bool _centrer = false;
    public GameObject _fond;
    public AlphabetScript _alphabet;

    public string _text;
    private string _textBrut = "";
    private List<ColorTagScript> _colorTags;
    private string _lignes;
    private Transform _transform;
    private GameObject[] _objetsInstancies;
    private GameObject[][] _tuileFondInstancies;
    private Vector3 _scale;
    private int _lignesDansLeTexte;
    
    void Awake()
    {
        _transform = this.transform;
        _scale = _transform.localScale;
        _objetsInstancies = new GameObject[_caracteresParLigne * _nbLignes];
        _tuileFondInstancies = new GameObject[_nbLignes][];
        for (int i = 0; i < _nbLignes; i++)
            _tuileFondInstancies[i] = new GameObject[_caracteresParLigne];

        if (_afficherFond)
        {
            AfficherFond();
        }
        SetText(_text);
    }

    private void AfficherFond()
    {
        _transform.localScale = new Vector3(1f, 1f, 1f);
        for (int i = 0; i < _nbLignes; i++)
        {
            for (int j = 0; j < _caracteresParLigne; j++)
            {
                Vector3 position = _transform.position;
                position.x -= j * _espacement;
                position.y -= i * 1;
                _tuileFondInstancies[i][j] = Instantiate(_fond, position, Quaternion.identity) as GameObject;
                _tuileFondInstancies[i][j].layer = 14;
                _tuileFondInstancies[i][j].transform.parent = _transform;
            }
        }
        _transform.localScale = _scale;
    }

    /// <summary>
    /// Detruit les objets instanciés.
    /// </summary>
    void ReinitialiserObjetsInstancies()
    {
        if (_objetsInstancies != null)
        {
            for (int i = 0; i < _objetsInstancies.Length; i++)
                if (_objetsInstancies[i])
                    Destroy(_objetsInstancies[i]);
        }
    }

    /// <summary>
    /// Sets the text.
    /// Affiche le texte dans les limites spécifiées dans les attributs publics.
    /// </summary>
    /// <param name='text'>
    /// _text.
    /// </param>
    public void SetText(string text)
    {
        Vector3 positionBase = _transform.position;
        Vector3 position = new Vector3(0, 0, positionBase.z);
        int ligne = 0;
        int colonne = 0;

        _text = MiseEnForme(text);

        _transform.localScale = new Vector3(1f, 1f, 1f);

        ReinitialiserObjetsInstancies();

        //centrage de la position
        if (_centrer)
        {
            if (_text.Length <= _caracteresParLigne)
            {
                positionBase.x += _text.Length * _espacement / 2;
            }
            else
            {
                positionBase.x += _caracteresParLigne * _espacement / 2;
            }
        }

        int debut = PremierCaractereAAfficher();

        //instantiation des lettres
        for (int i = debut; i < _text.Length; i++)
        {
            Color color = ColorTagScript.GetColor(_colorTags, i);
            if (_text[i] == '\n')
            {
                ligne++;
                colonne = 0;
                if (ligne > _nbLignes)
                    break;
            }
            else
            {
                position.x = positionBase.x - colonne * _espacement;
                position.y = positionBase.y - ligne * 1;
                InstancierLettre(position, _text[i], i - debut, color);
                colonne++;
            }
        }

        _transform.localScale = _scale;
    }

    GameObject InstancierLettre(Vector3 position, char lettreACreer, int i, Color color)
    {
        if (i >= _objetsInstancies.Length)
        {
            return null;
        }

        GameObject lettre = _alphabet.CharToGameObject(lettreACreer);
        if (lettre != null)
        {
            lettre = Instantiate(lettre, position, Quaternion.identity) as GameObject;
            lettre.transform.parent = _transform;
            lettre.layer = 14;
            lettre.renderer.materials[0].color = color;
        }
        _objetsInstancies[i] = lettre;
        return lettre;
    }

    private int PremierCaractereAAfficher()
    {
        if (_lignesDansLeTexte <= _nbLignes)
        {
            return 0;
        }

        int premiereLigne = _lignesDansLeTexte - _nbLignes;
        int ligneCourante = 1;

        for (int i = 0; i < _text.Length; i++)
        {
            if (_text[i] == '\n')
            {
                if (ligneCourante == premiereLigne)
                {
                    return i + 1;
                }
                ligneCourante++;
            }
        }
        return 0;
    }

    /// <summary>
    /// Mises en forme.
    /// Permet de mettre a la ligne les mots qui seraient coupés
    /// sans cela.
    /// </summary>
    /// <returns>
    /// Texte en forme.
    /// </returns>
    /// <param name='text'>
    /// minuscule à mettre en forme.
    /// </param>
    /// 
    private string MiseEnForme(string text)
    {
        _textBrut = text;
        _colorTags = new List<ColorTagScript>();
        string resultat = "";
        int caractereDansLigneCourante = 0;
        int caractereCourant = 0;
        _lignesDansLeTexte = 0;

        //Séparation des balises de couleur et des retours à la ligne
        text = text.Replace("<", " <");
        text = text.Replace(">", "> ");
        text = text.Replace("\n", " \n ");

        //Supprime tous les espaces entre chevrons
        Regex matchTag = new Regex("<(?<tagname>[^>]*)>");
        foreach (Match m in matchTag.Matches(text))
        {
            text = text.Replace(m.Value, m.Value.Replace(" ", ""));
        }

        string[] mots = text.Split(new char[] { ' ' });

        for (int i = 0; i < mots.Length; i++)
        {
            //Traitement des balises de couleur
            if (mots[i].Length > 0 && mots[i][0] == '<')
            {
                //Effacement du dernier espace ajouté par défaut
                caractereDansLigneCourante--;
                caractereCourant--;
                string colorName = mots[i].Replace("<", "").Replace(">", "");
                _colorTags.Add( new ColorTagScript(caractereCourant, colorName) );
                
                resultat = resultat.Substring(0, resultat.Length - 1);
            }
            //Traitement du texte
            else
            {
                caractereDansLigneCourante += mots[i].Length + 1;
                caractereCourant += mots[i].Length + 1;

                //Cas ou le retour à ligne provient directement du texte
                if (mots[i] == "\n")
                {
                    resultat += "\n";
                    _lignesDansLeTexte++;
                    caractereDansLigneCourante = 0;
                    caractereCourant--;
                }
                //Ajout d'un mot si le nombre de caractères tient dans la ligne actuelle
                else if (caractereDansLigneCourante <= _caracteresParLigne)
                {
                    resultat += mots[i] + " ";
                }
                //Cas ou le retour à la ligne est du au dépassement du nombre de caractères de la ligne actuelle
                else
                {
                    if (mots[i].Length > 1)
                    {
                        resultat = resultat.Substring(0, resultat.Length - 1);
                    }
                    resultat += "\n";
                    _lignesDansLeTexte++;
                    resultat += mots[i] + " ";
                    caractereDansLigneCourante = mots[i].Length + 1;
                }
            }
        }

        _lignesDansLeTexte++;

        return resultat;
    }

    public void Append(string str)
    {
        if (SupprimerBalises(_textBrut).Length + str.Length <= _caracteresParLigne)
        {
            _textBrut += str;
            SetText(_textBrut);
        }
    }

    public string SupprimerBalises(string str)
    {
        string text = str;

        //Supprime toutes les balises <balise>
        Regex matchTag = new Regex("(?<tag><[^>]*[>]?)");
        foreach (Match m in matchTag.Matches(text))
        {
            string tag = m.Groups["tag"].Value;
            text = text.Replace(m.Value, m.Value.Replace(tag, ""));
        }

        return text;
    }

    public void EffacerDerniereLettre()
    {
        if (_textBrut != "")
        {
            _textBrut = _textBrut.Substring(0, _textBrut.Length - 1);
        }
        SetText(_textBrut);
    }

    public void SetFocused(bool focus)
    {
        Color color = focus ? Color.red : Color.black;
        for (int i = 0; i < _nbLignes; i++)
            for (int j = 0; j < _caracteresParLigne; j++)
                _tuileFondInstancies[i][j].renderer.material.color = color;
    }

    public string GetText()
    {
        return SupprimerBalises(_textBrut);
    }
}
