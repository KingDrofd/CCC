using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI speed;
    public TextMeshProUGUI gravity;
    public TextMeshProUGUI jumpForce;
    public TextMeshProUGUI dashForce;
    public TextMeshProUGUI wallSpeed;
    public CustomCharacterController charController;
    public WallRun wallRun;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speed.text = "Speed: " + charController.Speed.ToString();
        gravity.text = "Gravity: " + charController.Gravity.ToString();
        jumpForce.text = "JumpForce: " + charController.jumpForce.ToString();
        dashForce.text = "dashForce: " + charController.DashDistance.ToString();
        wallSpeed.text = "Wall Seed: " + wallRun.wallSpeed.ToString();
        
    }

    public void AdjustSpeed(float newSpeed)
    {        
        charController.Speed = newSpeed;
    }

    public void AdjustGravity (float newGravity)
    {
        charController.Gravity = newGravity;
    }  
    public void AdjustJF (float newForce)
    {
        charController.jumpForce = newForce;
    }
    public void AdjustDF(float newForce)
    {
        charController.DashDistance = newForce;
    } 
    public void AdjustWallRS(float newSpeed)
    {
        wallRun.wallSpeed = newSpeed;
    }
    public void ResetScene()
    {
        SceneManager.LoadScene(0);
    }
    
}
