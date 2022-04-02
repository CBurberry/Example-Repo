using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<PillSO> pillsTemplate = new List<PillSO>();

    [SerializeField] List<Colour> colours = new List<Colour>();
    [SerializeField] List<Shape> shapes = new List<Shape>();

    [SerializeField] float range;


    void SpawnPill(PillSO pso)
    {
        GameObject pill = Instantiate(pso.shape.prefab, this.transform);
        pill.GetComponent<MeshRenderer>().material = pso.colr.material;
    }

    PillSO CreatePill()
    {
        PillSO newPill = ScriptableObject.CreateInstance<PillSO>();
        newPill.colr = colours[Random.Range(0, colours.Count)];
        newPill.shape = shapes[Random.Range(0, shapes.Count)];
        return (newPill);
    }




    // Update is called once per frame
    void Update()
    {

    }    
    void Start()
    {
        PillSO tempSO = CreatePill();
        SpawnPill(tempSO);
    }
}
