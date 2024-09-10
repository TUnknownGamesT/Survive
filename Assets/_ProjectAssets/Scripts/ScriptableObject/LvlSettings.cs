using UnityEngine;



[System.Serializable]
public struct EnemiPair
{
    public GameObject enemyToSpawn;
    public int roundStartSpawning;
}

[CreateAssetMenu(fileName = "LvlSettings", menuName = "ScriptableObjects/LvlSettings", order = 3)]
public class LvlSettings : ScriptableObject
{
    public EnemiPair[] enemies;
    public float pauseBetweenRounds;
    public int enemiesToSpawn;
    public int enemiesPerRoundIncrease;
    public int enemyToSpawnIncreaseRate;
    public float pauseBetweenSpawns;
}
