using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;



public class PlayerStats : MonoBehaviour
{
    public float nausea;
    public int heartRate;
    public float health;
    public float hallucination;
    public float lightSensitivity;

    public Volume volume;
    Bloom bloom;

    // Start is called before the first frame update
    void Start()
    {
        

        if(volume.profile.TryGet<Bloom>(out bloom))
        {
            //bloom.intensity.value = 100;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bloom.intensity.value = Mathf.PingPong(Time.time, lightSensitivity);
    }
}
