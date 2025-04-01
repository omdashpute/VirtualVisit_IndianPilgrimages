using UnityEngine;

public class GazeInteractable : MonoBehaviour
{
    public void OnGazeActivate()
    {
        Debug.Log($"Gaze activated: {name}");

        if (TryGetComponent(out MediaAudio mediaAudio))
            mediaAudio.PlayAudio();

        if (TryGetComponent(out MediaImage mediaImage))
            mediaImage.ShowImage();
    }
}
