using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Sensitivity of mouse
    private float rotateSpeed = 300.0f;

    //zoom in/out variables
    private float zoomSpeed = 600.0f;
    private float zoomAmount = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // Rotate camera according to the mouse
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x + Input.GetAxis("Mouse Y") * Time.deltaTime * rotateSpeed, transform.localEulerAngles.y + Input.GetAxis("Mouse X") * Time.deltaTime * rotateSpeed, 0);
        }

        if(Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            //move the camera forward/back
            zoomAmount = Mathf.Clamp(zoomAmount + Input.GetAxis("Mouse Y") * Time.deltaTime * zoomSpeed, -5.0f, 5.0f);
            Camera.main.transform.localPosition = new Vector3(0 ,0 ,zoomAmount);
        }
    }
}
