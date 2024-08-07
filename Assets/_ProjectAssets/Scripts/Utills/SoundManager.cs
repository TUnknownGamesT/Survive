using System;
using System.Collections.Generic;
using System.Diagnostics;
using ConstantsValues;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public List<AudioClip> musicClips;
    public AudioClip clickButtonSound;
    public AudioClip playerUpgrade;
    public AudioClip baseUpgrade;
    public AudioClip gunsUpgrade;
    public AudioClip enemyUpgrade;
    
    private static AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        GunsUpgrade.onGunsUpgradeSelected += ClickButtonSound;
        PlayerUpgrades.onPlayerUpgradeSelected += ClickButtonSound;
        BaseUpgrade.onBaseUpgradeSelected += ClickButtonSound;
        OptionsMenu.onAmbientVolumeValueChanged += SetMusicVolume;
        UpgradePanelBehaviour.onUpgradeCardInFront += SoundEffect;
    }

    private void OnDisable()
    {
        GunsUpgrade.onGunsUpgradeSelected -= ClickButtonSound;
        PlayerUpgrades.onPlayerUpgradeSelected -= ClickButtonSound;
        BaseUpgrade.onBaseUpgradeSelected -= ClickButtonSound;
        OptionsMenu.onAmbientVolumeValueChanged -= SetMusicVolume;
        UpgradePanelBehaviour.onUpgradeCardInFront -= SoundEffect;
        
    }

    private void Start()
    {
        // _audioSource.clip = musicClips[Random.Range(0, musicClips.Count)];
        // _audioSource.Play();
    }

    private void SoundEffect(UpgradeType obj)
    {
        switch (obj)
        {
            case UpgradeType.Base:
                _audioSource.PlayOneShot(baseUpgrade);
                break;
            case UpgradeType.Enemy:
                _audioSource.PlayOneShot(enemyUpgrade);
                break;
            case UpgradeType.Guns:
                _audioSource.PlayOneShot(gunsUpgrade);
                break;
            case UpgradeType.Player:
                _audioSource.PlayOneShot(playerUpgrade);
                break;
        }
    }
    
    private static void SetMusicVolume(float value)
    {
        _audioSource.volume = value;
    }

    private void ClickButtonSound(UpgradeType obj)
    {
        _audioSource.PlayOneShot(clickButtonSound);
    }
    
    public static void PlayAudioClip(AudioClip audioClip)
    {
        _audioSource.PlayOneShot(audioClip);
    }
}
