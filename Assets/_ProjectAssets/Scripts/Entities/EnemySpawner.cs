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
    [Tooltip("The amount of enemies to spawn per round")]
    public int enemies;
    [Tooltip("The amount of time between each round")]
    public float pauseTime;
    public int roundsToPass;
    public int enemiesPerRoundIncrease;

    private List<Transform> spawnPoints = new();
    private int enemySpawned = 0;
    private int roundPassed;

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
        Init();
        StartSpawning();
    }

    private void Init()
    {
        LvlSettings lvlSettings = GameManager.instance.LvlSettings;
        enemies = lvlSettings.enemiesToSpawn;
        pauseTime = lvlSettings.pauseBetweenRounds;
        roundsToPass = lvlSettings.roundsToPass;
        enemiesPerRoundIncrease = lvlSettings.enemiesPerRoundIncrease;
    }

    //Schimbat pe viitor sa nu mai fie nevoie de Upgrade Type
    private void StartSpawning()
    {
        Debug.Log("Start Spawning");
        roundPassed++;
        if (roundPassed == roundsToPass)
        {
            enemies += enemiesPerRoundIncrease;
            roundPassed = 0;
        }
        UniTask.Void(async () =>
        {
            onPauseStart?.Invoke(pauseTime);
            await UniTask.Delay(TimeSpan.FromSeconds(pauseTime));
            enemySpawned = enemies;
            for (int i = 0; i < enemies; i++)
            {
                Debug.Log("Spawned");
                Transform spawnPoint = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)];
                FactoryObjects.instance.CreateObject(new FactoryObject<Transform>(
                    FactoryObjectsType.Enemy, spawnPoint));
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
