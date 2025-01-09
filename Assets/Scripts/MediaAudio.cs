using System.Collections;
using System.Collections.Generic;
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
        audioToPlay.Play();
    }

    public void StopAudio()
    {
        audioToPlay.Stop();
    }
}