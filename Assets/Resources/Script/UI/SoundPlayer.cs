using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip soundClip;

    // Play the sound when this method is called
    public void PlaySound()
    {
        // If soundClip is set, assign it to the AudioSource
        if (soundClip != null)
        {
            audioSource.clip = soundClip;
        }

        // Play the sound
        audioSource.Play();
    }
}
