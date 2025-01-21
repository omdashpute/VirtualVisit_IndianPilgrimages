using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Required for Event Camera

public class CameraController : MonoBehaviour
{
    private float rotateSpeed = 10.0f;
    private float zoomSpeed = 10.0f;
    private float zoomAmount = 0.0f;
    private Vector2 lastTouchPosition;
    private TourManager tourManager;

    public GameObject standardCamera; // Standard camera
    public GameObject xrOrigin; // XR Origin for VR
    public GameObject vrToggleButton; // VR toggle button
    public GameObject reticleCanvas; // Reticle canvas
    public Canvas worldSpaceCanvas; // Reference to the new World Space Canvas

    private bool isVRActive = false;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft; // Lock to landscape left

        tourManager = GetComponent<TourManager>();
        xrOrigin.SetActive(false); // Ensure VR is off initially
        standardCamera.SetActive(true); // Use the standard camera by default
        vrToggleButton.SetActive(true); // Ensure the button is always visible
        reticleCanvas.SetActive(false); // Hide the reticle initially

        if (worldSpaceCanvas != null)
        {
            worldSpaceCanvas.worldCamera = standardCamera.GetComponent<Camera>(); // Set initial Event Camera
        }
    }

    void Update()
    {
        if (tourManager.isCameraMove)
        {
            if (!isVRActive)
            {
                HandleTouchInput(); // Handle touch input only for the standard camera
            }
            else
            {
                HandleGyroInput(); // Handle gyroscope input if VR is active
            }
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                transform.localEulerAngles = new Vector3(
                    transform.localEulerAngles.x - touch.deltaPosition.y * Time.deltaTime * rotateSpeed,
                    transform.localEulerAngles.y + touch.deltaPosition.x * Time.deltaTime * rotateSpeed,
                    0
                );
            }
        }

        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                float prevTouchDeltaMag = (touch0.position - touch0.deltaPosition).magnitude -
                                          (touch1.position - touch1.deltaPosition).magnitude;
                float touchDeltaMag = (touch0.position - touch1.position).magnitude;

                zoomAmount = Mathf.Clamp(zoomAmount + (prevTouchDeltaMag - touchDeltaMag) * Time.deltaTime * zoomSpeed, -5.0f, 5.0f);
                Camera.main.transform.localPosition = new Vector3(0, 0, zoomAmount);
            }
        }
    }

    private void HandleGyroInput()
    {
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;
            Quaternion gyroRotation = Input.gyro.attitude;
            gyroRotation = Quaternion.Euler(-90, 0, 0) * gyroRotation;
            gyroRotation = new Quaternion(-gyroRotation.x, -gyroRotation.y, gyroRotation.z, gyroRotation.w);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, gyroRotation, rotateSpeed * Time.deltaTime);
        }
    }

    public void ToggleVR()
    {
        isVRActive = !isVRActive;

        if (isVRActive)
        {
            standardCamera.SetActive(false);
            xrOrigin.SetActive(true);
            reticleCanvas.SetActive(true);

            if (worldSpaceCanvas != null)
            {
                worldSpaceCanvas.worldCamera = xrOrigin.GetComponentInChildren<Camera>(); // Switch to VR camera
            }
        }
        else
        {
            xrOrigin.SetActive(false);
            standardCamera.SetActive(true);
            reticleCanvas.SetActive(false);

            if (worldSpaceCanvas != null)
            {
                worldSpaceCanvas.worldCamera = standardCamera.GetComponent<Camera>(); // Switch back to normal camera
            }
        }
    }

    public void ResetCamera()
    {
        if (transform != null)
        {
            transform.localEulerAngles = Vector3.zero;
        }
        zoomAmount = 0.0f;

        if (Camera.main != null)
        {
            Camera.main.transform.localPosition = new Vector3(0, 0, zoomAmount);
        }
    }
}