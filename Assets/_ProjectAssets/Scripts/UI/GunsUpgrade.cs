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
    
    public static Action onGunsUpgradeSelected;
    
    public float reloadSpeedAmountToDecrease;
    public float damageAmountToAdd;
    public int magazineAmountToAdd;
    public float spreadAmountToDecrease;

   

    public Gun pistol;
    public Gun ak47;
    public Gun shotgun;
    
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
        weaponTypes.Add(Constants.GunsType.AKA47);
        weaponTypes.Add(Constants.GunsType.ShotGun);
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
            case Constants.GunsType.AKA47:
                GetRandomUpgrade(ak47);
                gunImage.sprite = ak47.weaponIcon;
                break;
            case Constants.GunsType.ShotGun:
                GetRandomUpgrade(shotgun);
                gunImage.sprite = shotgun.weaponIcon;
                break;
        }
    }

    private void GetRandomUpgrade(Gun gun)
    {
        randomUpgrade = gunsUpgradeOptions[UnityEngine.Random.Range(0, gunsUpgradeOptions.Count)];
        switch (randomUpgrade)
        {
            case GunsUpgradeOptions.ReloadSpeed:
                gun.reloadTime -= reloadSpeedAmountToDecrease;
                upgrade = reloadSpeedAmountToDecrease;
                break;
            case GunsUpgradeOptions.Damage:
                gun.damage += damageAmountToAdd;
                upgrade = damageAmountToAdd;
                break;
            case GunsUpgradeOptions.Magazine:
                gun.magSize += magazineAmountToAdd;
                upgrade = magazineAmountToAdd;
                break;
            case GunsUpgradeOptions.Spread:
                gun.spread -= spreadAmountToDecrease;
                upgrade = spreadAmountToDecrease;
                break;
        }
        
        onGunsUpgradeSelected?.Invoke();
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
