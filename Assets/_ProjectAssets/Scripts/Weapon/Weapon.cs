using System;
using ConstantsValues;
using UnityEngine;
using UnityEngine.VFX;

public abstract class Weapon : MonoBehaviour
{


    public float damage;
    public float reloadTime;
    public Sprite weaponIcon;
    public VisualEffect vfx;
    protected float timeSinceLastShot;
    protected AnimationManager _animationManager;

    public float fireRate;

    public float maxSpreadAmount;

    public float spreadAmount;

    protected float recoilRecoverySpread = 2f;

    public virtual bool CanShoot()
    {
        throw new NotImplementedException();
    }

    public abstract void Tick(bool wantsToAttack);

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
