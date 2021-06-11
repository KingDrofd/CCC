using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCharacterController : MonoBehaviour
{


    #region

    [Header("CharacterMovement")]
    public float defaultGravity = 9.81f;
    public float Gravity;
    [HideInInspector] public Vector3 charVelocity;
    public float Speed = 5f;
    [HideInInspector] public CharacterController charController;
    public Camera playerCamera;

    [Header("Jump")]
    public LayerMask Ground;
    private int maxJumpTimes = 2;
    private int curJumps;
    public float JumpHeight;
    public float jumpForce = 2f;
    public float GroundDistance = 0.2f;
    private bool isGrounded = true;


    [Header("Looking")]
    public float rotationSpeed;
    public float rotationMultiplier;
    [HideInInspector] public float verticalCamAng = 0f;

    [Header("Dash")]
   // public float DashSpeed = 5f;
    public float DashDistance;
    //public float DashTime;
    public Vector3 Drag;

    [Header("WallRun")]
    public WallRun wallRun;


    private Transform charGroundCheck;


    #endregion

    void MovementHandler()
    {

        transform.Rotate(new Vector3(0f, (Input.GetAxis("Mouse X") * rotationSpeed * rotationMultiplier)));
        verticalCamAng += -Input.GetAxis("Mouse Y");        

        if(wallRun != null)
        {
            playerCamera.transform.localEulerAngles = new Vector3(verticalCamAng, 0, wallRun.GetCameraRoll());
            //Debug.Log(verticalCamAng);
        }
        else
        {
            playerCamera.transform.localEulerAngles = new Vector3(-verticalCamAng, 0, 0);
            //Debug.Log(verticalCamAng);
        }

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        
        

        Vector3 move = transform.right * x + transform.forward * z;

        charController.Move(Speed * Time.deltaTime * move.normalized);

        charVelocity.y += -Gravity * Time.deltaTime;

        charVelocity.x /= 1 + Drag.x * Time.deltaTime;
        charVelocity.y /= 1 + Drag.y * Time.deltaTime;
        charVelocity.z /= 1 + Drag.z * Time.deltaTime;

        charController.Move(charVelocity * Time.deltaTime);

    }
    void IsGrounded()
    {
        isGrounded = Physics.CheckSphere(charGroundCheck.position, GroundDistance, Ground, QueryTriggerInteraction.Ignore);
        if (isGrounded && charVelocity.y < 0)
            charVelocity.y = 0f;
    }
    void Dash()
    {

        charVelocity += Vector3.Scale(transform.forward, DashDistance * new Vector3(
           Mathf.Log(1f / ((Time.deltaTime * Drag.x) + 1)) / -Time.deltaTime,
           0,
            Mathf.Log(1f / (Time.deltaTime * Drag.z + 1)) / -Time.deltaTime));



        //charController.Move(DashDistance * Time.deltaTime * transform.forward);
            Debug.Log("Dash");
        

       
    }



    public void Jump()
    {
        if (Input.GetButtonDown("Jump") && charController.isGrounded == true && curJumps <= maxJumpTimes)
        {            
            charVelocity.y = jumpForce;
            Debug.Log("Jumped");
        }
        Vector3 jumpVector = new Vector3(0, charVelocity.y, 0);
        charController.Move(jumpVector * Time.deltaTime);
    }
    void Start()
    {
        charController = GetComponent<CharacterController>();
        charGroundCheck = transform.GetChild(0);
        curJumps = maxJumpTimes;
        defaultGravity = Gravity;
    }

    void Update()
    {
        IsGrounded();
        MovementHandler();
        if(wallRun.isWallRunning)
        {
            defaultGravity = 0f;
        }
       else
        {
            defaultGravity = Gravity;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Dash();
        }
        
        

        
        if (charController.isGrounded)
        {
            defaultGravity = Gravity;
            charVelocity.y = -defaultGravity * Time.deltaTime;
            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }
        }
        else
        {
            charVelocity.y -= defaultGravity * Time.deltaTime;
        }

     
    }
}

    


