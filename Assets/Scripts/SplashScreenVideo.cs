using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreenVideo : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public CanvasGroup fadePanel; // Drag your black panel here
    public float fadeDuration = 1.5f; // Adjust fade speed
    public float sceneChangeTime = 5.0f; // Time before switching scenes

    void Start()
    {
        videoPlayer.Play();
        StartCoroutine(FadeIn());
        StartCoroutine(ChangeSceneEarly());
    }

    IEnumerator FadeIn()
    {
        fadePanel.alpha = 1;
        while (fadePanel.alpha > 0)
        {
            fadePanel.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }
    }

    IEnumerator ChangeSceneEarly()
    {
        yield return new WaitForSeconds(sceneChangeTime); // Wait before fading out
        yield return StartCoroutine(FadeOut()); // Fade-out before scene transition
        SceneManager.LoadScene("StartPage");
    }

    IEnumerator FadeOut()
    {
        while (fadePanel.alpha < 1)
        {
            fadePanel.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
}
