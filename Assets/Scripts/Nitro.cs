using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nitro : MonoBehaviour
{
    public GameObject exp;

    public float force;
    public float radius;


    //https://www.codegrepper.com/code-examples/csharp/unity+add+explosion+force


    void OnCollisionEnter(Collision col)
    {
        Debug.Log("hit");
        Debug.Log(col.gameObject.tag);
        Debug.Log(col.gameObject);
        if (col.gameObject.tag == "Items")
        {
            Debug.Log("EXPLODE");
            Triggered();
        }
    }


    void Triggered()
    {
        GameObject _explosion = Instantiate(exp, transform.position, transform.rotation);
        Destroy(_explosion, 3);
        KnockBack();
        Destroy(gameObject);
    }


    void KnockBack()
    {
        //grab all the colliders within radius from this transform.position
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider collider in colliders)
        {
            Rigidbody rigg = collider.GetComponent<Rigidbody>();
            if (rigg != null)
            {
                rigg.AddExplosionForce(force, transform.position, radius);
            }
        }
    }
}
