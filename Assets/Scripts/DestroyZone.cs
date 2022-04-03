using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        //Debug.Log("HIT SHIT");
        /////// IF ITS AN ITEM ///////
        if (col.gameObject.tag == "Items")
        {

            Destroy(col.gameObject);
        }

    }
}
