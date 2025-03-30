using UnityEngine;

public class MediaImage : MonoBehaviour
{
    private GameObject canvasImage;
    private TourManager tourManager;

    public void Initialize(GameObject assignedCanvas, TourManager manager)
    {
        canvasImage = assignedCanvas;
        tourManager = manager;

        if (canvasImage)
            canvasImage.SetActive(false);
        else
            Debug.LogWarning("Canvas Image is not assigned!");
    }

    public void ShowImage()
    {
        canvasImage?.SetActive(true);
        // tourManager?.OpenMedia(); // To uncomment if needed...
    }

    public void HideImage()
    {
        canvasImage?.SetActive(false);
        tourManager?.ReturnToSite();
    }
}
