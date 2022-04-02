using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Objective
{
    public PillSO Pill;
    public int Count;
}

[System.Serializable]
public class SpawnChances 
{
    public PillSO Pill;
    public float Chance;
}

[CreateAssetMenu(fileName = nameof(RoundData), menuName = "Gameplay/Create New Rounds Data SO")]
public class RoundData : ScriptableObject
{
    [SerializeField] List<Objective> objectives;

    [BoxGroup("Spawner")]
    [SerializeField] List<SpawnChances> spawnRates;
}
