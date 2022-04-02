using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pill", menuName = "Pills/Create New Pill")]
public class PillSO : ScriptableObject
{
    [SerializeField] public Colour colr;
    [SerializeField] public Shape shape;
    private SideEffect effects;


}
