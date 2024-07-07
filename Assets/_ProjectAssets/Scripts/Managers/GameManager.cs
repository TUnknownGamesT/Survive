using System;
using UnityEngine;

[RequireComponent(typeof(ScenesManager))]
public class GameManager : MonoBehaviour
{

    #region Singleton 

    public static GameManager instance;
    
    private void Awake()
    {   
        instance = FindObjectOfType<GameManager>();
        if(instance == null)
        {
            instance = this;
        }
        
        enemySpawner = GetComponent<EnemySpawner>();
        
        
        Cursor.visible= false;
        playerRef = GameObject.FindGameObjectWithTag("Player").transform;
        crossHairRef = GameObject.FindGameObjectWithTag("CrossHair").transform;
        playerBaseRef = GameObject.FindGameObjectWithTag("PlayerBase").transform;
    }

    #endregion
    
    public static Action onGameEnd;
    
    public static Transform playerRef;
    public static Transform crossHairRef;
    public static Transform playerBaseRef;
    [SerializeField]
    private LvlSettings lvlSettings;
    
    public LvlSettings LvlSettings => lvlSettings;
    
    //Enemy Spawner
    private EnemySpawner enemySpawner;
    
    
    

    

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
