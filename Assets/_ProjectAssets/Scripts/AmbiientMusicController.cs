using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AmbiientMusicController : MonoBehaviour
{
    public List<AudioClip> musicClips;
    
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        OptionsMenu.onAmbientVolumeValueChanged += SetMusicVolume;
    }
    
    private void OnDisable()
    {
        OptionsMenu.onAmbientVolumeValueChanged -= SetMusicVolume;
    }

    private void Start()
    {
        // _audioSource.clip = musicClips[Random.Range(0, musicClips.Count)];
        // _audioSource.Play();
    }
    
    private void SetMusicVolume(float value)
    {
        _audioSource.volume = value;
    }
}
