using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeZone : MonoBehaviour
{
    //get the list from the player

    /////// ON COLLISION ///////
    private void OnTriggerEnter(Collider col)
    {
        //Debug.Log("HIT SHIT");
        /////// IF ITS AN ITEM ///////
        if (col.gameObject.tag == "Items")
        {
            //// PULL DEETS TO PLAYA ////
            //Pill pullme = col.GetComponent<Pill>();
            //add it to the list
            //
            /// 
            //// DELETE COLLIDED OBJECT ////
            Destroy(col.gameObject);
        }

    }
}
