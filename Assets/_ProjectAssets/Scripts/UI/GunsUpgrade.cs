using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ConstantsValues;

public enum GunsUpgradeOptions
{
    ReloadSpeed,
    Damage,
    Magazine,
    Spread
}

public class GunsUpgrade : MonoBehaviour
{
    
    public static Action<UpgradeType> onGunsUpgradeSelected;
    
    [Header("Upgrades Parameters")]
    public float reloadSpeedAmountToDecrease;
    public float damageAmountToAdd;
    public int magazineAmountToAdd;
    public float spreadAmountToDecrease;
    
    [Header("Guns")]
    public Firearm pistol;
    public Firearm ak47;
    public Firearm shotgun;

    [Header("References")] 
    public Sprite defaultImage;
    public TextMeshProUGUI upgradeText;
    public Button upgradeButton;
    public Image mainImage;


    private List<GunsUpgradeOptions> gunsUpgradeOptions = new();
    private List<GunsType> weaponTypes = new();

    private float upgrade;
    private GunsUpgradeOptions randomUpgrade;

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
    
    
    public void Start()
    {
        // Add all guns upgrade type to the list
        gunsUpgradeOptions.Add(GunsUpgradeOptions.ReloadSpeed);
        gunsUpgradeOptions.Add(GunsUpgradeOptions.Damage);
        gunsUpgradeOptions.Add(GunsUpgradeOptions.Magazine);
        gunsUpgradeOptions.Add(GunsUpgradeOptions.Spread);
        
        // Add all weapon types to the list
        weaponTypes.Add(GunsType.Pistol);
        weaponTypes.Add(GunsType.AK);
        weaponTypes.Add(GunsType.Shotgun);
    }
    
    public void GetRandomWeaponUpgrade()
    {
        GunsType randomWeapon = weaponTypes[UnityEngine.Random.Range(0, weaponTypes.Count)];
        switch (randomWeapon)
        {
            case GunsType.Pistol:
                GetRandomUpgrade(pistol);
                break;
            case GunsType.AK:
                GetRandomUpgrade(ak47);
                break;
            case GunsType.Shotgun:
                GetRandomUpgrade(shotgun);
                break;
        }
    }

    private void GetRandomUpgrade(Firearm firearm)
    {
        randomUpgrade = gunsUpgradeOptions[UnityEngine.Random.Range(0, gunsUpgradeOptions.Count)];
        switch (randomUpgrade)
        {
            case GunsUpgradeOptions.ReloadSpeed:
                firearm.reloadTime -= reloadSpeedAmountToDecrease;
                upgrade = reloadSpeedAmountToDecrease;
                break;
            case GunsUpgradeOptions.Damage:
                firearm.damage += damageAmountToAdd;
                upgrade = damageAmountToAdd;
                break;
            case GunsUpgradeOptions.Magazine:
                firearm.magSize += magazineAmountToAdd;
                upgrade = magazineAmountToAdd;
                break;
            case GunsUpgradeOptions.Spread:
                firearm.spread -= spreadAmountToDecrease;
                upgrade = spreadAmountToDecrease;
                break;
        }
        
        onGunsUpgradeSelected?.Invoke(UpgradeType.Guns);
    }
    
    private void AddCardUpgradeAttribute()
    {
        mainImage.sprite = randomUpgrade switch
        {
            GunsUpgradeOptions.ReloadSpeed => pistol.weaponIcon,
            GunsUpgradeOptions.Damage => ak47.weaponIcon,
            GunsUpgradeOptions.Magazine => shotgun.weaponIcon,
            GunsUpgradeOptions.Spread => pistol.weaponIcon,
            _ => defaultImage
        };

        upgradeText.gameObject.SetActive(true);
        upgradeText.text = $"+ {upgrade} {randomUpgrade.ToString()}";
        upgradeButton.gameObject.SetActive(false);
    }


    private void SwitchCardToIdle()
    {
        upgradeText.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(true);
        mainImage.sprite = defaultImage;
    }
}
