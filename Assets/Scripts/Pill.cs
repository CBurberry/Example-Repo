using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pill : MonoBehaviour
{
    [SerializeField] public PillSO pso;

    public PillSO GetData() 
    {
        return pso;
    }
}
