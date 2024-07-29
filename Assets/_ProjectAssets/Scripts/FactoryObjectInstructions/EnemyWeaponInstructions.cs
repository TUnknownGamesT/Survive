using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponInstructions
{
    public Constants.EnemyType enemyType;
    public Transform parent;
    
    public EnemyWeaponInstructions(Constants.EnemyType enemyType, Transform parent)
    {
        this.enemyType = enemyType;
        this.parent = parent;
    }
}
