using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEuler : MonoBehaviour
{

    RaycastHit hitRight;
    RaycastHit hitLeft;
    Vector3 currentEuler;
    public float rotationSpeed = 0f;
    float x, y, z;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) x = 1 - x;
        if (Input.GetKeyDown(KeyCode.Y)) y = 1 - y;
        if (Input.GetKeyDown(KeyCode.Z)) z = 10;

        
        int layerMask = 1 << 8;
        layerMask = ~layerMask;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hitRight, 2, layerMask) )
        {
            
            
            currentEuler = new Vector3(0, 0, 10) * rotationSpeed;
            transform.localEulerAngles = currentEuler;

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hitRight.distance, Color.red);
            Debug.Log("Hitting right");


        }
        else if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hitLeft, 2, layerMask) )
        {
            currentEuler = new Vector3(0, 0, -z);
            transform.localEulerAngles = currentEuler;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * hitLeft.distance, Color.yellow);
            Debug.Log("Hitting left");
        }
       
    }
    void OnGUI()
    {
        GUIStyle style = new GUIStyle
        {
            fontSize = 24
        };
        GUI.Label(new Rect(10, 0, 0, 0), "Rotating on X:" + x + " Y:" + y + " Z:" + z, style);

        GUI.Label(new Rect(10, 50, 0, 0), "Transform.localEulerAngle: " + transform.localEulerAngles, style);
    }
}
