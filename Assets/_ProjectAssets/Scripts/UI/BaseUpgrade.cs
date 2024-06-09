using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum BaseUpgradesOptions
{
    Wall,
    Amo,
    MadKit
}

public class BaseUpgrade : MonoBehaviour
{

    public static Action<BaseUpgradesOptions,float> onBaseUpgraded;
    public static Action<UpgradeType> onBaseUpgradeSelected;

    
    [Header("References")]
    public TextMeshProUGUI upgradeText;
    public Image upgradeImage;
    public Button upgradeButton;
    public AudioClip upgradeSound;
    public SoundComponent soundComponent;
    public GameObject title;

    [Header("Items Images")]
    public Sprite wallImage;
    public Sprite amoImage;
    public Sprite madKitImage;
    
    [Header("Upgrades")]
    public float wallUpgrade;
    public float amoUpgrade;
    public float madKitUpgrade;

    private readonly List<BaseUpgradesOptions> _baseUpgradesOptions = new ();
    private float _upgradedStatus;
    private BaseUpgradesOptions _randomUpgrade;
    
    private void Awake()
    {
        _baseUpgradesOptions.Add(BaseUpgradesOptions.Wall);
        _baseUpgradesOptions.Add(BaseUpgradesOptions.Amo);
        _baseUpgradesOptions.Add(BaseUpgradesOptions.MadKit);
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
        _randomUpgrade = _baseUpgradesOptions[UnityEngine.Random.Range(0, _baseUpgradesOptions.Count)];
        soundComponent.PlaySound(upgradeSound);
        switch (_randomUpgrade)
        {
            case BaseUpgradesOptions.Wall:
                onBaseUpgraded?.Invoke(BaseUpgradesOptions.Wall,wallUpgrade);
                _upgradedStatus = wallUpgrade;
                break;
            case BaseUpgradesOptions.Amo:
                onBaseUpgraded?.Invoke(BaseUpgradesOptions.Amo ,amoUpgrade);
                _upgradedStatus = amoUpgrade;
                break;
            case BaseUpgradesOptions.MadKit:
                onBaseUpgraded?.Invoke(BaseUpgradesOptions.MadKit ,madKitUpgrade);
                _upgradedStatus = madKitUpgrade;
                break;
        }
        
        onBaseUpgradeSelected?.Invoke(UpgradeType.Base);
    }

    private void AddCardUpgradeAttribute()
    {
        upgradeText.gameObject.SetActive(true);
        upgradeText.text = $"+ {_upgradedStatus} {_randomUpgrade.ToString()}";
        upgradeButton.gameObject.SetActive(false);
        upgradeImage.gameObject.SetActive(true);
        upgradeImage.sprite = _randomUpgrade switch
        {
            BaseUpgradesOptions.Wall => wallImage,
            BaseUpgradesOptions.Amo => amoImage,
            BaseUpgradesOptions.MadKit => madKitImage,
            _ => throw new ArgumentOutOfRangeException()
        };
        title.gameObject.SetActive(false);
    }
    
    private void SwitchCardToIdle()
    {
        upgradeText.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(true);
        upgradeImage.gameObject.SetActive(false);
        title.gameObject.SetActive(true);
    }

   
}
