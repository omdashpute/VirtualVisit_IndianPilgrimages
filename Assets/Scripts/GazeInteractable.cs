using UnityEngine;

public class GazeInteractable : MonoBehaviour
{
    public void OnGazeActivate()
    {
        Debug.Log($"Gaze activated: {name}");

        // Check if this object has MediaAudio
        MediaAudio mediaAudio = GetComponent<MediaAudio>();
        if (mediaAudio != null)
        {
            mediaAudio.PlayAudio();
        }

        // Check if this object has MediaImage
        MediaImage mediaImage = GetComponent<MediaImage>();
        if (mediaImage != null)
        {
            mediaImage.ShowImage();
        }
    }
}
