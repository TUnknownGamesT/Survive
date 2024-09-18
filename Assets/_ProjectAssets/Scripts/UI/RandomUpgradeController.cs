using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ConstantsValues;

public class RandomUpgradeController : MonoBehaviour
{


    public List<CardConstructor> cards;

    private List<UpgradesType> _upgradeType = new List<UpgradesType>();

    private List<CardClass> _cardClasses = new List<CardClass>();

    private PlayerUpgradeController _playerUpgradeController;

    private WeaponUpgradeController _weaponUpgradeController;


    void OnEnable()
    {
        PlayerXP.onPlayerLevelUp += RandomUpgrade;
    }

    void OnDisable()
    {
        PlayerXP.onPlayerLevelUp -= RandomUpgrade;
    }

    void Start()
    {
        _upgradeType.Add(UpgradesType.Player);
        _upgradeType.Add(UpgradesType.Guns);

        _cardClasses.Add(CardClass.COMMON);
        _cardClasses.Add(CardClass.RARE);
        _cardClasses.Add(CardClass.EPIC);

        _playerUpgradeController = GetComponent<PlayerUpgradeController>();
        _weaponUpgradeController = GetComponent<WeaponUpgradeController>();
    }

    [ContextMenu("RandomUpgrade")]
    public void RandomUpgrade()
    {

        for (int i = 0; i < cards.Count; i++)
        {
            UpgradesType randomUpgrade = _upgradeType[Random.Range(0, _upgradeType.Count)];
            CardClass randomCardClass = _cardClasses[Random.Range(0, _cardClasses.Count)];

            if (randomUpgrade == UpgradesType.Player)
            {
                _playerUpgradeController.Upgrade(randomCardClass, cards[i]);
            }
            else
            {
                _weaponUpgradeController.Upgrade(randomCardClass, cards[i]);
            }

        }
    }

}
