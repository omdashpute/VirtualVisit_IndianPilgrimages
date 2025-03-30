using UnityEngine;
using System.Collections;

public class MainPageFadeIn : MonoBehaviour
{
    public CanvasGroup fadePanel; // Drag your fade panel here
    public float fadeDuration = 1.5f; // Adjust fade speed

    void Start()
    {
        if (fadePanel == null)
        {
            return;
        }

        fadePanel.alpha = 1; // Ensure it starts fully black
        fadePanel.blocksRaycasts = true; // Prevent interaction until fade completes

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(0.1f); // Small delay to ensure scene is fully loaded

        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            fadePanel.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadePanel.alpha = 0; // Ensure it's fully transparent
        fadePanel.blocksRaycasts = false; // Allow UI interaction

    }
}
