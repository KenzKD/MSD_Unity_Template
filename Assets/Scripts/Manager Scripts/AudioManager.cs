using System;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    // Singleton instance for easy access
    public static AudioManager Instance;

    // Audio mixer for managing audio channels
    public AudioMixer Mixer;

    // Arrays of background music and sound effects
    public Sound[] bgm, sfx;

    // Audio sources for background music and sound effects
    public AudioSource bgmSource, sfxSource;

    // Flag to allow overlapping sound effects
    private static bool sfxAllowOverlap = false;

    // Initialize the audio manager
    void Awake()
    {
        Instance = this; // Set in Awake() for Bgm and Sfx volume preferences
    }

    // Start playing the BGM
    void Start()
    {
        bgmSource.Stop();
        PlayBGM("Theme");
    }

    // Play a background music track by name
    public void PlayBGM(string name)
    {
        Sound s = Array.Find(bgm, x => x.name == name);
        if (s == null)
        {
            Debug.Log($"Sound '{name}' not found!");
        }
        else
        {
            bgmSource.clip = s.clip;
            bgmSource.Play();
        }
    }

    // Play a sound effect by name
    public void PlaySFX(string name)
    {
        Sound sound = Array.Find(sfx, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning($"Sound '{name}' not found!");
            return;
        }

        if (!sfxAllowOverlap)
        {
            StopSFX();
        }

        if (sfxSource.loop)
        {
            sfxSource.clip = sound.clip;
            sfxSource.Play();
        }
        else
        {
            sfxSource.PlayOneShot(sound.clip);
        }
    }

    // Toggle sfxAllowOverlap
    public void SetSFXAllowOverlap(bool allowOverlap)
    {
        sfxAllowOverlap = allowOverlap;
    }

    // Toggle looping of sfxSource
    public void SetSFXLooping(bool isLooping)
    {
        sfxSource.loop = isLooping;
    }

    // Stop sfxSource
    public void StopSFX()
    {
        sfxSource.Stop();
    }

    // Adjust the volume of the background music
    public void BGMVolume(float volume)
    {
        // Ensure volume is not zero to avoid log(0) error
        volume = Mathf.Max(volume, 0.0001f);

        // Convert linear volume to decibels
        float decibels = 20f * Mathf.Log10(volume);

        Mixer.SetFloat("bgmMixerVolume", decibels);
    }

    // Adjust the volume of the sound effects
    public void SFXVolume(float volume)
    {
        // Ensure volume is not zero to avoid log(0) error
        volume = Mathf.Max(volume, 0.0001f);

        // Convert linear volume to decibels
        float decibels = 20f * Mathf.Log10(volume);

        Mixer.SetFloat("sfxMixerVolume", decibels);
    }
}
