using System;
using System.Collections;
using System.Collections.Generic;
using ConstantsValues;
using UnityEngine;

public struct PlayerUpgradeObject
{
    public PlayerUpgradesOptions playerUpgradesOptions;
    public int upgradeValue;
}

public struct WeaponUpgradeObject
{
    public GunsType gunsType;
    public WeaponUpgradeOptions weaponUpgradeOptions;
    public int upgradeValue;
}



public class UpgradeInterpretor : MonoBehaviour
{
    public static Action<PlayerUpgradesOptions, int> onPlayerGetUpgrade;
    public static Action<WeaponUpgradeOptions, GunsType, int> onWeaponGetUpgrade;

    private Dictionary<int, PlayerUpgradeObject> _playerUpgrades = new Dictionary<int, PlayerUpgradeObject>();
    private Dictionary<int, WeaponUpgradeObject> _weaponUpgrades = new Dictionary<int, WeaponUpgradeObject>();

    private int order = 1;

    void OnEnable()
    {
        PlayerUpgradeController.onPlayerGetUpgrade += IntercepPlayerUpgrade;
        WeaponUpgradeController.onWeaponGetUpgrade += IntercepWeaponUpgrade;
        CardConstructor.onCardClicked += ApplyUpgrades;
    }

    void OnDisable()
    {
        PlayerUpgradeController.onPlayerGetUpgrade -= IntercepPlayerUpgrade;
        WeaponUpgradeController.onWeaponGetUpgrade -= IntercepWeaponUpgrade;
        CardConstructor.onCardClicked -= ApplyUpgrades;
    }

    public void IntercepWeaponUpgrade(WeaponUpgradeOptions weaponUpgradeOptions, GunsType gunsType, int amount)
    {
        WeaponUpgradeObject weaponUpgradeObject = new WeaponUpgradeObject();
        weaponUpgradeObject.gunsType = gunsType;
        weaponUpgradeObject.weaponUpgradeOptions = weaponUpgradeOptions;
        weaponUpgradeObject.upgradeValue = amount;

        _weaponUpgrades.Add(order, weaponUpgradeObject);
        order++;
    }

    public void IntercepPlayerUpgrade(PlayerUpgradesOptions playerUpgradesOptions, int amount)
    {
        PlayerUpgradeObject playerUpgradeObject = new PlayerUpgradeObject();
        playerUpgradeObject.playerUpgradesOptions = playerUpgradesOptions;
        playerUpgradeObject.upgradeValue = amount;

        _playerUpgrades.Add(order, playerUpgradeObject);
        order++;
    }

    public void ApplyUpgrades(int index)
    {
        if (_playerUpgrades.ContainsKey(index))
        {
            onPlayerGetUpgrade?.Invoke(_playerUpgrades[index].playerUpgradesOptions, _playerUpgrades[index].upgradeValue);
        }

        if (_weaponUpgrades.ContainsKey(index))
        {
            onWeaponGetUpgrade?.Invoke(_weaponUpgrades[index].weaponUpgradeOptions, _weaponUpgrades[index].gunsType, _weaponUpgrades[index].upgradeValue);
        }

        _playerUpgrades.Clear();
        _weaponUpgrades.Clear();
        order = 1;
    }

}
