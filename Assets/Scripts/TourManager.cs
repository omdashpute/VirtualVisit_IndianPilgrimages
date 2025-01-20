using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourManager : MonoBehaviour
{
    public GameObject[] objSites;           // Array of sites
    public GameObject canvasMainMenu;       // Main menu canvas
    public GameObject vrToggleButton;    // Reference to VR toggle button
    public bool isCameraMove = false;       // Camera move flag

    void Start()
    {
        if (vrToggleButton != null)
            vrToggleButton.SetActive(true);      // Ensure VR toggle button is visible
        else
            Debug.LogWarning("VR Toggle Button is not assigned!");

        if (canvasMainMenu != null)
            ReturnToMenu();                      // Go back to the main menu at the start
        else
            Debug.LogWarning("Main Menu Canvas is not assigned!");
    }

    void Update()
    {
        // Handle back button press on Android (Escape key for testing)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isCameraMove)
            {
                ReturnToMenu(); // Return to the main menu
            }
            else
            {
                Application.Quit(); // Exit the app if on the main menu
            }
        }

        // Check if camera is moving (allow interactions)
        if (isCameraMove)
        {
            // Handle touch input (raycast for touch interaction)
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Ended)
                {
                    // Check if Camera.main is null and skip the raycast if it's not found
                    if (Camera.main != null)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(touch.position);
                        RaycastHit hit;

                        if (Physics.Raycast(ray, out hit, 100.0f))
                        {
                            // Check if we hit an interactable object
                            if (hit.transform.CompareTag("Interactable"))
                            {
                                // Trigger LoadSite based on the touched object
                                HandleSiteSelection(hit.transform.gameObject);
                            }
                        }
                    }
                    // Ignore the "Main Camera not found" message silently without logging
                }
            }

            // Handle gaze interaction (handled by another script, such as GazeController)
        }
    }



    public void HandleSiteSelection(GameObject hitObject)
    {
        // Check if the hit object has a SpriteRenderer and is tagged as "Interactable"
        SpriteRenderer spriteRenderer = hitObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && hitObject.CompareTag("Interactable"))
        {
            // Find the index of the site based on the sprite
            for (int i = 0; i < objSites.Length; i++)
            {
                if (hitObject == objSites[i]) // Match the sprite with the corresponding site
                {
                    LoadSite(i); // Load the site
                    break;
                }
            }

            // Handle any media interactions if the sprite has audio or image components
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


    public void LoadSite(int siteNumber)
    {
        objSites[siteNumber].SetActive(true);         // Activate the selected site
        canvasMainMenu.SetActive(false);              // Hide the main menu
        isCameraMove = true;                          // Enable camera movement

        // Reset camera for the new site (if needed)
        GetComponent<CameraController>().ResetCamera();
    }

    public void ReturnToMenu()
    {
        canvasMainMenu.SetActive(true);               // Show the main menu
        foreach (GameObject site in objSites)
        {
            site.SetActive(false);                    // Deactivate all sites
        }
        isCameraMove = false;                         // Disable camera movement
    }

    public void ReturnToSite()
    {
        isCameraMove = true;                          // Enable camera movement
    }

    public void OpenMedia()
    {
        isCameraMove = false;                         // Disable camera movement
    }
}
