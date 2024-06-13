using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum WeaponTypesAnimated
{
    Melee,
    Pistol,
    Ak,
    Shotgun,
}
public abstract class AnimationManager : MonoBehaviour
{
    [SerializeField]
    protected Animator _animator;
    protected WeaponTypesAnimated _currentWeaponType = WeaponTypesAnimated.Melee;
    public RagDollController ragDollController;
    public void SetWeaponType(int weaponType)
    {
        _currentWeaponType = (WeaponTypesAnimated) weaponType;
        _animator.SetInteger("I_weaponId",  weaponType);
    }
    public virtual void Attack()
    {
        _animator.SetTrigger("T_Attack");
    }
    
    public void Reload()
    {
        _animator.SetTrigger("T_Reload");
    }
    
    public void SetSpeed(float speed)
    {
        _animator.SetFloat("F_Speed", speed);
    }

    public void Die()
    {
        ragDollController.ActivateRagdoll();
    }
    
}
