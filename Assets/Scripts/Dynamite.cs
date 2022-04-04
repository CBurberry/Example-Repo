using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dynamite : MonoBehaviour
{
    public GameObject exp;

    public float force;
    public float radius;

    public TextMeshPro textMesh;

    public float countdown;
    float timer = 3;

    //https://www.codegrepper.com/code-examples/csharp/unity+add+explosion+force


    private void Start()
    {
        timer = countdown;

    }

    private void Update()
    {
        timer = timer - Time.deltaTime;
        //Debug.Log(timer);
        if (timer <= 0)
        {
            Debug.Log("why");
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
