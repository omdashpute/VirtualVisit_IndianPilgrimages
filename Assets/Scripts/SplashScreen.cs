using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SplashScreen : MonoBehaviour
{
    public Image logo;
    public Slider loadingBar;
    private float loadTime = 3f; // Adjust loading duration

    void Start()
    {
        StartCoroutine(ShowSplashScreen());
    }

    IEnumerator ShowSplashScreen()
    {
        // 1️ Fade-in Logo
        logo.color = new Color(1, 1, 1, 0); // Set transparent
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            logo.color = new Color(1, 1, 1, t); // Gradually make it visible
            yield return null;
        }

        // 2️ Fill Loading Bar
        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime / loadTime;
            loadingBar.value = progress;
            yield return null;
        }

        // 3️ Load StartPage Scene
        SceneManager.LoadScene("StartPage");
    }
}
