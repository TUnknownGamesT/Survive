using System.Collections.Generic;
using UnityEngine;

public class EnemyInstructions
{
    public Vector3 Position { get; }
    public Constants.EnemyType Type { get; }
    public List<Transform> travelPoints;

    public EnemyInstructions(Vector3 position, Constants.EnemyType enemyType, List<Transform> travelPoints)
    {
        Position = position;
        Type = enemyType;
        this.travelPoints = travelPoints;
    }
}
