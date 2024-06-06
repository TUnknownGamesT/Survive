using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum PlayerUpgradesOptions
{
    Speed,
    Life
}

public class PlayerUpgrades : MonoBehaviour
{
    public static Action<int> onHealthUpgrade;
    public static Action<float> onSpeedUpgrade;
    public static Action onPlayerUpgradeSelected;


    public float speedAmountToAdd;
    public int lifeAmountToAdd;
    public float backpackAmountToAdd;
    public TextMeshProUGUI upgradeText;
    public Button upgradeButton;
    public AudioClip upgradeSound;
    public SoundComponent soundComponent;
  
    
    private readonly List<PlayerUpgradesOptions> _playerUpgradesOptions = new ();
    private float _upgradedStatus;
    private PlayerUpgradesOptions _randomUpgrade;

    private void Awake()
    {
        _playerUpgradesOptions.Add(PlayerUpgradesOptions.Speed);
        _playerUpgradesOptions.Add(PlayerUpgradesOptions.Life);
    }


    private void OnEnable()
    {
        UpgradePanelBehaviour.onSecondaryCardDisappear += AddCardUpgradeAttribute;
        UpgradePanelBehaviour.onPanelDisappear += SwitchCardToIdle;
    }
    
    private void OnDisable()
    {
        UpgradePanelBehaviour.onSecondaryCardDisappear -= AddCardUpgradeAttribute;
        UpgradePanelBehaviour.onPanelDisappear -= SwitchCardToIdle;
    }

    public void GetRandomUpgrade()
    {
        _randomUpgrade = _playerUpgradesOptions[UnityEngine.Random.Range(0, _playerUpgradesOptions.Count)];
        soundComponent.PlaySound(upgradeSound);
        switch (_randomUpgrade)
        {
            case PlayerUpgradesOptions.Speed:
                onSpeedUpgrade?.Invoke(speedAmountToAdd);
                _upgradedStatus = speedAmountToAdd;
                break;
            case PlayerUpgradesOptions.Life:
                onHealthUpgrade?.Invoke(lifeAmountToAdd);
                _upgradedStatus = lifeAmountToAdd;
                break;
        }
        
        onPlayerUpgradeSelected?.Invoke();
    }

    private void AddCardUpgradeAttribute()
    {
        upgradeText.gameObject.SetActive(true);
        upgradeText.text = $"+ {_upgradedStatus} {_randomUpgrade.ToString()}";
        upgradeButton.gameObject.SetActive(false);
    }


    private void SwitchCardToIdle()
    {
        upgradeText.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(true);
    }
}
