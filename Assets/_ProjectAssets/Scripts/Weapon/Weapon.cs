using System;
using UnityEngine;
using UnityEngine.VFX;

public class Weapon : MonoBehaviour
{
    
    public Sprite weaponIcon;
    public VisualEffect vfx;
    protected float timeSinceLastShot;
    
    public float fireRate;

    public virtual bool  CanShoot()
    {
        throw new NotImplementedException();
    }
    
    public virtual void Shoot()
    {
        throw new NotImplementedException();
    }

    public virtual void SetArmHandler(AnimationManager arm)
    {
        throw new NotImplementedException();
    }
}
