using System.Collections;
using System.Collections.Generic;
using TS.GazeInteraction;
using UnityEngine;
using UnityEngine.UI;

public class GazeController : MonoBehaviour
{
    public Camera leftEyeCamera; // Reference to the left eye camera
    public Camera rightEyeCamera; // Reference to the right eye camera
    public Image leftReticle; // Reticle for the left eye
    public Image rightReticle; // Reticle for the right eye
    public float gazeDuration = 2.0f; // Time required to activate the object

    private float gazeTimer = 0.0f; // Timer to track gaze duration
    private GameObject currentGazeObject = null; // Object currently being gazed at

    private TourManager tourManager; // Reference to the TourManager script

    void Start()
    {
        // Get reference to the TourManager (assumes it's attached to the same GameObject)
        tourManager = GetComponent<TourManager>();
    }

    void Update()
    {
        // Cast rays from both cameras
        bool leftHit = ProcessGaze(leftEyeCamera, leftReticle);
        bool rightHit = ProcessGaze(rightEyeCamera, rightReticle);

        // If both rays hit the same object, process the gaze action
        if (leftHit && rightHit && currentGazeObject != null)
        {
            gazeTimer += Time.deltaTime;
            leftReticle.fillAmount = gazeTimer / gazeDuration;
            rightReticle.fillAmount = gazeTimer / gazeDuration;

            if (gazeTimer >= gazeDuration)
            {
                // Trigger the gaze action (load the site)
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
        // Log the gaze interaction
        Debug.Log($"Gaze activated on {target.name}");

        // Call the method from the TourManager to load the corresponding site
        tourManager.HandleSiteSelection(target);

        // Trigger any additional actions such as audio or image display
        MediaAudio mediaAudio = target.GetComponent<MediaAudio>();
        if (mediaAudio != null)
        {
            mediaAudio.PlayAudio();
        }

        MediaImage mediaImage = target.GetComponent<MediaImage>();
        if (mediaImage != null)
        {
            mediaImage.ShowImage();
        }
    }
}
