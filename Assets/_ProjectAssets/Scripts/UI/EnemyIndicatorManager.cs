using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIndicatorManager : MonoBehaviour
{
    #region Singleton
    
    public static EnemyIndicatorManager instance;

    private void Awake()
    {
        instance = FindObjectOfType<EnemyIndicatorManager>();
        
        if(instance == null)
        {
            instance = this;
        }
    }

    #endregion
    
    public GameObject _enemyIndicatorPrefab;

    private void OnEnable()
    {
        FactoryObjects.onEnemySpawned += RegisterEnemy;
    }
    
    private void OnDisable()
    {
        FactoryObjects.onEnemySpawned -= RegisterEnemy;
    }

    private void RegisterEnemy(Transform enemy)
    {
        var indicator = Instantiate(_enemyIndicatorPrefab, transform);
        indicator.GetComponent<EnemyIndicator>().RegisterTarget(enemy);
    }
}
