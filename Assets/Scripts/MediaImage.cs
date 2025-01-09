using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediaImage : MonoBehaviour
{
    public GameObject canvasImage;
    public TourManager tourManager;

    void Start()
    {
        canvasImage.SetActive(false);
    }

    public void ShowImage()
    {
        canvasImage.SetActive(true);
        tourManager.OpenMedia();
    }

    public void HideImage()
    {
        canvasImage.SetActive(false);
        tourManager.ReturnToSite();
    }
}