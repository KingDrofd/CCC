using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FpsCounter : MonoBehaviour
{
    public TextMeshProUGUI fpsDisplay;
   

    
    void Update()
    {
        float fps = 1 / Time.unscaledDeltaTime;
        fpsDisplay.text = "FPS: " + fps;

      
    }
}
