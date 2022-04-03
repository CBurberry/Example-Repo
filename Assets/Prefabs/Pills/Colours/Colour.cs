using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pills/Colour")]
public class Colour : ScriptableObject
{
    public enum Color
    {
        Red,
        Blue,
        Green,
        Yellow,
        White
    };

    [SerializeField] public Material material;
    [SerializeField] Color colour;

    public Color ColorType => colour;
}
