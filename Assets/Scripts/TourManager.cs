using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourManager : MonoBehaviour
{
    // List of sites
    public GameObject[] objSites;

    // Main menu
    public GameObject canvasMainMenu;

    // Should the camera move
    public bool isCameraMove = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isCameraMove)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ReturnToMenu();
            }
        }
    }

    public void LoadSite(int siteNumber)
    {
        // Show site
        objSites[siteNumber].SetActive(true);

        // Hide menu
        canvasMainMenu.SetActive(false);

        // Enable camera
        isCameraMove = true;
    }

    public void ReturnToMenu()
    {
        // Show menu
        canvasMainMenu.SetActive(true);

        // Hide sites
        for (int i = 0; i < objSites.Length; i++) 
        {
            objSites[i].SetActive(false);
        }

        // Disable the camera
        isCameraMove = false;
    }
}
