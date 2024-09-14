using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundComponent : MonoBehaviour
{

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.maxDistance = 20f;
        _audioSource.minDistance = 1;

    }

    public void OnEnable()
    {
        OptionsMenu.onSoundEffectVolumeValueChanged += SetSoundEffectsSound;
    }

    public void OnDisable()
    {
        OptionsMenu.onSoundEffectVolumeValueChanged -= SetSoundEffectsSound;
    }

    public void PlaySound(AudioClip sound)
    {
        _audioSource.PlayOneShot(sound);
    }

    private void SetSoundEffectsSound(float value)
    {
        _audioSource.volume = value;
    }
}
