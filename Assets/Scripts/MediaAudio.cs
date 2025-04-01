using UnityEngine;

public class MediaAudio : MonoBehaviour
{
    private AudioSource audioToPlay;

    void Start()
    {
        audioToPlay = GetComponent<AudioSource>();
    }

    public void PlayAudio()
    {
        (audioToPlay ??= GetComponent<AudioSource>())?.Play();
    }

    public void StopAudio()
    {
        audioToPlay?.Stop();
    }
}
