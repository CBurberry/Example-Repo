using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pills/Colour")]
public class Colour : ScriptableObject
{
    [SerializeField] public Material material;
    [SerializeField] string colour;
}
