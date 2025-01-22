using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; // Required for Event Camera

public class CameraController : MonoBehaviour
{
    private float rotateSpeed = 10.0f;
    private float zoomSpeed = 5.0f; // Adjusted for smoother zoom
    private float zoomAmount = 0.0f;
    private Vector2 lastTouchPosition;
    private TourManager tourManager;

    public GameObject standardCamera; // Standard camera
    public GameObject xrOrigin; // XR Origin for VR
    public GameObject vrToggleButton; // VR toggle button
    public GameObject reticleCanvas; // Reticle canvas
    public Canvas worldSpaceCanvas; // Reference to the new World Space Canvas

    private bool isVRActive = false;
    private bool isVRZoomEnabled = false; //  NEW: Toggle for VR Zoom
    private Transform vrCamera; // VR camera reference

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
                if (isVRZoomEnabled) //  Zoom only when enabled
                {
                    HandleVRZoom();
                }
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

    private void HandleVRZoom()
    {
        if (vrCamera == null) return;

        float headTiltAngle = vrCamera.eulerAngles.x;
        if (headTiltAngle > 180) headTiltAngle -= 360; // Normalize angle to -180 to 180

        float zoomDirection = 0f;

        if (headTiltAngle > 10f) // Tilt down -> Zoom in
        {
            zoomDirection = 1f;
        }
        else if (headTiltAngle < -10f) // Tilt up -> Zoom out
        {
            zoomDirection = -1f;
        }

        float zoomChange = zoomDirection * (zoomSpeed * 0.1f) * Time.deltaTime; // Reduced zoom speed
        xrOrigin.transform.position += vrCamera.forward * zoomChange;
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

            vrCamera = xrOrigin.GetComponentInChildren<Camera>().transform; // Get VR Camera Transform
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

    // NEW: Toggle function for enabling/disabling VR zoom
    public void ToggleVRZoom()
    {
        isVRZoomEnabled = !isVRZoomEnabled;
    }
}
