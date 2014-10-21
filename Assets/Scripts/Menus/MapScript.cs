using UnityEngine;
using System.Collections;

public class MapScript : MonoBehaviour {

    //Tableau de string du nom des scènes de jeu disponibles
    [SerializeField]
    private string[] name;
    //Tableau des images des scènes de jeu disponibles
    [SerializeField]
    private Material[] texture;

    public string[] Name
    {
        get { return name; }
        set { name = value; }
    }

    public Material[] Texture
    {
        get { return texture; }
        set { texture = value; }
    }
}
