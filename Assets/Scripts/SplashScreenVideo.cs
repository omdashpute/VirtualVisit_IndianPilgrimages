using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class SplashScreenVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd; // Calls function when video finishes
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SceneManager.LoadScene("StartPage"); // Replace with your actual scene name
    }
}
