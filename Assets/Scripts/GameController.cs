using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [BoxGroup("User Interface")]
    [SerializeField] Canvas patientChartCanvas;
    [BoxGroup("User Interface")]
    [SerializeField] GridLayoutGroup medicinesList;

    [BoxGroup("Gameplay")]
    [SerializeField] List<RoundData> Rounds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewRound() 
    {

    }

    public void EndRound() 
    {

    }
}
