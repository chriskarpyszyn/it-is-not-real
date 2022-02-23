using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private bool firstMovement = false;
    private bool waitingForSound = false;

    public AudioClip enterPortalSound;
    public float enterPortalSoundPitch = 1;
    public float enterPortalSoundVolume = 0.8f;

    public AudioClip playerMovementSound;
    public float playerMovementSoundPitch = 1;
    public float playerMovmentSoundVolume = 0.8f;
    public float playerMovementSoundFirstDalay = 0.5f;
    public float playerMovementSoundDelay = 0.3f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Portal")
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = enterPortalSound;

            //slight pitch shift to make things different
            float randomPitchShift = slightPitchShift(enterPortalSoundPitch);

            audioSource.pitch = enterPortalSoundPitch;
            audioSource.volume = enterPortalSoundVolume;
            audioSource.Play();
        }
    }

    private float slightPitchShift(float startingNumber)
    {
        return Random.Range(startingNumber - 0.8f, startingNumber + 0.8f);
    }

    public void playPlayerMovementSound()
    {
      if (firstMovement == true)
        {
            firstMovement = false;
            waitingForSound = true;
            Invoke("firstMovementSound", playerMovementSoundFirstDalay);
        } else
        {
            if (!waitingForSound)
            {
                playWithDelay(playerMovementSoundDelay);
            }
        }
    }

    private void firstMovementSound()
    {
        Debug.Log("TRUE)");
        playWithDelay(0);
        waitingForSound = false;
    }

    private void playWithDelay(float delay)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = playerMovementSound;
            audioSource.pitch = slightPitchShift(playerMovementSoundPitch);
            audioSource.volume = playerMovmentSoundVolume;

            audioSource.PlayDelayed(delay);
        }
    }

    public void setFirstMovement()
    {
        firstMovement = true;
    }
}
