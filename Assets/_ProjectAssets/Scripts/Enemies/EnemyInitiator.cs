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
        
        mockUpEnemys = new List<EnemyType>(enemyTypes);
    }

    #endregion

    public List<EnemyType> enemyTypes;

    private List<EnemyType> mockUpEnemys;


    private void OnEnable()
    {
        EnemyStatusManager.onEnemyHealthChanged += UpgradeHealth;
        EnemyStatusManager.onEnemySpeedChanged += UpgradeSpeed;
    }
    
    private void OnDisable()
    {
        EnemyStatusManager.onEnemyHealthChanged -= UpgradeHealth;
        EnemyStatusManager.onEnemySpeedChanged -= UpgradeSpeed;
    }


    public EnemyType GetEnemyStats(Constants.EnemyType enemy)
    {
        return mockUpEnemys.Find(x => x.enemyType == enemy);
    }

    private void UpgradeHealth(float amount)
    {
        foreach (var enemy in mockUpEnemys)
        {
            enemy.health += (int) amount;
        }
    }
    
    private void UpgradeSpeed(float amount)
    {
        foreach (var enemy in mockUpEnemys)
        {
            enemy.speed += amount;
        }
    }
    


    
}
