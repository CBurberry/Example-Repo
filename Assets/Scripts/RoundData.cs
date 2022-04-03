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

[CreateAssetMenu(fileName = nameof(RoundData), menuName = "Gameplay/Create New Rounds Data SO")]
public class RoundData : ScriptableObject
{
    [SerializeField] List<Objective> objectives;

    [BoxGroup("Spawner")]
    [SerializeField] RandomSpawnProperties spawnProperties;

    [SerializeField] float roundDuration;
    [SerializeField] float conveyorSpeed;

    public List<Objective> Objectives => objectives;
    public RandomSpawnProperties SpawnProperties => spawnProperties;
    public float RoundDuration => roundDuration;
    public float ConveyorSpeed => conveyorSpeed;
}
