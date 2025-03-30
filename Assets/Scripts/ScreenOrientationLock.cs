using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenOrientationLock : MonoBehaviour
{
    private ScreenOrientation previousOrientation;

    void Start()
    {
        // Store current orientation before locking
        previousOrientation = Screen.orientation;

        // Lock orientation to Portrait
        Screen.orientation = ScreenOrientation.Portrait;

        //Optional: Force update
        Screen.autorotateToLandscapeLeft = false;
        Screen.autorotateToLandscapeRight = false;
        Screen.autorotateToPortrait = true;
        Screen.autorotateToPortraitUpsideDown = true;
    }

    void OnDestroy()
    {
        // Restore the previous screen orientation when leaving this scene
        Screen.orientation = previousOrientation;
    }
}
