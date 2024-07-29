using System;
using UnityEngine;
using UnityEngine.VFX;

public class Weapon : MonoBehaviour
{
    
    
    public float damage;
    public float reloadTime;
    public Constants.EnemyType enemyType;
    public Sprite weaponIcon;
    public VisualEffect vfx;
    protected float timeSinceLastShot;
    protected AnimationManager _animationManager;
    
    public float fireRate;

    public virtual bool  CanShoot()
    {
        throw new NotImplementedException();
    }
    
    public virtual void Shoot()
    {
        throw new NotImplementedException();
    }
    
    public virtual void Reload()
    {
        Debug.Log("Not implemented");
    }

    public virtual void SetArmHandler(AnimationManager arm)
    {
        _animationManager = arm;
    }
    
    public virtual void RefillAmmunition(int amount)
    {
        Debug.Log("Not implemented");
    }
}
