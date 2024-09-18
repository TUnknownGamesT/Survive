
using System.Collections.Generic;
using ConstantsValues;
using SerializableDictionary.Scripts;
using UnityEngine;
using ConstValues = ConstantsValues;
public enum SoundEffects
{
    Death,
    Hit,

    GetDamage,
}

[System.Serializable]
public struct SFXCollection
{
    public string name;
    public ConstValues.EnemyType enemyType;
    public SerializableDictionary<SoundEffects, AudioClip> sfxDictionary;
}



public class SoundManager : MonoBehaviour
{

    #region Singleton
    public static SoundManager instance;
    private void Awake()
    {
        instance = FindObjectOfType<SoundManager>();

        if (instance == null)
        {
            instance = this;
        }

        _audioSource = GetComponent<AudioSource>();
    }

    #endregion

    public List<AudioClip> musicClips;
    public AudioClip clickButtonSound;
    public AudioClip playerUpgrade;
    public AudioClip baseUpgrade;
    public AudioClip gunsUpgrade;
    public AudioClip enemyUpgrade;

    [Header("Sound Effects")]

    public List<SFXCollection> sfxCollections;


    private static AudioSource _audioSource;



    private void OnEnable()
    {
        // GunsUpgrade.onGunsUpgradeSelected += ClickButtonSound;
        // PlayerUpgrades.onPlayerUpgradeSelected += ClickButtonSound;
        BaseUpgrade.onBaseUpgradeSelected += ClickButtonSound;
        OptionsMenu.onAmbientVolumeValueChanged += SetMusicVolume;
        UpgradePanelBehaviour.onUpgradeCardInFront += UpgradeSoundEffect;
    }

    private void OnDisable()
    {
        // GunsUpgrade.onGunsUpgradeSelected -= ClickButtonSound;
        // PlayerUpgrades.onPlayerUpgradeSelected -= ClickButtonSound;
        BaseUpgrade.onBaseUpgradeSelected -= ClickButtonSound;
        OptionsMenu.onAmbientVolumeValueChanged -= SetMusicVolume;
        UpgradePanelBehaviour.onUpgradeCardInFront -= UpgradeSoundEffect;

    }

    private void Start()
    {
        // _audioSource.clip = musicClips[Random.Range(0, musicClips.Count)];
        // _audioSource.Play();
    }

    private void UpgradeSoundEffect(UpgradeType obj)
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

    public void PlaySoundEffect(ConstValues.EnemyType typeOfMonster, SoundComponent soundComponent, SoundEffects soundEffect)
    {
        AudioClip audioClip = sfxCollections.Find(x => x.enemyType == typeOfMonster).sfxDictionary.Get(soundEffect);
        if (audioClip == null)
        {
            Debug.LogError("Sound Effect is null");
            return;
        }
        soundComponent.PlaySound(audioClip);
    }

    private void PlaySound(SoundComponent soundComponent, AudioClip audioClip)
    {
        soundComponent.PlaySound(audioClip);
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
