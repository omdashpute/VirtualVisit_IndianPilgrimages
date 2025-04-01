using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourManager : MonoBehaviour
{
    public GameObject[] objSites;          // Array of sites
    public GameObject[] canvasDescriptions; // Array of description canvases for each site
    public GameObject canvasSubMenu;       // Sub-menu canvas (3D menu)
    public GameObject vrToggleButton;      // Reference to VR toggle button
    public bool isCameraMove = false;      // Camera move flag

    void Start()
    {
        if (vrToggleButton != null)
            vrToggleButton.SetActive(true);
        else
            Debug.LogWarning("VR Toggle Button is not assigned!");

        if (canvasSubMenu != null)
        {
            LoadSubMenu();
        }
        else
        {
            Debug.LogWarning("Sub Menu Canvas is not assigned!");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isCameraMove)
            {
                ReturnToMenu();
            }
            else
            {
                Application.Quit();
            }
        }

        if (isCameraMove)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Ended)
                {
                    if (Camera.main != null)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(touch.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, 100.0f))
                        {
                            if (hit.transform.CompareTag("Interactable"))
                            {
                                HandleSiteSelection(hit.transform.gameObject);
                            }
                        }
                    }
                }
            }
        }
    }

    public void HandleSiteSelection(GameObject hitObject)
    {
        SpriteRenderer spriteRenderer = hitObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && hitObject.CompareTag("Interactable"))
        {
            for (int i = 0; i < objSites.Length; i++)
            {
                if (hitObject == objSites[i])
                {
                    LoadSite(i);
                    break;
                }
            }

            MediaAudio mediaAudio = hitObject.GetComponent<MediaAudio>();
            if (mediaAudio != null)
            {
                mediaAudio.PlayAudio();
            }

            MediaImage mediaImage = hitObject.GetComponent<MediaImage>();
            if (mediaImage != null)
            {
                mediaImage.ShowImage();
            }
        }
    }

    public void LoadSubMenu()
    {
        if (canvasSubMenu != null)
        {
            canvasSubMenu.SetActive(true);
        }

        for (int i = 0; i < objSites.Length; i++)
        {
            objSites[i].SetActive(i == 0);
        }

        // Hide all description canvases
        foreach (GameObject description in canvasDescriptions)
        {
            description.SetActive(false);
        }

        isCameraMove = true;
    }


    public void LoadSite(int siteNumber)
    {
        foreach (GameObject site in objSites)
        {
            site.SetActive(false);
        }

        objSites[siteNumber].SetActive(true);
        canvasSubMenu.SetActive(false);
        isCameraMove = true;

        GetComponent<CameraController>().ResetCamera();

        // Assign the correct description canvas, skipping the submenu (siteNumber 0)
        if (siteNumber > 0 && (siteNumber - 1) < canvasDescriptions.Length)
        {
            MediaImage mediaImage = objSites[siteNumber].GetComponentInChildren<MediaImage>();
            if (mediaImage != null)
            {
                mediaImage.Initialize(canvasDescriptions[siteNumber - 1], this);
            }
        }
    }

    public void ReturnToMenu()
    {
        LoadSubMenu();
    }

    public void ReturnToSite()
    {
        isCameraMove = true;
    }

    /*public void OpenMedia()
    {
        isCameraMove = false;
    }*/

    public void BackButton()
    {
        if (isCameraMove)
        {
            ReturnToMenu();
        }
        else
        {
            Application.Quit();
        }
    }

    public void QuitApp()
    {
        Application.Quit(); // Closes the app on a real device
        Debug.Log("App is closing..."); // Just for debugging in Unity Editor
    }
}
