using UnityEngine;
using System.Collections;

public class StartPageFadeIn : MonoBehaviour
{
    public CanvasGroup fadePanel;
    public float fadeDuration = 1.5f;

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
