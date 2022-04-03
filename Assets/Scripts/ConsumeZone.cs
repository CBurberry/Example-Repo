using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeZone : MonoBehaviour
{
    //External events
    public event Action<PillSO> OnAdded;
    public event Action<PillSO> OnDropped;

    public List<PillSO> contained = new List<PillSO>();


    //get the list from the player

    /////// ON COLLISION ///////
    private void OnTriggerEnter(Collider col)
    {
        //Debug.Log("HIT SHIT");
        /////// IF ITS AN ITEM ///////
        if (col.gameObject.tag == "Items")
        {
            
            //// PULL DEETS TO PLAYA ////
            Pill pill = col.gameObject.GetComponent<Pill>();
            contained.Add(pill.pso);
            OnAdded?.Invoke(pill.GetData());

        }

    }

    private void OnTriggerExit(Collider col)
    {
        //Debug.Log("HIT SHIT");
        /////// IF ITS AN ITEM ///////
        if (col.gameObject.tag == "Items")
        {
            //// PULL DEETS TO PLAYA ////
            Pill pill = col.gameObject.GetComponent<Pill>();
            contained.Remove(pill.pso);
            OnDropped?.Invoke(pill.GetData());
        }

    }

}
