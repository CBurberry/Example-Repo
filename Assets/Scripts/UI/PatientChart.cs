using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using DG.Tweening;

public class ChartData
{
    public List<Objective> Objectives;
}

public class PatientChart : MonoBehaviour
{
    [BoxGroup("User Interface")]
    [SerializeField] GridLayoutGroup objectivesParent;
    [BoxGroup("User Interface")]
    [SerializeField] MedicineCounterUI medCounterPrefab;

    private List<MedicineCounterUI> objectivesUi;
    private List<PillSO> pills;

    public void SetData(ChartData data)
    {
        //Dumb approach of just reinitializing the whole list.
        //Definitely not efficient but gets the job done
        objectivesUi = new List<MedicineCounterUI>();
        pills = new List<PillSO>();

        Transform parentTransform = objectivesParent.gameObject.transform;
        int children = parentTransform.childCount;
        for (int i = children - 1; i >= 0; i--)
        {
            parentTransform.GetChild(i).gameObject.SetActive(false);
            Destroy(parentTransform.GetChild(i).gameObject);
        }

        foreach (var objective in data.Objectives) 
        {
            var member = Instantiate(medCounterPrefab, parentTransform);
            member.Initialize(objective.Count, objective.Pill.PreviewIcon);
            objectivesUi.Add(member);
            pills.Add(objective.Pill);
        }
    }

    public void IncrementPillCount(PillSO pill) 
    {
        if (!pills.Contains(pill)) 
        {
            return;
        }

        int index = pills.FindIndex(x => x.colr == pill.colr && x.shape == pill.shape);
        objectivesUi[index].IncrementCount();
    }

    public void DecrementPillCount(PillSO pill) 
    {
        if (!pills.Contains(pill))
        {
            return;
        }

        int index = pills.FindIndex(x => x.colr == pill.colr && x.shape == pill.shape);
        objectivesUi[index].DecrementCount();
    }

    //Helper function to prevent the popup of the clipboard
    // while an item is being dragged in the same area
    private bool CanShow() 
    {
        return !DragPing.IsDragged;
    }
}
