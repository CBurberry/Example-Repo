using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Values in this class will be used for randomisation
[System.Serializable]
public class RandomSpawnProperties
{
    public List<Colour> colours = new List<Colour>();
    public List<Shape> shapes = new List<Shape>();

    //TODO
    //[SerializeField] float pillSpawnRate;
    //[SerializeField] float explosiveSpawnRate;
    //[SerializeField] float nonPillSpawnRate;
}

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

    private bool shouldSpawn = false;

    void SpawnPill(PillSO pso)
    {
        
        GameObject pill = Instantiate(pso.shape.prefab, this.transform.position + new Vector3(Random.Range((float)-range,(float)range), 0, 0),
        pso.shape.prefab.transform.rotation);
        pill.GetComponent<MeshRenderer>().material = pso.colr.material;
        pill.GetComponent<Pill>().pso = pso;
        Debug.Log(pso);
    }

    PillSO CreatePill()
    {
        PillSO newPill = ScriptableObject.CreateInstance<PillSO>();
        newPill.colr = colours[Random.Range(0, colours.Count)];
        newPill.shape = shapes[Random.Range(0, shapes.Count)];
        return (newPill);
    }

    public void SetRandomSpawnProperties(RandomSpawnProperties spawnProperties) 
    {
        colours = spawnProperties.colours;
        shapes = spawnProperties.shapes;
    }

    public void SetSpawningActive(bool value) 
    {
        shouldSpawn = value;
    }

    public void SetSpawnRate(float newRate) 
    {
        spawnRate = newRate;
    }

    // Update is called once per frame
    void Update()
    {
        //Don't spawn if this flag is set
        if (!shouldSpawn) 
        {
            return;
        }
        
        if (Time.time > spawnNumber * (0.1/spawnRate))
        {
            if(0 == Random.Range(0,8))
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
            }
            spawnNumber++;
            
        }
    }    

    void SpawnRandom()
    {
        PillSO tempSO = CreatePill();
        Debug.Log($"Spawning Random: {tempSO.colr}, {tempSO.shape}");
        SpawnPill(tempSO);
    }

    IEnumerator WaitnSpawn()
    {
        yield return new WaitForSeconds(Random.Range(0,(float)maxDelay));
        SpawnRandom();
    }
}
