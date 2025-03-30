using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    private float rotateSpeed = 10.0f;
    private float zoomSpeed = 2.5f;
    private float zoomAmount = 0.0f;
    private TourManager tourManager;

    public GameObject standardCamera; // Standard (non-VR) camera
    public GameObject xrOrigin; // XR Origin for VR
    public GameObject vrToggleButton;
    public GameObject reticleCanvas;
    public Canvas worldSpaceCanvas;

    private bool isVRActive = false;
    private bool isVRZoomEnabled = false;
    private Transform vrCamera;

    public Image targetImage1;
    public Image targetImage2;

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft; // Lock to landscape

        tourManager = GetComponent<TourManager>();
        xrOrigin.SetActive(false);
        standardCamera.SetActive(true);
        vrToggleButton.SetActive(true);
        reticleCanvas.SetActive(false);

        if (worldSpaceCanvas != null)
            worldSpaceCanvas.worldCamera = standardCamera.GetComponent<Camera>();
    }

    void Update()
    {
        if (!tourManager.isCameraMove) return;

        if (isVRActive)
        {
            HandleGyroInput();
            if (isVRZoomEnabled) HandleVRZoom();
        }
        else
        {
            HandleTouchInput();
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount == 1) // Single touch for rotation
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                transform.localEulerAngles += new Vector3(
                    -touch.deltaPosition.y * Time.deltaTime * rotateSpeed,
                    touch.deltaPosition.x * Time.deltaTime * rotateSpeed,
                    0
                );
            }
        }

        if (Input.touchCount == 2) // Pinch-to-zoom
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
            {
                // Fix orientation issue by swapping X and Y
                Vector2 prevTouch0 = new Vector2(touch0.position.y - touch0.deltaPosition.y, touch0.position.x - touch0.deltaPosition.x);
                Vector2 prevTouch1 = new Vector2(touch1.position.y - touch1.deltaPosition.y, touch1.position.x - touch1.deltaPosition.x);

                float prevDistance = (prevTouch0 - prevTouch1).magnitude;
                float currentDistance = (touch0.position - touch1.position).magnitude;

                zoomAmount = Mathf.Clamp(zoomAmount + (currentDistance - prevDistance) * Time.deltaTime * zoomSpeed, -5.0f, 5.0f);
                Camera.main.transform.localPosition = new Vector3(0, 0, zoomAmount);
            }
        }
    }

    private void HandleGyroInput()
    {
        if (!SystemInfo.supportsGyroscope) return;

        Input.gyro.enabled = true;
        Quaternion gyroRotation = Input.gyro.attitude;
        gyroRotation = Quaternion.Euler(-90, 0, 0) * gyroRotation;
        gyroRotation = new Quaternion(-gyroRotation.x, -gyroRotation.y, gyroRotation.z, gyroRotation.w);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, gyroRotation, rotateSpeed * Time.deltaTime);
    }

    private void HandleVRZoom()
    {
        if (vrCamera == null) return;

        float headTilt = vrCamera.eulerAngles.x;
        if (headTilt > 180) headTilt -= 360; // Normalize angle

        float zoomDirection = headTilt > 10f ? 1f : headTilt < -10f ? -1f : 0f;
        float zoomChange = zoomDirection * (zoomSpeed * 0.1f) * Time.deltaTime;

        xrOrigin.transform.position += vrCamera.forward * zoomChange;
    }

    public void ToggleVR()
    {
        isVRActive = !isVRActive;
        targetImage2.color = isVRActive ? Color.red : Color.white;

        standardCamera.SetActive(!isVRActive);
        xrOrigin.SetActive(isVRActive);
        reticleCanvas.SetActive(isVRActive);

        if (worldSpaceCanvas != null)
            worldSpaceCanvas.worldCamera = isVRActive ? xrOrigin.GetComponentInChildren<Camera>() : standardCamera.GetComponent<Camera>();

        if (isVRActive)
            vrCamera = xrOrigin.GetComponentInChildren<Camera>().transform;
    }

    public void ResetCamera()
    {
        transform.localEulerAngles = Vector3.zero;
        zoomAmount = 0.0f;

        if (Camera.main != null)
            Camera.main.transform.localPosition = new Vector3(0, 0, zoomAmount);
    }

    public void ToggleVRZoom()
    {
        isVRZoomEnabled = !isVRZoomEnabled;
        targetImage1.color = isVRZoomEnabled ? Color.red : Color.white;
    }
}
