using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

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
    
    public float reloadSpeedAmountToDecrease;
    public float damageAmountToAdd;
    public int magazineAmountToAdd;
    public float spreadAmountToDecrease;

   

    public Firearm pistol;
    public Firearm ak47;
    public Firearm shotgun;
    
    public TextMeshProUGUI upgradeText;
    public Button upgradeButton;
    public TextMeshProUGUI title;
    public Image gunImage;
    public SoundComponent soundComponent;
    public AudioClip upgradeSound;


    private List<GunsUpgradeOptions> gunsUpgradeOptions = new();
    private List<Constants.GunsType> weaponTypes = new();

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
        weaponTypes.Add(Constants.GunsType.Pistol);
        weaponTypes.Add(Constants.GunsType.AK);
        weaponTypes.Add(Constants.GunsType.Shotgun);
    }
    
    public void GetRandomWeaponUpgrade()
    {
        Constants.GunsType randomWeapon = weaponTypes[UnityEngine.Random.Range(0, weaponTypes.Count)];
        soundComponent.PlaySound(upgradeSound);
        switch (randomWeapon)
        {
            case Constants.GunsType.Pistol:
                GetRandomUpgrade(pistol);
                gunImage.sprite = pistol.weaponIcon;
                break;
            case Constants.GunsType.AK:
                GetRandomUpgrade(ak47);
                gunImage.sprite = ak47.weaponIcon;
                break;
            case Constants.GunsType.Shotgun:
                GetRandomUpgrade(shotgun);
                gunImage.sprite = shotgun.weaponIcon;
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
        upgradeText.gameObject.SetActive(true);
        upgradeText.text = $"+ {upgrade} {randomUpgrade.ToString()}";
        upgradeButton.gameObject.SetActive(false);
        gunImage.gameObject.SetActive(true);
        title.gameObject.SetActive(false);
    }


    private void SwitchCardToIdle()
    {
        upgradeText.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(true);
        gunImage.gameObject.SetActive(false);
        title.gameObject.SetActive(true);
    }
}
