using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Color
{
    Red, 
    Blue, 
    Green, 
    Yellow, 
    White
}

[CreateAssetMenu(menuName = "Pills/Colour")]
public class Colour : ScriptableObject
{
    [SerializeField] public Material material;
    [SerializeField] Color colour;

    public Color ColorType => colour;
}
