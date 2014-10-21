using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorTagScript
{
    int _beginning;
    Color _color;

    private static string[] COLOR_NAMES = { "black", "white", "red", "green", "blue" };
    private static Color[] COLORS = { Color.black, Color.white, Color.red, Color.green, Color.blue };

    public ColorTagScript(int beginning, string colorName)
    {
        _beginning = beginning;
        _color = HexaToColor(colorName);
    }

    public static Color HexaToColor(string hexaValue)
    {
        string red = hexaValue.Substring(0,2);
        string green = hexaValue.Substring(2,2);
        string blue = hexaValue.Substring(4, 2);

        float r = (float)int.Parse(red, System.Globalization.NumberStyles.HexNumber) / 255;
        float g = (float)int.Parse(green, System.Globalization.NumberStyles.HexNumber) / 255;
        float b = (float)int.Parse(blue, System.Globalization.NumberStyles.HexNumber) / 255;

        return new Color(r, g, b);
    }

    public static Color StringToColor(string colorString)
    {
        for (int i = 0; i < COLOR_NAMES.Length; i++)
        {
            if (COLOR_NAMES[i] == colorString)
            {
                return COLORS[i];
            }
        }
        return COLORS[0];
    }

    /// <summary>
    /// Retourne la couleur correspondant à l'indice index
    /// </summary>
    /// <param name="colorMarkups">La liste ordonnée des différents tags de couleur</param>
    /// <param name="index">Indice du caractère pour lequel on cherche la couleur</param>
    /// <returns>La couleur correspondant à l'indice index</returns>
    public static Color GetColor(List<ColorTagScript> colorTags, int index)
    {
        Color color = Color.white;
        foreach (ColorTagScript colorTag in colorTags)
        {
            if (index >= colorTag._beginning)
            {
                color = colorTag._color;
            }
        }
        return color;
    }
}
