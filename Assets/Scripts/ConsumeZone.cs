using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeZone : MonoBehaviour
{
    //External event
    public event Action<PillSO> OnConsumed;

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
            OnConsumed?.Invoke(pill.GetData());
            //add it to the list
            //
            /// 
            //// DELETE COLLIDED OBJECT ////
            Destroy(col.gameObject);
        }

    }
}
