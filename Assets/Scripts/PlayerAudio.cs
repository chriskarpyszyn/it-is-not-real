using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource audioSource1;
    private AudioSource audioSource2;
    private AudioSource audioSource3;
    private AudioSource[] audioSources;
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

    public AudioClip playerJumpSound;
    public float playerJumpSoundPitch = 1;
    public float playerJumpSoundVolume = 0.8f;

    public AudioClip playerLandSound;
    public float playerLandSoundPitch = 1;
    public float playerLandSoundVolume = 0.8f;

    public AudioClip deathSound;
    public float deathSoundPitch = 1;
    public float deathSoundVolume = 0.8f;

    public AudioClip ghostDeathSound;
    public float ghostDeathSoundPitch = 1;
    public float ghostDeathSoundVolume = 0.8f;

    private void Start()
    {
        audioSources = GetComponents<AudioSource>();
        audioSource1 = audioSources[0];
        audioSource2 = audioSources[1];
    }

    private void OnTriggerEnter(Collider other)
    {
        //todo-ck this class is becoming more of an audio manager, 
        //and i can refactor this ontrigger back to the player class and call a play function
        //more directly.
        if (other.gameObject.tag == "Portal")
        {
            audioSource1 = GetComponent<AudioSource>();
            audioSource1.clip = enterPortalSound;

            //slight pitch shift to make things different
            float randomPitchShift = slightPitchShift(enterPortalSoundPitch);

            audioSource1.pitch = enterPortalSoundPitch;
            audioSource1.volume = enterPortalSoundVolume;
            audioSource1.Play();
        }
    }

    private float slightPitchShift(float startingNumber)
    {
        return Random.Range(startingNumber - 0.2f, startingNumber + 0.2f);
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
                playWalkingWithDelay(playerMovementSoundDelay);
            }
        }
    }
    private void firstMovementSound()
    {
        playWalkingWithDelay(0);
        waitingForSound = false;
    }
    private void playWalkingWithDelay(float delay)
    {
        if (!audioSource2.isPlaying)
        {
            audioSource2.clip = playerMovementSound;
            audioSource2.pitch = slightPitchShift(playerMovementSoundPitch);
            audioSource2.volume = playerMovmentSoundVolume;

            audioSource2.PlayDelayed(delay);
        }
    }

    public void playJumpSound()
    {
        playSound(playerJumpSound, playerJumpSoundPitch, playerJumpSoundVolume);
    }

    public void playDeathSound()
    {
        playSound(deathSound, deathSoundPitch, deathSoundVolume, false);
    }

    public void playGhostDeathSound()
    {
        playSound(ghostDeathSound, ghostDeathSoundPitch, ghostDeathSoundVolume, false);

    }

    private bool skipFirstTime = false;
    public void playLandingSound()
    {
        if (skipFirstTime)
        {
            playSound(playerLandSound, playerLandSoundPitch, playerLandSoundVolume);

        }else
        {
            skipFirstTime = true;
        }
    }

    private void playSound(AudioClip clip, float pitch, float vol, bool shiftPitch)
    {
        AudioSource audioSource1 = getAvailableAudioSource();
        audioSource1.clip = clip;
        if (shiftPitch)
            audioSource1.pitch = slightPitchShift(pitch);
        else
            audioSource1.pitch = pitch;
        audioSource1.volume = vol;
        audioSource1.Play();

    }
    private void playSound(AudioClip clip, float pitch, float vol)
    {
        playSound(clip, pitch, vol, true);
    }
    
    private AudioSource getAvailableAudioSource()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }
        return audioSources[0];
    }

    public void setFirstMovement()
    {
        firstMovement = true;
    }
}
