using System;
using System.Collections.Generic;
using ConstantsValues;
using Cysharp.Threading.Tasks;
using SerializableDictionary.Scripts;
using UnityEngine;
using UnityEngine.UI;



[System.Serializable]
public struct PlayerUpgradesValues
{
    public string name;
    public PlayerUpgradesOptions enemyType;
    public string description;
    public string shortDescription;
    public Texture2D image;
    public SerializableDictionary<CardClass, Vector2> range;
}

public class PlayerUpgradeController : MonoBehaviour
{
    public static Action<PlayerUpgradesOptions, int> onPlayerGetUpgrade;

    public List<PlayerUpgradesValues> playerUpgrades;

    private List<PlayerUpgradesOptions> playerUpgradesOptions = new List<PlayerUpgradesOptions>();

    void Start()
    {
        playerUpgradesOptions.Add(PlayerUpgradesOptions.Life);
        playerUpgradesOptions.Add(PlayerUpgradesOptions.Speed);
        playerUpgradesOptions.Add(PlayerUpgradesOptions.Shield);

    }

    public void Upgrade(CardClass upgradeTier, CardConstructor card)
    {
        PlayerUpgradesOptions randomUpgrade = playerUpgradesOptions[UnityEngine.Random.Range(0, playerUpgradesOptions.Count)];

        PlayerUpgradesValues stats = playerUpgrades.Find(x => x.enemyType == randomUpgrade);

        Vector2 range = stats.range.Get(upgradeTier);

        int upgradeValue = UnityEngine.Random.Range((int)range.x, (int)range.y);

        onPlayerGetUpgrade?.Invoke(randomUpgrade, upgradeValue);

        ConstructCard(upgradeTier, card, stats, upgradeValue);

    }

    private void ConstructCard(CardClass upgradeTier, CardConstructor card, PlayerUpgradesValues stats, int upgradeValue)
    {

        UpgradeObject upgradeObject = new UpgradeObject(upgradeTier, stats.image, "+" + upgradeValue.ToString(), stats.description, stats.shortDescription);

        card.ConstructCard(upgradeObject);
    }

}
