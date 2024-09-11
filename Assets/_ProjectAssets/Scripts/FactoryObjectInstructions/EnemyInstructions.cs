using UnityEngine;

public class EnemyInstructions
{
    public Vector3 Position { get; }

    public GameObject enemyPrefab;

    public EnemyInstructions(Vector3 position, GameObject enemyPrefab)
    {
        Position = position;
        this.enemyPrefab = enemyPrefab;
    }
}
