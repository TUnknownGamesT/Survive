using System;
using System.Collections.Generic;
using ConstantsValues;
using Cysharp.Threading.Tasks;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    public GameObject spawnPointsParent;
    private List<Transform> spawnPoints = new();
    private List<GameObject> enemiesToSpawn = new();
    public static int roundPassed;

    private static int roundsPassedUntilNextUpgrade;
    private static int enemyToSpawnAdaosd;
    private static int enemySpawned = 0;

    private void Awake()
    {
        for (int i = 0; i < spawnPointsParent.transform.childCount; i++)
        {
            spawnPoints.Add(spawnPointsParent.transform.GetChild(i));
        }
    }

    private void OnEnable()
    {
        GameManager.onPuaseEnd += StartSpawning;
        AIBrain.onEnemyDeath += EnemyDied;
        AIGroupBrain.onEnemyDeath += EnemyDied;
    }

    private void OnDisable()
    {
        GameManager.onPuaseEnd -= StartSpawning;
        AIBrain.onEnemyDeath -= EnemyDied;
        AIGroupBrain.onEnemyDeath -= EnemyDied;
    }

    private void Start()
    {
        InitEnemiToSpawn();
        StartSpawning();
    }

    private void InitEnemiToSpawn()
    {
        enemiesToSpawn.Clear();
        foreach (var enemy in GameManager.instance.LvlSettings.enemies)
        {
            if (enemy.roundStartSpawning <= roundPassed)
            {
                enemiesToSpawn.Add(enemy.enemyToSpawn);
            }
        }
    }


    //Schimbat pe viitor sa nu mai fie nevoie de Upgrade Type
    private void StartSpawning()
    {
        roundPassed++;
        roundsPassedUntilNextUpgrade++;
        if (roundsPassedUntilNextUpgrade >= GameManager.instance.LvlSettings.enemiesPerRoundIncrease)
        {
            roundsPassedUntilNextUpgrade = 0;
            enemyToSpawnAdaosd += GameManager.instance.LvlSettings.enemyToSpawnIncreaseRate;
        }

        UniTask.Void(async () =>
        {
            enemySpawned = GameManager.instance.LvlSettings.enemiesToSpawn;
            for (int i = 0; i < enemySpawned + enemyToSpawnAdaosd; i++)
            {
                Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
                FactoryObjects.instance.CreateObject(new FactoryObject<EnemyInstructions>(FactoryObjectsType.Enemy
                , new EnemyInstructions(spawnPoint.position, enemiesToSpawn[UnityEngine.Random.Range(0, enemiesToSpawn.Count)])));
                await UniTask.Delay(TimeSpan.FromSeconds(GameManager.instance.LvlSettings.pauseBetweenSpawns));
            }
        });

    }

    private void EnemyDied()
    {
        enemySpawned--;
        if (enemySpawned == 0)
        {
            GameManager.instance.NextWave();
            InitEnemiToSpawn();
        }
    }

    public static void ResetForNextLvl()
    {
        roundPassed = 0;
        roundsPassedUntilNextUpgrade = 0;
        enemyToSpawnAdaosd = 0;
    }



}
