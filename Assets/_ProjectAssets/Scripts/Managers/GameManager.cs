using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    public void StartGame()
    {
        
    }
    
    
}
