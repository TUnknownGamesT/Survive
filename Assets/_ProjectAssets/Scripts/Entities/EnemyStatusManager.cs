using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusManager : MonoBehaviour
{
    
    public static Action<float>  onEnemyHealthChanged;
    public static Action<float>  onEnemySpeedChanged;
    public static Action<float>  onEnemyDamageChanged;
    
    private void OnEnable()
    {
        EnemyUpgrades.onEnemyUpgraded += UpgradeEnemy;
    }

    private void OnDisable()
    {
        EnemyUpgrades.onEnemyUpgraded -= UpgradeEnemy;
    }
    
    private void UpgradeEnemy(EnemyUpgradesOptions upgrade,float amount)
    {

        switch (upgrade)
        {
            case EnemyUpgradesOptions.Health:
                onEnemyHealthChanged?.Invoke(amount);
                break;
            case EnemyUpgradesOptions.Speed:
                onEnemySpeedChanged?.Invoke(amount);
                break;
            case EnemyUpgradesOptions.Damage:
                onEnemyDamageChanged?.Invoke(amount);
                break;
        }
     
    }
}
