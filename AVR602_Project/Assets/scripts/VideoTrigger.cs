using UnityEngine;
using UnityEngine.Video;

public class VideoTrigger : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            videoPlayer.Play();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            videoPlayer.Stop();
    }
}