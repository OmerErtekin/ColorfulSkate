using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Palette", menuName = "ScriptableObjects/ColorPalette", order = 1)]
public class ColorPalette : ScriptableObject
{
    public List<Color> colors;
    public List<Material> materials;
}

public enum ColorFromPalette
{
    Red,
    Blue,
    Green,
    Yellow,
    Purple,
    White,
}
