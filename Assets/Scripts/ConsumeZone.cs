using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumeZone : MonoBehaviour
{
    /////// ON COLLISION ///////
    private void OnTriggerEnter(Collider col)
    {
        Debug.Log("HIT SHIT");
        /////// IF ITS AN ITEM ///////
        if (col.gameObject.tag == "Items")
        {
            //// PULL DEETS TO PLAYA ////
            ///
            /// 
            //// DELETE COLLIDED OBJECT ////
            col.gameObject.SetActive(false);
        }

    }
}
