using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartPageManager : MonoBehaviour
{
    public GameObject[] pages;
    public Button nextButton;
    public Button prevButton;
    public Button startButton;
    public CanvasGroup fadePanel;
    public float fadeDuration = 1.5f;

    private int currentPage = 0;

    void Start()
    {
        fadePanel.alpha = 1; // Start with fade-in effect
        StartCoroutine(FadeIn());
        UpdatePage();
    }

    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            UpdatePage();
        }
    }

    public void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }

    public void StartApp()
    {
        StartCoroutine(FadeOutAndLoadScene("MainPage"));
    }

    void UpdatePage()
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == currentPage);
        }

        prevButton.gameObject.SetActive(currentPage > 0);
        nextButton.gameObject.SetActive(currentPage < pages.Length - 1);
        startButton.gameObject.SetActive(currentPage == pages.Length - 1);
    }

    IEnumerator FadeIn()
    {
        while (fadePanel.alpha > 0)
        {
            fadePanel.alpha -= Time.deltaTime / fadeDuration;
            yield return null;
        }
    }

    IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        while (fadePanel.alpha < 1)
        {
            fadePanel.alpha += Time.deltaTime / fadeDuration;
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }
}
