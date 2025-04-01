using UnityEngine;
using System.Collections;

public class MainPageFadeIn : MonoBehaviour
{
    public CanvasGroup fadePanel;
    public float fadeDuration = 1.5f;

    void Start()
    {
        if (!fadePanel) return;

        fadePanel.alpha = 1;
        fadePanel.blocksRaycasts = true;

        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(0.1f);

        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            fadePanel.alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        fadePanel.alpha = 0;
        fadePanel.blocksRaycasts = false;
        enabled = false; // Disable script when fade is complete
    }
}
