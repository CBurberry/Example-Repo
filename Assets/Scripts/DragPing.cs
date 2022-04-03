using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragPing : MonoBehaviour
{
    LineRenderer line;

    Vector3 pointA;
    Vector3 pointB;

    private float mZCoord;
    private Vector3 mOffset;

    public float upHeight = 0.1f;
    public float forceMultiplier = 50;


    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    private void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        // get the world position offset
        pointA = gameObject.transform.position - GetMouseWorldPos();
        line.enabled = true;
    }

    private Vector3 GetMouseWorldPos()
    {
        // pixel coordinates (x,y)
        Vector3 mousepoint = Input.mousePosition;

        // z coordinates of game object
        mousepoint.z = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        return Camera.main.ScreenToWorldPoint(mousepoint);
    }

    private void OnMouseUp()
    {
        pointB = GetMouseWorldPos();

        Vector3 difference = pointB - this.transform.position;
        Vector3 direction = new Vector3(difference.x, -upHeight, difference.z);
        Debug.Log(direction);
        this.GetComponent<Rigidbody>().AddForce(-direction * forceMultiplier);
        line.enabled = false;
    }

        private void FixedUpdate()
    {
        if(pointA != Vector3.zero && pointB == Vector3.zero && line.enabled == true)
        {
         
        line.SetPosition(0, this.transform.position);
        line.SetPosition(1, GetMouseWorldPos());
        //line.SetPosition(1, pointB);
        }
        //if point A exists && Point B doesnt exist
        //draw line between A + mouse
    }
}
