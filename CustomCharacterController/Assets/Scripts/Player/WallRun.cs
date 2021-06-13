using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class WallRun : MonoBehaviour
{
    [Space(3, order = 0)]
    [Header("Wall Characteristics")]
    
    public float wallMaxDistance;
    public float minimumJumpHeight = 1.2f;
    public float jumpDuration = 1f;
    public float normalizedAngleThreshold = 0.1f;
    public float wallGravity= 20f;
    public float wallSpeed;

    [Tooltip("Should be negative")]
    public float WallRunUpAngle;
    public float WallRunDownAngle;
    float timePassedSinceJump = 0;
    float timePassedSinceAttached = 0;
    float timePassedSinceDetached = 0;


    [Header("Wall Run Directions")]    
    [Tooltip("Enable if you want to Run up the wall")]public bool canRunUp = false;
    [Tooltip("Enable if you want to Run Down the wall")]public bool canRunDown = false;
    [Tooltip("Enable if you want to Run along the wall")]public bool canRunAlong = false;
    [HideInInspector]public bool isWallRunning = false;
    [HideInInspector]public bool jumping;


    

    
    [Header("Camera Rotation Angle")]    
    public new Camera camera;
    private MouseLook mouseLook;
    public float maxAngleRoll;
    public float cameraTransitionDuration;

    private CustomCharacterController characterController;
    Vector3 lastWallPosition;
    Vector3 lastWallNormal;

    RaycastHit[] hitDir;
    Vector3[] directions;

    private void Start()
    {
        
        mouseLook = GetComponent<MouseLook>();
        characterController = GetComponent<CustomCharacterController>();

        directions = new Vector3[] {
            Vector3.right,
            Vector3.left,
            Vector3.right + Vector3.forward,
            Vector3.left + Vector3.forward,
            Vector3.forward
        };
        
    }

    
    public void CanRunUp(bool value)
    {
        canRunUp = value;         
    } 
    public void CanRunDown(bool value)
    {
        canRunDown = value;
    } 
    public void CanRunAlong(bool value)
    {
        canRunAlong = value;
    }
    float CalculateSide()
    {
        if (isWallRunning)
        {
            Vector3 heading = lastWallPosition - transform.position;
            Vector3 perp = Vector3.Cross(transform.forward, heading);
            float dir = Vector3.Dot(perp, transform.up);
            return dir;
        }
        return 0;
    }

    public float GetCameraRoll()
    {
        float dir = CalculateSide();
        float cameraAngle = camera.transform.eulerAngles.z;
        float targetAngle = 0f;
        if (dir != 0)
        {
            targetAngle = Mathf.Sign(dir) * maxAngleRoll;
        }
        return Mathf.LerpAngle(cameraAngle, targetAngle, Mathf.Max(timePassedSinceAttached, timePassedSinceDetached) / cameraTransitionDuration);
    }

    bool CanWallRun()
    {
        float verticalAxis = Input.GetAxisRaw("Vertical");

        return !characterController.charController.isGrounded && verticalAxis > 0 && VerticalCheck();
    }

    bool VerticalCheck()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    private void LateUpdate()
    {

        isWallRunning = false;
        if ((canRunAlong == true && (canRunDown == false && canRunUp == false)) 
            || (canRunUp == true &&(canRunDown == false && canRunAlong == false)) 
            || (canRunDown == true &&(canRunAlong == false && canRunUp == false)))
        {
            if (Input.GetButtonDown("Jump"))
            {
            jumping = true;
            }
        if (CanAttach())
        {
            hitDir = new RaycastHit[directions.Length];
            for (int i = 0; i < directions.Length; i++)
            {

                Vector3 direction = transform.TransformDirection(directions[i]);
                Physics.Raycast(transform.position, direction, out hitDir[i], wallMaxDistance);
                if (hitDir[i].collider != null)
                {
                    Debug.DrawRay(transform.position, direction * hitDir[i].distance, Color.red);
                }
                else
                {
                    Debug.DrawRay(transform.position, direction * hitDir[i].distance, Color.yellow);
                }
            }
            if (CanWallRun())
            {
                hitDir = hitDir.ToList().Where(h => h.collider != null).OrderBy(h => h.distance).ToArray();
                if (hitDir.Length > 0)
                {
                    OnWall(hitDir[0]);
                    lastWallPosition = hitDir[0].point;
                    lastWallNormal = hitDir[0].normal;
                }
            }
        }
        
            if (isWallRunning)
            {
                timePassedSinceDetached = 0;
                timePassedSinceAttached += Time.deltaTime;
                if (canRunAlong == true)
                {
                    characterController.charVelocity += Time.deltaTime * wallGravity * Vector3.forward;
                }
                else if (mouseLook.lookDirection.y >= WallRunUpAngle && canRunUp == true)
                {
                    characterController.charVelocity += Time.deltaTime * wallGravity * Vector3.up;

                }
                else if (mouseLook.lookDirection.y <= WallRunDownAngle && canRunDown == true)
                {
                    characterController.charVelocity += Time.deltaTime * wallGravity * Vector3.down;

                }
                

                Debug.Log("wallRunning");
            }
            else
            {
                timePassedSinceAttached = 0;
                timePassedSinceDetached += Time.deltaTime;
            }
        }
        else 
        {
            Debug.LogError("No Direction specified in 'Wall Run Direction' or Chose more than one direction");
        }
    }

    bool CanAttach()
    {
        if(jumping)
        {
            timePassedSinceJump += Time.deltaTime;
            if(timePassedSinceJump > jumpDuration)
            {
                timePassedSinceJump = 0;
                jumping = false;
            }
            return false;
        }
        return true;
    }

    void OnWall(RaycastHit hit)
    {
        float d = Vector3.Dot(hit.normal, Vector3.up);
        if(d >= -normalizedAngleThreshold && d<= normalizedAngleThreshold)
        {
            float vertical = Input.GetAxisRaw("Vertical");
            Vector3 alongWall = transform.TransformDirection(Vector3.forward);
            Vector3 upWall = transform.TransformDirection(Vector3.up + Vector3.forward);
            Vector3 downWall = transform.TransformDirection(Vector3.down + Vector3.forward);
            Debug.DrawRay(transform.position, alongWall.normalized * 10, Color.green);
            Debug.DrawRay(transform.position, lastWallNormal * 10, Color.magenta);



            if (canRunAlong == true )
            {
                characterController.charVelocity = vertical * wallSpeed * alongWall;
            }
            else if (mouseLook.lookDirection.y >= WallRunUpAngle && canRunUp == true)
            { 
                characterController.charVelocity = vertical * wallSpeed * upWall; 
            }    
            else if (mouseLook.lookDirection.y <= WallRunDownAngle && canRunDown == true)
            { 
                characterController.charVelocity = vertical * wallSpeed * downWall; 
            } 
         
            isWallRunning = true;
        }
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle
        {
            fontSize = 24
        };
        
          
            GUI.Label(new Rect(10, 0, 0, 0), "Wall Running Up: " + canRunUp, style);
            GUI.Label(new Rect(10, 25, 0, 0), "Wall Running Down: " + canRunDown, style);
            GUI.Label(new Rect(10, 50, 0, 0), "Wall Running Along: " + canRunAlong, style);
        
      
    }


}

