using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OptionsSettings", menuName = "ScriptableObjects/OptionsSettings", order = 3)]
public class OptionsMenu : ScriptableObject
{

    public static Action<float> onAmbientVolumeValueChanged;
    public static Action<float> onSoundEffectVolumeValueChanged;
    
    private  float _ambientMusicVolume= 1;
    private  float _soundEffectVolume=1;

    public  float AmbientMusicVolume
    {
        get => _ambientMusicVolume;
    }

    public  float SoundEffectVolume
    {
        get => _soundEffectVolume;
    }


    public  void SetAmbientSound(float value)
    {
        _ambientMusicVolume = value;
        onAmbientVolumeValueChanged?.Invoke(_ambientMusicVolume);
    }

    public  void SetSoundEffectVolume(float value)
    {
        _soundEffectVolume = value;
        onSoundEffectVolumeValueChanged?.Invoke(value);
    }
}
