using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    void SpawnPill(Colour cl, Shape shape)
    {
        GameObject pill = Instantiate(shape.prefab);
        pill.GetComponent<MeshRenderer>().material = cl.material;
    }

    // Update is called once per frame
    void Update()
    {

    }    
    void Start()
    {
        
    }
}
