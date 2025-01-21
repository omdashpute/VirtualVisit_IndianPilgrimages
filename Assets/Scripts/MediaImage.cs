using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaImage : MonoBehaviour
{
    private GameObject canvasImage;  // Reference to the description canvas
    private TourManager tourManager;

    public void Initialize(GameObject assignedCanvas, TourManager manager)
    {
        canvasImage = assignedCanvas;
        tourManager = manager;

        if (canvasImage != null)
            canvasImage.SetActive(false);  // Ensure it's hidden initially
        else
            Debug.LogWarning("Canvas Image is not assigned!");
    }

    public void ShowImage()
    {
        if (canvasImage != null)
        {
            canvasImage.SetActive(true);
            //tourManager.OpenMedia();
        }
    }

    public void HideImage()
    {
        if (canvasImage != null)
            canvasImage.SetActive(false);

        tourManager.ReturnToSite();
    }
}
