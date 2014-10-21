using UnityEngine;
using System.Collections;

public class skinsScript : MonoBehaviour
{
    //Tableau des prix des textures pour les mines et bombes et les corps des personnages
    [SerializeField]
    private int[] prix;
    //Tableau des prix des visages des personnages 
    [SerializeField]
    private int[] prixVisage;
    //Tableau des noms des textures pour les mines et bombes et les corps des personnages
    [SerializeField]
    private string[] name;
    //Tableau des noms des visage pour les personnages
    [SerializeField]
    private string[] nameVisage;
    //Tableau des contenant les textures pour les mines et bombes et les corps des personnages
    [SerializeField]
    private Material[] texture;
    //Tableau des contenant les textures des visages des personnages 
    [SerializeField]
    private Material[] textureVisage;
    //Tableau des contenant les booléans des achats effectués concernant les mines et bombes et les corps des personnages 
    [SerializeField]
    private bool[] acheteTexture;
    //Tableau des contenant les booléans des achats effectués concernant les visages des personnages
    [SerializeField]
    private bool[] acheteTextureVisage;

    //variable qui permet d'indiquer au script SelecteurskinMulti qu'un achat a été réalisé
    private bool miseAJour = false;

    public string[] NameVisage
    {
        get { return nameVisage; }
        set { nameVisage = value; }
    }

    public int[] PrixVisage
    {
        get { return prixVisage; }
        set { prixVisage = value; }
    }

    public Material[] TextureVisage
    {
        get { return textureVisage; }
        set { textureVisage = value; }
    }

    public bool[] AcheteTextureVisage
    {
        get { return acheteTextureVisage; }
        set { acheteTextureVisage = value; }
    }

    public bool MiseAJour
    {
        get { return miseAJour; }
        set { miseAJour = value; }
    }

    public bool[] AcheteTexture
    {
        get { return acheteTexture; }
        set { acheteTexture = value; }
    }

    public int[] Prix
    {
        get { return prix; }
        set { prix = value; }
    }

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
