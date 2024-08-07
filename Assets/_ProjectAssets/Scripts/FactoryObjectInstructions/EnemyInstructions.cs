using System.Collections.Generic;
using ConstantsValues;
using UnityEngine;

public class EnemyInstructions
{
    public Vector3 Position { get; }
    public ConstantsValues.EnemyType Type { get; }
    public List<Transform> travelPoints;

    public EnemyInstructions(Vector3 position, ConstantsValues.EnemyType enemyType, List<Transform> travelPoints)
    {
        Position = position;
        Type = enemyType;
        this.travelPoints = travelPoints;
    }
}
