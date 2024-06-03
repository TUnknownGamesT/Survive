using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpawner : MonoBehaviour
{
    public static Action onAllEnemiesDead;

    public GameObject spawnPointsParent;
    [Tooltip("The amount of enemies to spawn per round")]
    public int enemies;
    [Tooltip("The amount of time between each round")]
    public float pauseTime;

    private List<Transform> spawnPoints = new();
    private int enemySpawned = 0;

    private void Awake()
    {
        for (int i = 0; i < spawnPointsParent.transform.childCount; i++)
        {
            spawnPoints.Add(spawnPointsParent.transform.GetChild(i));
        }
    }

    private void OnEnable()
    {
        GunsUpgrade.onGunsUpgradeSelected += StartSpawning;
        PlayerUpgrades.onPlayerUpgradeSelected += StartSpawning;
        BaseUpgrade.onBaseUpgradeSelected += StartSpawning;
        AIBrain.onEnemyDeath += EnemyDied;
    }
    
    private void OnDisable()
    {
        GunsUpgrade.onGunsUpgradeSelected -= StartSpawning;
        PlayerUpgrades.onPlayerUpgradeSelected -= StartSpawning;
        AIBrain.onEnemyDeath -= EnemyDied;
        BaseUpgrade.onBaseUpgradeSelected -= StartSpawning;
    }

    private void Start()
    {
        StartSpawning();
    }


    private void StartSpawning()
    {
        UniTask.Void(async () =>
        {
            Debug.LogWarning("Spawning");
            await UniTask.Delay(TimeSpan.FromSeconds(pauseTime));
            enemySpawned = enemies;
            for (int i = 0; i < enemies; i++)
            {
                Debug.Log("Spawned");
                Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
                FactoryObjects.instance.CreateObject(new FactoryObject<Transform>(
                    FactoryObjectsType.Enemy,spawnPoint));
            }
        });
        
    }

    private void EnemyDied()
    {
        enemySpawned--;
        if (enemySpawned == 0)
        {
            onAllEnemiesDead?.Invoke();
        }
    }
    
    
    
}
