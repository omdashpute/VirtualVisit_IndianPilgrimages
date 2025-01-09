using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourManager : MonoBehaviour
{
    public GameObject[] objSites;
    public GameObject canvasMainMenu;
    public bool isCameraMove = false;

    void Start()
    {
        ReturnToMenu();
    }

    void Update()
    {
        // Handle back button press on Android
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isCameraMove)
            {
                ReturnToMenu(); // Go back to the main menu
            }
            else
            {
                Application.Quit(); // Exit the app if on the main menu
            }
        }

        if (isCameraMove && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    if (hit.transform.CompareTag("Sound"))
                    {
                        MediaAudio mediaAudio = hit.transform.GetComponent<MediaAudio>();
                        if (mediaAudio != null)
                        {
                            mediaAudio.PlayAudio();
                        }
                    }
                    else if (hit.transform.CompareTag("Image"))
                    {
                        MediaImage mediaImage = hit.transform.GetComponent<MediaImage>();
                        if (mediaImage != null)
                        {
                            mediaImage.ShowImage();
                        }
                    }
                }
            }
        }
    }

    public void LoadSite(int siteNumber)
    {
        objSites[siteNumber].SetActive(true);
        canvasMainMenu.SetActive(false);
        isCameraMove = true;

        GetComponent<CameraController>().ResetCamera();
    }

    public void ReturnToMenu()
    {
        canvasMainMenu.SetActive(true);

        foreach (GameObject site in objSites)
        {
            site.SetActive(false);
        }

        isCameraMove = false;
    }

    public void ReturnToSite()
    {
        isCameraMove = true;
    }

    public void OpenMedia()
    {
        isCameraMove = false;
    }
}