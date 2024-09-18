using System.Collections.Generic;
using ConstantsValues;
using UnityEngine;

public class PlayerGunsController : MonoBehaviour
{
    public List<Firearm> firearms;


    void OnEnable()
    {
        UpgradeInterpretor.onWeaponGetUpgrade += UpgradeFirearms;
    }

    void OnDisable()
    {
        UpgradeInterpretor.onWeaponGetUpgrade -= UpgradeFirearms;
    }

    private void UpgradeFirearms(WeaponUpgradeOptions weaponUpgradeOptions, GunsType gunsType, int amount)
    {
        switch (gunsType)
        {
            case GunsType.Pistol:
                ApplyUpgrade(weaponUpgradeOptions, amount, firearms.Find(x => x.gunsType == GunsType.Pistol));
                break;
            case GunsType.Shotgun:
                ApplyUpgrade(weaponUpgradeOptions, amount, firearms.Find(x => x.gunsType == GunsType.Shotgun));
                break;
            case GunsType.AK:
                ApplyUpgrade(weaponUpgradeOptions, amount, firearms.Find(x => x.gunsType == GunsType.AK));
                break;
        }

    }



    private void ApplyUpgrade(WeaponUpgradeOptions weaponUpgradeOptions, int amount, Firearm firearm)
    {
        switch (weaponUpgradeOptions)
        {
            case WeaponUpgradeOptions.Damage:
                Debug.Log(firearm.damage += CustomMath.GetPercentage(firearm.damage, amount));
                firearm.damage += CustomMath.GetPercentage(firearm.damage, amount);
                break;
            case WeaponUpgradeOptions.FireRate:
                Debug.Log(firearm.fireRate += CustomMath.GetPercentage(firearm.fireRate, amount));
                firearm.fireRate += CustomMath.GetPercentage(firearm.fireRate, amount);
                break;
            case WeaponUpgradeOptions.Spread:
                Debug.Log(firearm.maxSpreadAmount -= CustomMath.GetPercentage(firearm.maxSpreadAmount, amount));
                firearm.maxSpreadAmount -= CustomMath.GetPercentage(firearm.maxSpreadAmount, amount);
                break;
        }
    }



}
