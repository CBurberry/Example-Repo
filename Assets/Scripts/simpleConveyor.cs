using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleConveyor : MonoBehaviour
{
    [SerializeField] float speed;
    Rigidbody rBody;

    // Start is called before the first frame update
    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 Pos  = rBody.position;
        rBody.position += Vector3.back * speed * Time.fixedDeltaTime;
        rBody.MovePosition(Pos);

    }
}
