using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Utility.DeveloperConsole;

public class GameController : MonoBehaviour
{
    private enum RoundState 
    {
        PlayerWon = 0,
        Overdosed,
        MassivelyOverdosed,
        Underdosed,
        ExtraMeds,
        ExplosiveSwallowed
    };

    [BoxGroup("User Interface")]
    [SerializeField] PatientChart patientChart;
    [BoxGroup("User Interface")]
    [SerializeField] GameObject patientChartButton;
    [BoxGroup("User Interface")]
    [SerializeField] GridLayoutGroup medicinesList;
    [BoxGroup("User Interface")]
    [SerializeField] Text countdownText;
    [BoxGroup("User Interface")]
    [SerializeField] Toggle nextRoundToggle;
    [BoxGroup("User Interface")]
    [SerializeField] Toggle retryToggle;
    [BoxGroup("User Interface")]
    [SerializeField] Toggle quitToggle;

    //End of round dialog
    [BoxGroup("User Interface")]
    [SerializeField] List<RoundStateDialog> diagnoses;

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

    [BoxGroup("Misc")]
    [SerializeField] ToggleTranslation tray;

    private int currentRound;
    private RoundData activeRound;
    private float roundElapsedTime;
    private RoundState activeRoundEndState;

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
        StartNextRound();
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

    public void StartNextRound() 
    {
        DestroyAllItems();
        currentRound++;

        if (currentRound > Rounds.Count - 1)
        {
            Debug.LogWarning("End of defined rounds!");
            QuitGame();
        }

        StartRound();
    }

    public void RestartRound() 
    {
        DestroyAllItems();
        StartRound();
    }

    public void QuitGame() 
    {
        StartCoroutine(QuitGameAsync());
    }

    private IEnumerator QuitGameAsync(float delay = 0.5f) 
    {
        if (delay > 0f) 
        {
            yield return new WaitForSeconds(delay);
        }

#if !UNITY_EDITOR
            Application.Quit();
#else
        EditorApplication.isPlaying = false;
#endif
    }

    private void StartRound()
    {
        Debug.Log("Starting round: " + currentRound);
        tray.Toggle();
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
        patientChart.SetObjectiveData(chartData);

        //Configure difficulty data
        //Initialize spawner with new data
        conveyor.SetSpeed(activeRound.ConveyorSpeed);
        spawner.SetRandomSpawnProperties(activeRound.SpawnProperties);
        spawner.SetSpawnRate(activeRound.SpawnRate);
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
            if (IsObjectiveAchieved())
            {
                break;
            }

            yield return new WaitForSeconds(1f);
        }

        //Disabling as collision triggers are causing issues.
        //yield return tray.Hide();
        EndRound();
    }

    public void EndRound() 
    {
        spawner.SetSpawningActive(false);
        activeRoundEndState = GetRoundEndResult();
        Debug.Log($"Round end result: {activeRoundEndState}");
        activeRound = null;
        StartCoroutine(EndOfRoundLogic());
    }

    private IEnumerator EndOfRoundLogic() 
    {
        //Disable clipboard button 
        patientChartButton.SetActive(false);

        //Hide clipboard if it's up
        var chartTranslationComponent = patientChart.GetComponent<ToggleTranslation>();
        if (chartTranslationComponent.IsShowing)
        {
            chartTranslationComponent.Hide();
        }

        //Show doctors diagnosis field
        patientChart.SetDiagnosisTitleActive(true);

        //Show clipboard
        patientChart.gameObject.SetActive(true);
        yield return chartTranslationComponent.Show();

        //Start typing diagnosis
        var result = diagnoses.First(x => x.State == activeRoundEndState);
        Debug.Log($"Result Dialog: '{result.Diagnosis}'");
        yield return patientChart.TypeDiagnosis(result.Diagnosis);

        //Show toggles
        if (activeRoundEndState != RoundState.PlayerWon)
        {
            retryToggle.isOn = false;
            retryToggle.gameObject.SetActive(true);
        }
        else 
        {
            nextRoundToggle.isOn = false;
            nextRoundToggle.gameObject.SetActive(true);
        }

        quitToggle.isOn = false;
        quitToggle.gameObject.SetActive(true);
        yield break;
    }

    private bool IsRoundComplete() 
    {
        return roundElapsedTime >= roundDuration;
    }

    private RoundState GetRoundEndResult() 
    {
        //TODO - explosive implementation needs a way to tell whether there is actually an explosive on the tray (currently blocked).
        var state = RoundState.PlayerWon;
        if (IsMassivelyOverdosed())
        {
            state = RoundState.MassivelyOverdosed;
        }
        else if (IsOverdosed()) 
        {
            state = RoundState.Overdosed;
        }
        else if (IsExtraMeds())
        {
            state = RoundState.ExtraMeds;
        }
        else if (!IsObjectiveAchieved())
        {
            state = RoundState.Underdosed;
        }

        return state;
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

        //Reset Toggles
        nextRoundToggle.isOn = false;
        nextRoundToggle.gameObject.SetActive(false);
        retryToggle.isOn = false;
        retryToggle.gameObject.SetActive(false);

        // Hide Patient Chart / Objectives
        var chartTranslationComponent = patientChart.GetComponent<ToggleTranslation>();
        if (chartTranslationComponent.IsShowing) 
        {
            chartTranslationComponent.Toggle();
        }

        patientChartButton.SetActive(true);
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

    private bool IsMassivelyOverdosed()
    {
        var objectives = activeRound.Objectives;
        var inventory = collectedMedication; //this may need changing to check on explosives or other items

        bool isTrue = true;
        foreach (var objective in objectives)
        {
            if (inventory.ContainsKey(objective.Pill))
            {
                isTrue &= inventory[objective.Pill] >= objective.Count + 3;
            }
            else
            {
                return false;
            }
        }

        return isTrue;
    }

    private bool IsOverdosed()
    {
        var objectives = activeRound.Objectives;
        var inventory = collectedMedication; //this may need changing to check on explosives or other items

        bool isTrue = true;
        foreach (var objective in objectives)
        {
            if (inventory.ContainsKey(objective.Pill))
            {
                isTrue &= inventory[objective.Pill] > objective.Count;
            }
            else
            {
                return false;
            }
        }

        return isTrue;
    }

    private bool IsExtraMeds()
    {
        var objectives = activeRound.Objectives;
        var inventory = collectedMedication; //this may need changing to check on explosives or other items
        var objectivesPillTypes = objectives.Select(x => x.Pill).ToList();

        foreach (var kvp in inventory) 
        {
            var pill = kvp.Key;
            int count = kvp.Value;
            if (count > 0) 
            {
                if (!objectivesPillTypes.Contains(pill)) 
                {
                    return true;
                }
            }
        }

        return false;
    }

    [System.Serializable]
    private class RoundStateDialog 
    {
        public RoundState State;
        public string Diagnosis;
    }
}
