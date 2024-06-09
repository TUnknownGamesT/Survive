using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInitiator : MonoBehaviour
{

    #region Singleton

    public static EnemyInitiator instance;

    private void Awake()
    {
        instance = FindObjectOfType<EnemyInitiator>();
        if (instance == null)
        {
            instance = this;
        }
    }

    #endregion

    public List<EnemyType> enemyTypes;
    public List<Gun> enemyGuns;
    
    private List<EnemyType> mockUpEnemys;


    private void OnEnable()
    {
        EnemyUpgrades.onEnemyUpgraded += UpgradeEnemy;
    }

    private void OnDisable()
    {
        EnemyUpgrades.onEnemyUpgraded -= UpgradeEnemy;
    }

    private void Start()
    {
        mockUpEnemys = new List<EnemyType>(enemyTypes);
    }

    public EnemyType GetEnemyStats(Constants.EnemyType enemy)
    {
        return mockUpEnemys.Find(x => x.enemyType == enemy);
    }


    private void UpgradeEnemy(EnemyUpgradesOptions upgrade,float amount)
    {

        switch (upgrade)
        {
            case EnemyUpgradesOptions.Health:
                foreach (var enemy in mockUpEnemys)
                {
                    enemy.health += (int) amount;
                }
                break;
            case EnemyUpgradesOptions.Speed:
                foreach (var enemy in mockUpEnemys)
                {
                    enemy.speed += amount;
                }
                break;
            case EnemyUpgradesOptions.Damage:
                foreach (var gun in enemyGuns)
                {
                    gun.damage += amount;
                }
                break;
        }
     
    }
}
