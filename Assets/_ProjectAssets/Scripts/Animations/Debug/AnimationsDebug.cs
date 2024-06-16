using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class AnimationsDebug : MonoBehaviour
{
    public AnimationManager animationsManager;
    public int weaponType = 1;
    public float speed = 1;
    
    [Button("Attack")]
    public void Attack()
    {
        animationsManager.Attack();
    }
    
    [Button("Reload")]
    public void Reload()
    {
        animationsManager.Reload();
    }
    
    [Button("Set Weapon Type")]
    public void SetWeaponType()
    {
        animationsManager.SetWeaponType(weaponType);
    }
    
    [Button("SetSpeed")]
    public void SetSpeed()
    {
        animationsManager.SetSpeed(speed);
    }
}
