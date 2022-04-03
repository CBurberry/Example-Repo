using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Utility.DeveloperConsole;

public class GameController : MonoBehaviour
{
    [BoxGroup("User Interface")]
    [SerializeField] PatientChart patientChart;
    [BoxGroup("User Interface")]
    [SerializeField] GridLayoutGroup medicinesList;
    [BoxGroup("User Interface")]
    [SerializeField] Text countdownText;

    [BoxGroup("Gameplay")]
    [SerializeField] List<RoundData> Rounds;
    [BoxGroup("Gameplay")]
    [SerializeField] float roundDuration = 60f;             //N.B. We may want to consider this as a difficulty parameter
    [BoxGroup("Gameplay")]
    [SerializeField] ConsumeZone scoringZone;
    [BoxGroup("Gameplay")]
    [SerializeField] Spawner spawner;
    [BoxGroup("Gameplay")] 
    [SerializeField] simpleConveyor conveyor;

    private int currentRound;
    private RoundData activeRound;
    private float roundElapsedTime;

    //Debug
    [Header("Debug")]
    [SerializeField] DeveloperConsoleBehaviour devConsole;

    //User Collected Items
    private Dictionary<PillSO, int> collectedMedication;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        currentRound = -1;

        if (Instance != null)
        {
            Destroy(this);
        }
        else 
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        collectedMedication = new Dictionary<PillSO, int>();
        scoringZone.OnAdded += OnItemScored;
        scoringZone.OnDropped += OnItemDropped;
        StartNewRound();
    }

    private void Update()
    {
        if (activeRound != null) 
        {
            roundElapsedTime += Time.deltaTime;
            float timeRemaining = Mathf.Clamp(roundDuration - roundElapsedTime, 0, roundDuration);
            countdownText.text = Mathf.FloorToInt(timeRemaining).ToString();
        }

#if UNITY_EDITOR
        if (Input.GetKeyDown(devConsole.ConsoleToggleKey)) 
        {
            devConsole?.Toggle();
        }

        if (devConsole != null && devConsole.IsOpen) 
        {
            if (Input.GetKeyDown(devConsole.ConsoleInputKey)) 
            {
                devConsole.ProcessCommand();
            }
        }
#endif
    }

    public void StartNewRound() 
    {
        DestroyAllItems();

        currentRound++;
        Debug.Log("Starting round: " + currentRound);
        activeRound = Rounds[currentRound];
        roundDuration = activeRound.RoundDuration;
        collectedMedication.Clear();

        //Clear gameplay scene of any leftover pills / medication thats no longer relevant.
        var pills = FindObjectsOfType<Pill>().ToList();
        pills.ForEach(x => Destroy(x));

        //Reset UI
        ResetUI();
        var chartData = new ChartData 
        {
            Objectives = activeRound.Objectives
        };
        patientChart.SetData(chartData);

         //Configure difficulty data
         //Initialize spawner with new data
        conveyor.SetSpeed(activeRound.ConveyorSpeed);
        spawner.SetRandomSpawnProperties(activeRound.SpawnProperties);
        spawner.SetSpawningActive(true);
        StartCoroutine(RoundLogic());
    }

    private void DestroyAllItems()
    {
        //Clear all items
        var previousItems = GameObject.FindGameObjectsWithTag("Items");
        foreach (var item in previousItems)
        {
            Destroy(item);
        }
    }

    public IEnumerator RoundLogic() 
    {
        while (!IsRoundComplete()) 
        {
            yield return new WaitForSeconds(1f);
        }

        EndRound();
    }

    public void EndRound() 
    {
        spawner.SetSpawningActive(false);

        if (IsObjectiveAchieved())
        {
            Debug.Log("You're Winner!");
        }
        else 
        {
            Debug.Log("You Loser!");
        }
        Debug.Log("Starting next round...");

        activeRound = null;

        StartCoroutine(EndOfRoundLogic());
    }

    private IEnumerator EndOfRoundLogic() 
    {
        /* TODO
         * - Update UI with any end of round stats
         * - Show UI
         * - Run any end of round events (e.g dialog events, wait for user input etc.)
         * - Trigger next round (if we're not handling this event via other UI)
         */

        StartNewRound();
        yield break;
    }

    private bool IsRoundComplete() 
    {
        return roundElapsedTime >= roundDuration;
    }

    private bool IsObjectiveAchieved() 
    {
        bool isComplete = true;
        foreach (var objective in activeRound.Objectives) 
        {
            if (collectedMedication.ContainsKey(objective.Pill))
            {
                //N.B.  Setting greater than or equal to condition for success
                //      This may not be what we want for overdosing scenarios
                isComplete &= collectedMedication[objective.Pill] >= objective.Count;
            }
            else 
            {
                return false;
            }
        }
        return isComplete;
    }

    private void ResetUI()
    {
        //Reset Timer
        roundElapsedTime = 0f;
        countdownText.text = roundDuration.ToString("F0", CultureInfo.InvariantCulture);

        // Hide Patient Chart / Objectives
        if (patientChart.IsShowing) 
        {
            patientChart.Toggle();
        }
    }

    private void OnItemScored(PillSO item) 
    {
        //Update inventory
        if (collectedMedication.ContainsKey(item))
        {
            collectedMedication[item]++;
        }
        else 
        {
            collectedMedication.Add(item, 1);
        }

        //Update chart counter
        patientChart.IncrementPillCount(item);
    }

    private void OnItemDropped(PillSO item)
    {
        //Update inventory
        if (collectedMedication.ContainsKey(item))
        {
            collectedMedication[item]--;
        }

        //Update chart counter
        patientChart.DecrementPillCount(item);
    }
}
