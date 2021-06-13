using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public Transform player;
    public Camera playerCamera;

    [HideInInspector]public Vector2 lookDirection;
    

    
    public float lookSensitivity;
    public float smooting;

    void MouseLookHandler()
    {       
        Vector2 mouseDirection = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mouseDirection = Vector2.Scale(mouseDirection, new Vector2(lookSensitivity, lookSensitivity));

        Vector2 deltaLook = new Vector2();        
        deltaLook.y = Mathf.Lerp(deltaLook.y, mouseDirection.y, 1.0f / smooting);

        lookDirection += deltaLook;     

        //lookDirection.y = Mathf.Clamp(lookDirection.y, -75f, 75f);    
        //Debug.Log(lookDirection.y);
    }

    
    private void Update()
    {
        
        MouseLookHandler();
        
        
    }

    //void HandleMouseLook()
    //{
    //    float mouseX = Input.GetAxisRaw("Horizontal");
    //    float mouseY = Input.GetAxisRaw("Vertical");
    //    Quaternion cameraRotation = Quaternion.Euler(mouseY, 0f, 0f);
    //    Quaternion playerRotation = Quaternion.Euler(0f, mouseX, 0f);

    //    transform.localRotation = cameraRotation;
    //    transform.parent.localRotation = playerRotation;
    //}
}
