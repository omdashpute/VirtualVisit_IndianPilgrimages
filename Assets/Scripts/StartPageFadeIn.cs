using UnityEngine;
using System.Collections;

public class StartPageFadeIn : MonoBehaviour
{
    public CanvasGroup fadePanel; // Drag the same black panel here
    public float fadeDuration = 1.5f; // Adjust fade speed

    void Start()
    {
        fadePanel.alpha = 1; // Start fully black
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        while (fadePanel.alpha > 0)
        {
            fadePanel.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }
    }
}
