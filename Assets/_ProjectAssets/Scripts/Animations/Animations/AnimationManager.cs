using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public virtual void SetWeaponType(int weaponType)
    {
        _currentWeaponType = (WeaponTypesAnimated)weaponType;
        _animator.SetInteger("I_weaponId", weaponType);
    }
    public virtual void Attack()
    {
        _animator.SetTrigger("T_Attack");
    }

    public void Reload()
    {
        _animator.SetTrigger("T_Reload");
    }

    public virtual void SetSpeed(float speed)
    {
        _animator.SetFloat("F_Speed", speed);
    }

    public virtual void Die()
    {
        ragDollController.ActivateRagdoll();
    }

    public void SetIsWalking(bool isWalking)
    {
        _animator.SetBool("T_Walking", isWalking);
    }

    public void SetIdle(bool idleStatus)
    {
        _animator.SetBool("T_Idle", idleStatus);
    }

    public void ResetTriger(string triggerName)
    {
        _animator.ResetTrigger(triggerName);
    }

}
