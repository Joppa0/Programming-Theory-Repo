using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource musicSource, effectSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(AudioClip clip, float volume)
    {
        effectSource.PlayOneShot(clip, volume);
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }

    public void ToggleEffects()
    {
        effectSource.mute = !effectSource.mute;
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }
}
