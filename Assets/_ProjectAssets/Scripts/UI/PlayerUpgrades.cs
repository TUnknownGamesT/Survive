using System;
using System.Collections.Generic;
using ConstantsValues;
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
    public static Action<UpgradeType> onPlayerUpgradeSelected;

    [Header("Player Upgrades Parameters")]
    public float speedAmountToAdd;
    public int lifeAmountToAdd;

    [Header("References")] 
    public Image mainImage;
    public TextMeshProUGUI upgradeText;
    public Button upgradeButton;


    [Header("Player Upgrades Options Image")]
    public  Sprite healthImage;
    public  Sprite speedImage;
    public Sprite defaultImage;
    
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
        
        onPlayerUpgradeSelected?.Invoke(UpgradeType.Player);
    }

    private void AddCardUpgradeAttribute()
    {
        upgradeText.gameObject.SetActive(true);
        upgradeText.text = $"+ {_upgradedStatus} {_randomUpgrade.ToString()}";
        upgradeButton.gameObject.SetActive(false);
        
        mainImage.sprite = _randomUpgrade switch
        {
            PlayerUpgradesOptions.Speed => speedImage,
            PlayerUpgradesOptions.Life => healthImage,
            _ => defaultImage
        };
        mainImage.SetNativeSize();
    }


    private void SwitchCardToIdle()
    {
        upgradeText.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(true);
        mainImage.sprite = defaultImage;
        mainImage.SetNativeSize();
    }
}
