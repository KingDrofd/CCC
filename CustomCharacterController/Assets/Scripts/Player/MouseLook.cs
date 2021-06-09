using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public Transform player;
    public Camera playerCamera;

    [HideInInspector]public Vector2 lookDirection;
    Vector3 rotation;

    private float turnSpeed = 2f;
    public float lookSensitivity;
    public float smooting;


    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void MouseLookHandler()
    {
       
        Vector2 mouseDirection = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        mouseDirection = Vector2.Scale(mouseDirection, new Vector2(lookSensitivity, lookSensitivity));

        Vector2 deltaLook = new Vector2();

        deltaLook.x = Mathf.Lerp(deltaLook.x, mouseDirection.x, 1.0f / smooting);
        deltaLook.y = Mathf.Lerp(deltaLook.y, mouseDirection.y, 1.0f / smooting);

        lookDirection += deltaLook;

        rotation = new Vector3(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);

        lookDirection.y = Mathf.Clamp(lookDirection.y, -75f, 75f);

        playerCamera.transform.localRotation = Quaternion.AngleAxis(-lookDirection.y, Vector3.right);
        player.transform.localRotation = Quaternion.AngleAxis(lookDirection.x, player.transform.up);
        player.transform.Rotate(rotation);
        Debug.Log(lookDirection.y);
    }

    
    private void Update()
    {
        
        MouseLookHandler();
        
        
    }

    //void HandleMouseLook()
    //{
    //    Quaternion cameraRotation = Quaternion.Euler(mouseY, 0f, 0f);
    //    Quaternion playerRotation = Quaternion.Euler(0f, mouseX, 0f);

    //    transform.localRotation = cameraRotation;
    //    transform.parent.localRotation = playerRotation;
    //}
}
