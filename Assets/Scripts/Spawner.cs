using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    List<PillSO> pillsTemplate = new List<PillSO>();
    void SpawnPill(PillSO pso)
    {
        GameObject pill = Instantiate(pso.shape.prefab);
        pill.GetComponent<MeshRenderer>().material = pso.colr.material;
    }

    // Update is called once per frame
    void Update()
    {

    }    
    void Start()
    {
        
    }
}
