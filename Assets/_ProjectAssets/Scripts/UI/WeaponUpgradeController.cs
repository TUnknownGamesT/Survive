using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstantsValues;
using SerializableDictionary.Scripts;
using System;
using Random = UnityEngine.Random;


[System.Serializable]
public struct WeaponUpgradesValues
{
    public string name;
    public WeaponUpgradeOptions enemyType;
    public string description;
    public string shortDescription;

    [HideInInspector]
    public Texture2D image;

    public SerializableDictionary<CardClass, Vector2> range;
}

public class WeaponUpgradeController : MonoBehaviour
{
    public static Action<WeaponUpgradeOptions, GunsType, int> onWeaponGetUpgrade;

    public List<WeaponUpgradesValues> weaponUpgradesValues;


    private List<WeaponUpgradeOptions> weaponUpgradeOptions = new List<WeaponUpgradeOptions>();

    private List<GunsType> gunsTypes = new List<GunsType>();

    void Start()
    {
        weaponUpgradeOptions.Add(WeaponUpgradeOptions.Damage);
        weaponUpgradeOptions.Add(WeaponUpgradeOptions.FireRate);
        weaponUpgradeOptions.Add(WeaponUpgradeOptions.Spread);

        gunsTypes.Add(GunsType.Pistol);
        gunsTypes.Add(GunsType.Shotgun);
        gunsTypes.Add(GunsType.AK);
    }

    public void Upgrade(CardClass upgradeTier, CardConstructor card)
    {
        WeaponUpgradeOptions randomUpgrade = weaponUpgradeOptions[Random.Range(0, weaponUpgradeOptions.Count)];

        GunsType randomGun = gunsTypes[Random.Range(0, gunsTypes.Count)];

        WeaponUpgradesValues stats = weaponUpgradesValues.Find(x => x.enemyType == randomUpgrade);

        Vector2 range = stats.range.Get(upgradeTier);

        int upgradeValue = Random.Range((int)range.x, (int)range.y);

        onWeaponGetUpgrade?.Invoke(randomUpgrade, randomGun, upgradeValue);

        ConstructCard(upgradeTier, card, stats, upgradeValue, randomUpgrade, randomGun);

    }

    public void ConstructCard(CardClass upgradeTier, CardConstructor card, WeaponUpgradesValues stats, int upgradeValue, WeaponUpgradeOptions randomUpgrade, GunsType randomGun)
    {
        UpgradeObject upgradeObject;
        if (randomUpgrade != WeaponUpgradeOptions.Spread)
        {
            upgradeObject = new UpgradeObject(upgradeTier, SelectImage(randomGun), "+" + upgradeValue.ToString(), stats.description, stats.shortDescription);
        }
        else
        {
            upgradeObject = new UpgradeObject(upgradeTier, SelectImage(randomGun), upgradeValue.ToString(), stats.description, stats.shortDescription);
        }



        card.ConstructCard(upgradeObject);
    }


    private Texture2D SelectImage(GunsType gunsType)
    {
        switch (gunsType)
        {
            case GunsType.Pistol:
                return Constants.instance.pistolIcon;
            case GunsType.AK:
                return Constants.instance.akIcon;
            case GunsType.Shotgun:
                return Constants.instance.shotgunIcon;
        }
        return null;
    }

}
