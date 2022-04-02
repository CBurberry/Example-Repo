using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<PillSO> pillsTemplate = new List<PillSO>();

    [SerializeField] List<Colour> colours = new List<Colour>();
    [SerializeField] List<Shape> shapes = new List<Shape>();

    [SerializeField] float range;
    [SerializeField] float spawnRate;
    [SerializeField] float spawnChance;
    [SerializeField] float maxDelay;

    [SerializeField] int spawnNumber = 1;

    
    void SpawnPill(PillSO pso)
    {
        
        GameObject pill = Instantiate(pso.shape.prefab, this.transform.position + new Vector3(Random.Range((float)-range,(float)range), 0, 0),
        pso.shape.prefab.transform.rotation);
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
        
        if (Time.time > spawnNumber * (1/spawnRate))
        {
            
            if(Random.Range(0, 1f)>(1-spawnChance))
            {
                SpawnPill(pillsTemplate[Random.Range(0, pillsTemplate.Count)]);
                
            }
            else
            {
                //SpawnRandom();
                StartCoroutine(WaitnSpawn());
            }
            spawnNumber++;
        }
    }    

    void SpawnRandom()
    {
        PillSO tempSO = CreatePill();
        SpawnPill(tempSO);
    }

    IEnumerator WaitnSpawn()
    {
        yield return new WaitForSeconds(Random.Range(0,(float)maxDelay));
        SpawnRandom();
    }
}
