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

    void Start()
    {
        tourManager = GetComponent<TourManager>();
    }

    void Update()
    {
        if (tourManager.isCameraMove)
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
                    float prevTouchDeltaMag = (touch0.position - touch0.deltaPosition).magnitude - (touch1.position - touch1.deltaPosition).magnitude;
                    float touchDeltaMag = (touch0.position - touch1.position).magnitude;

                    zoomAmount = Mathf.Clamp(zoomAmount + (prevTouchDeltaMag - touchDeltaMag) * Time.deltaTime * zoomSpeed, -5.0f, 5.0f);
                    Camera.main.transform.localPosition = new Vector3(0, 0, zoomAmount);
                }
            }
        }
    }

    public void ResetCamera()
    {
        transform.localEulerAngles = new Vector3(0, 0, 0);
        zoomAmount = 0.0f;
        Camera.main.transform.localPosition = new Vector3(0, 0, zoomAmount);
    }
}