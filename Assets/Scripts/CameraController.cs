using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float rotateSpeed = 10.0f;
    private float zoomSpeed = 10.0f;
    private float zoomAmount = 0.0f;
    private TourManager tourManager;
    private Vector2 lastTouchPosition;

    public GameObject standardCamera; // Reference to your standard camera
    public GameObject xrOrigin; // Reference to the XR Origin
    public GameObject vrToggleButton; // VR toggle button
    public GameObject reticleCanvas; // Reticle canvas

    private bool isVRActive = false;

    // Lock device orientation to landscape left at the start
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft; // Lock to landscape left

        tourManager = GetComponent<TourManager>();
        xrOrigin.SetActive(false); // Ensure VR is off initially
        standardCamera.SetActive(true); // Use the standard camera by default
        vrToggleButton.SetActive(true); // Ensure the button is always visible
        reticleCanvas.SetActive(false); // Hide the reticle initially
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
                // Rotate camera based on touch movement
                transform.localEulerAngles = new Vector3(
                    transform.localEulerAngles.x - touch.deltaPosition.y * Time.deltaTime * rotateSpeed,
                    transform.localEulerAngles.y + touch.deltaPosition.x * Time.deltaTime * rotateSpeed,
                    0
                );
            }
        }

        if (Input.touchCount == 2)
        {
            // Zoom based on two-finger pinch gesture
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
        // Get the gyroscope's rotation rate
        if (SystemInfo.supportsGyroscope)
        {
            Input.gyro.enabled = true;

            // Get the gyroscope's attitude (rotation)
            Quaternion gyroRotation = Input.gyro.attitude;

            // Correct the orientation by rotating 90 degrees on the X-axis
            gyroRotation = Quaternion.Euler(-90, 0, 0) * gyroRotation;

            // Invert the axes if necessary to match the expected movement
            gyroRotation = new Quaternion(-gyroRotation.x, -gyroRotation.y, gyroRotation.z, gyroRotation.w);

            // Apply the rotation to the camera
            transform.localRotation = gyroRotation;

            // Optional: Adjust the rotation speed if necessary
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
            reticleCanvas.SetActive(true); // Show the reticle in VR mode
        }
        else
        {
            xrOrigin.SetActive(false);
            standardCamera.SetActive(true);
            reticleCanvas.SetActive(false); // Hide the reticle in standard mode
        }
    }

    public void ResetCamera()
    {
        if (transform != null)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        zoomAmount = 0.0f;

        if (Camera.main != null)
        {
            Camera.main.transform.localPosition = new Vector3(0, 0, zoomAmount);
        }
    }
}
