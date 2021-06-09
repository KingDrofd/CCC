using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float turnSpeed = 2f;
    private float angle = 1f;
    private Quaternion rotationTarget;

    void Start()
    {
        
    }

    
    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        rotationTarget = Quaternion.Euler(0, angle, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationTarget, turnSpeed * Time.deltaTime);
    }
}
