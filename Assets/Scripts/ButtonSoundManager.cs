using UnityEngine;
using UnityEngine.UI;

public class ButtonSoundManager : MonoBehaviour
{
    public static ButtonSoundManager instance;
    private AudioSource audioSource;

    public AudioClip buttonClickSound;

    private void Awake()
    {
        // Ensure only one instance exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();
    }

    public void PlayButtonSound()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }
}
