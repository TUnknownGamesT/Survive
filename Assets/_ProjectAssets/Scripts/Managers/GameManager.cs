using System;
using UnityEngine;

[RequireComponent(typeof(ScenesManager))]
public class GameManager : MonoBehaviour
{
    public static Action onGameEnd;
    
    public static Transform playerRef;
    public static Transform crossHairRef;
    public static Transform playerBaseRef;
    

    //Enemy Spawner
    private EnemySpawner enemySpawner;
    

    private void Awake()
    {   
        enemySpawner = GetComponent<EnemySpawner>();
        
        
        Cursor.visible= false;
        playerRef = GameObject.FindGameObjectWithTag("Player").transform;
        crossHairRef = GameObject.FindGameObjectWithTag("CrossHair").transform;
        playerBaseRef = GameObject.FindGameObjectWithTag("PlayerBase").transform;
    }

    private void OnEnable()
    {
        BaseBehaviour.onBaseDestroyed += EndGame;
        PlayerHealth.onPlayerDeath += EndGame;
    }
    
    private void OnDisable()
    {
        BaseBehaviour.onBaseDestroyed -= EndGame;
        PlayerHealth.onPlayerDeath -= EndGame;
    }

    private void Start()
    {
        Time.timeScale = 0;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
    }

    private void EndGame()
    {
        //Time.timeScale = 0;
        onGameEnd?.Invoke();
    }
    
    public void RestartGame()
    {
        ScenesManager.ReloadCurrentScene();
    }
    
}
