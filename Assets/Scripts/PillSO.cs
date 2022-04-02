using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pill", menuName = "Pills/Create New Pill")]
public class PillSO : ScriptableObject
{
    [SerializeField] public Colour colr;
    [SerializeField] public Shape shape;
    private SideEffect effects;

    public override int GetHashCode()
    {
        return colr.ColorType.GetHashCode() ^ shape.ShapeType.GetHashCode();
    }
    public override bool Equals(object obj)
    {
        return Equals(obj as PillSO);
    }
    public bool Equals(PillSO obj)
    {
        return obj != null 
            && obj.colr.ColorType == this.colr.ColorType 
            && obj.shape.ShapeType == this.shape.ShapeType;
    }
}
