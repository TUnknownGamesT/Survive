using System.Collections;
using System.Collections.Generic;
using ConstantsValues;
using UnityEngine;

public class EnemyWeaponInstructions
{
    public ConstantsValues.EnemyType enemyType;
    public Transform parent;
    
    public EnemyWeaponInstructions(ConstantsValues.EnemyType enemyType, Transform parent)
    {
        this.enemyType = enemyType;
        this.parent = parent;
    }
}
