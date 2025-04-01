using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenOrientationLockMain : MonoBehaviour
{
    private ScreenOrientation previousOrientation;

    void Start()
    {
        // Store the previous orientation before locking
        previousOrientation = Screen.orientation;

        // Lock orientation to Landscape
        Screen.orientation = ScreenOrientation.LandscapeLeft;

        // Force update
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
    }

    void OnDestroy()
    {
        // Restore the previous screen orientation when leaving this scene
        Screen.orientation = previousOrientation;
    }
}
