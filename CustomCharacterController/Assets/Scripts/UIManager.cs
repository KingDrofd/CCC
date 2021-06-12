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
    public TextMeshProUGUI nat;
    public CustomCharacterController charController;
    public WallRun wallRun;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse2) && Cursor.visible == true)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Input.GetKeyDown(KeyCode.Mouse2) )
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
      

        speed.text = "Speed: " + charController.curSpeed.ToString();
        gravity.text = "Gravity: " + charController.Gravity.ToString();
        jumpForce.text = "JumpForce: " + charController.jumpForce.ToString();
        dashForce.text = "dashForce: " + charController.DashDistance.ToString();
        wallSpeed.text = "Wall Speed: " + wallRun.wallSpeed.ToString();
        nat.text = "Normalized Angle Threshold: " + wallRun.normalizedAngleThreshold.ToString();
        
    }

    public void AdjustSpeed(float newSpeed)
    {        
        charController.curSpeed = newSpeed;
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
    public void AdjustNAT(float newValue)
    {
        wallRun.normalizedAngleThreshold = newValue;
    }
    public void ResetScene()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
    }

}
