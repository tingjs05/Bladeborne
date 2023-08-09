using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SoundEffects : MonoBehaviour
{
    [SerializeField] private string[] soundNames;
    [SerializeField] private AudioClip[] soundClips;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {   
        // get audio source component
        audioSource = GetComponent<AudioSource>();

        // do not run code if each sound does not have a corresponding name
        if (soundNames.Length != soundClips.Length)
        {
            Debug.LogError("Length of soundName and soundClip must be equal!");
            return;
        }
    }

    public void playSound(string sound, bool loop = false)
    {
        int index = Array.IndexOf(soundNames, sound);

        // check if soundNames list contains the sound
        if (index <= -1)
        {
            return;
        }

        // do not play sound if audio source is null
        if (audioSource == null)
        {
            return;
        }

        // stop current audio if something is playing
        stopSound();
        // set audio clip
        audioSource.clip = soundClips[index];
        // set loop
        audioSource.loop = loop;
        // play audio
        audioSource.Play();
    }

    public void stopSound()
    {
        // stop audio if audio is playing
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
