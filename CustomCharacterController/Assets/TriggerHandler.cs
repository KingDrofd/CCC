using UnityEngine.Video;
using UnityEngine;

public class TriggerHandler : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public GameObject quad;
    void Start()
    {
        quad.SetActive(false);
    }

    // Update is called once per frame
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            videoPlayer.Play();
            quad.SetActive(true);
            
            
            
        }
    }
    void Update()
    {
        
    }
}
