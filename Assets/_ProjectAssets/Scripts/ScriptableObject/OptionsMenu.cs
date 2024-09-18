using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OptionsSettings", menuName = "ScriptableObjects/OptionsSettings", order = 3)]
public class OptionsMenu : ScriptableObject
{

    public static Action<float> onAmbientVolumeValueChanged;
    public static Action<float> onSoundEffectVolumeValueChanged;


    private float _ambientMusicVolume = 1;
    private float _soundEffectVolume = 1;

    public float AmbientMusicVolume
    {
        get => _ambientMusicVolume;
    }

    public float SoundEffectVolume
    {
        get => _soundEffectVolume;
    }


    public void SetAmbientSound(float value)
    {
        float sound = Mathf.Clamp01(_ambientMusicVolume + value);
        _ambientMusicVolume = Mathf.Round(sound * 1000f) / 1000f;
        onAmbientVolumeValueChanged?.Invoke(_ambientMusicVolume);
    }

    public void SetSoundEffectVolume(float value)
    {
        float sound = Mathf.Clamp01(_soundEffectVolume + value);
        _soundEffectVolume = Mathf.Round(sound * 1000f) / 1000f;
        onSoundEffectVolumeValueChanged?.Invoke(_soundEffectVolume);
    }
}
