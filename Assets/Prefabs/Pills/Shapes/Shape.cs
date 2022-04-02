using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Shapes
{
    Diamond,
    Capsule,
    Rounded,
    Cylinder,
    Oblong,
    Pentagon
}

[CreateAssetMenu(menuName = "Pills/Shape")]
public class Shape : ScriptableObject
{
    [SerializeField] public GameObject prefab;
    [SerializeField] Shapes shape;

    public Shapes ShapeType => shape;
}
