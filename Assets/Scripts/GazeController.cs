using System.Collections;
using System.Collections.Generic;
using TS.GazeInteraction;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required for button click simulation

public class GazeController : MonoBehaviour
{
    public Camera leftEyeCamera;
    public Camera rightEyeCamera;
    public Image leftReticle;
    public Image rightReticle;
    public float gazeDuration = 2.0f;

    private float gazeTimer = 0.0f;
    private GameObject currentGazeObject = null;
    private TourManager tourManager;

    void Start()
    {
        // Ensure TourManager is correctly assigned
        tourManager = FindObjectOfType<TourManager>();

        if (tourManager == null)
        {
            Debug.LogError("TourManager not found in the scene!");
        }
    }

    void Update()
    {
        bool leftHit = ProcessGaze(leftEyeCamera, leftReticle);
        bool rightHit = ProcessGaze(rightEyeCamera, rightReticle);

        if (leftHit && rightHit && currentGazeObject != null)
        {
            gazeTimer += Time.deltaTime;
            leftReticle.fillAmount = gazeTimer / gazeDuration;
            rightReticle.fillAmount = gazeTimer / gazeDuration;

            if (gazeTimer >= gazeDuration)
            {
                TriggerGazeAction(currentGazeObject);
                ResetGaze();
            }
        }
        else
        {
            ResetGaze();
        }
    }

    private bool ProcessGaze(Camera eyeCamera, Image reticle)
{
    Ray ray = new Ray(eyeCamera.transform.position, eyeCamera.transform.forward);
    RaycastHit hit;

    // 1️⃣ Check for 3D objects using Physics.Raycast
    if (Physics.Raycast(ray, out hit))
    {
        GameObject hitObject = hit.collider.gameObject;

        if (hitObject != currentGazeObject)
        {
            currentGazeObject = hitObject;
            gazeTimer = 0.0f;
        }

        reticle.fillAmount = gazeTimer / gazeDuration;
        return hitObject.CompareTag("Interactable");
    }

    // 2️⃣ Check for UI buttons using EventSystem.RaycastAll
    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
    pointerEventData.position = new Vector2(Screen.width / 2, Screen.height / 2); // Center of screen (gaze position)

    List<RaycastResult> results = new List<RaycastResult>();
    EventSystem.current.RaycastAll(pointerEventData, results);

    foreach (RaycastResult result in results)
    {
        Button button = result.gameObject.GetComponent<Button>();
        if (button != null)
        {
            currentGazeObject = result.gameObject;
            gazeTimer += Time.deltaTime;
            reticle.fillAmount = gazeTimer / gazeDuration;

            if (gazeTimer >= gazeDuration)
            {
                button.onClick.Invoke(); // Simulate button click
                ResetGaze();
                return true;
            }
        }
    }

    return false;
}

    private void ResetGaze()
    {
        gazeTimer = 0.0f;
        leftReticle.fillAmount = 0.0f;
        rightReticle.fillAmount = 0.0f;
        currentGazeObject = null;
    }

    private void TriggerGazeAction(GameObject target)
    {
        Debug.Log($"Gaze activated on {target.name}");

        // Check if the target has a Button component (Unity UI Button)
        Button button = target.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.Invoke(); // Simulate button click via gaze
            return; // Exit to prevent other logic from running
        }

        // If it's a site, handle site selection
        tourManager.HandleSiteSelection(target);

        // Play audio if present
        MediaAudio mediaAudio = target.GetComponent<MediaAudio>();
        if (mediaAudio != null)
        {
            mediaAudio.PlayAudio();
        }

        // Show image if present
        MediaImage mediaImage = target.GetComponent<MediaImage>();
        if (mediaImage != null)
        {
            mediaImage.ShowImage();
        }
    }

}
