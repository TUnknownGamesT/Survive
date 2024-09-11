using System;
using System.Collections.Generic;
using ConstantsValues;
using Cysharp.Threading.Tasks;
using UnityEngine;


public class EnemySpawner : MonoBehaviour
{
    public static Action onAllEnemiesDead;
    public static Action<float> onPauseStart;

    public GameObject spawnPointsParent;
    private List<Transform> spawnPoints = new();
    private List<GameObject> enemiesToSpawn = new();
    private int enemySpawned = 0;
    private int roundPassed;

    private int roundsPassedUntilNextUpgrade;
    private int enemyToSpawnAdaosd;

    private void Awake()
    {
        for (int i = 0; i < spawnPointsParent.transform.childCount; i++)
        {
            spawnPoints.Add(spawnPointsParent.transform.GetChild(i));
        }
    }

    private void OnEnable()
    {
        EnemySpawner.onAllEnemiesDead += StartSpawning;
        AIBrain.onEnemyDeath += EnemyDied;
        AIGroupBrain.onEnemyDeath += EnemyDied;
    }

    private void OnDisable()
    {
        EnemySpawner.onAllEnemiesDead += StartSpawning;
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
            onPauseStart?.Invoke(GameManager.instance.LvlSettings.pauseBetweenRounds);
            await UniTask.Delay(TimeSpan.FromSeconds(GameManager.instance.LvlSettings.pauseBetweenRounds));
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
            onAllEnemiesDead?.Invoke();
        }
    }



}
