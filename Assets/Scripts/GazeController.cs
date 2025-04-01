using System.Collections;
using System.Collections.Generic;
using TS.GazeInteraction;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
        tourManager = FindObjectOfType<TourManager>();

        if (tourManager == null)
            Debug.LogError("TourManager not found in the scene!");
    }

    void Update()
    {
        bool leftHit = ProcessGaze(leftEyeCamera, leftReticle);
        bool rightHit = ProcessGaze(rightEyeCamera, rightReticle);

        if (leftHit && rightHit && currentGazeObject != null)
        {
            gazeTimer += Time.deltaTime;
            UpdateReticleFill();

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

        // Check for 3D interactable objects
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

        // Check for UI buttons
        return CheckUIInteraction(reticle);
    }

    private bool CheckUIInteraction(Image reticle)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Screen.width / 2, Screen.height / 2) // Center of screen
        };

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
                    button.onClick.Invoke();
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

    private void UpdateReticleFill()
    {
        leftReticle.fillAmount = gazeTimer / gazeDuration;
        rightReticle.fillAmount = gazeTimer / gazeDuration;
    }

    private void TriggerGazeAction(GameObject target)
    {
        Debug.Log($"Gaze activated on {target.name}");

        if (target.TryGetComponent(out Button button))
        {
            button.onClick.Invoke();
            return;
        }

        tourManager.HandleSiteSelection(target);

        if (target.TryGetComponent(out MediaAudio mediaAudio))
            mediaAudio.PlayAudio();

        if (target.TryGetComponent(out MediaImage mediaImage))
            mediaImage.ShowImage();
    }
}
