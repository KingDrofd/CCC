using UnityEngine;
using System.Linq;
using UnityEngine.Rendering;

public class WallRun : MonoBehaviour
{

    public float wallMaxDistance;
    public float minimumJumpHeight = 1.2f;
    public float jumpDuration = 1f;
    public float normalizedAngleThreshold = 0.1f;
    public float wallGravity= 20f;
    public float wallSpeed;
    public float WallRunUpAngle;
    [Tooltip("Should be negative")]
    public float WallRunDownAngle;
    float timePassedSinceJump = 0;
    float timePassedSinceAttached = 0;
    float timePassedSinceDetached = 0;
    

    public bool isWallRunning = false;
    public bool jumping;
    [SerializeField] private CustomCharacterController characterController;

    

    Vector3 lastWallPosition;
    Vector3 lastWallNormal;

    RaycastHit[] hitDir;
    Vector3[] directions;




    [Header("Looking")]
    public MouseLook mouseLook;
    public new Camera camera;
    public float maxAngleRoll;
    public float cameraTransitionDuration;



    private void Start()
    {
        directions = new Vector3[] {
            Vector3.right,
            Vector3.left,
            Vector3.right + Vector3.forward,
            Vector3.left + Vector3.forward,
            Vector3.forward
        };
        
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
       if(Input.GetButtonDown("Jump"))
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
            if(CanWallRun())
            {
                hitDir = hitDir.ToList().Where(h => h.collider != null).OrderBy(h => h.distance).ToArray();
                if(hitDir.Length > 0)
                {
                    OnWall(hitDir[0]);
                    lastWallPosition = hitDir[0].point;
                    lastWallNormal = hitDir[0].normal;
                }
            }
        }
        if(isWallRunning)
        {
            timePassedSinceDetached = 0;
            timePassedSinceAttached += Time.deltaTime;
            if (mouseLook.lookDirection.y >= WallRunUpAngle)
            {
                characterController.charVelocity += Time.deltaTime * wallGravity * Vector3.up;
                

            }
            else if (mouseLook.lookDirection.y <= WallRunDownAngle)
            {
                characterController.charVelocity += Time.deltaTime * wallGravity * Vector3.down;
               
            }

              
            Debug.Log("wallRunning");
        }else
        {
            timePassedSinceAttached = 0;
            timePassedSinceDetached += Time.deltaTime;
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
           
            characterController.charVelocity = vertical * wallSpeed * alongWall;

            if (mouseLook.lookDirection.y >= WallRunUpAngle)
            { characterController.charVelocity = vertical * wallSpeed * upWall; }    

            else if (mouseLook.lookDirection.y <= WallRunDownAngle)
            { characterController.charVelocity = vertical * wallSpeed * downWall; }
                
            isWallRunning = true;
        }
    }
    
}
