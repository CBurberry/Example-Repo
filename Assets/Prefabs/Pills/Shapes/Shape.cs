using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Pills/Shape")]
public class Shape : ScriptableObject
{
    [SerializeField] public GameObject prefab;
    [SerializeField] string shape;
}
