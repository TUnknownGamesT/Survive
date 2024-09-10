using UnityEngine;

public class EnemyAnimations : AnimationManager
{
    public override void SetWeaponType(int weaponType)
    {
        //_currentWeaponType = (WeaponTypesAnimated)weaponType;
        //_animator.SetInteger("I_weaponId", weaponType);
    }

    public override void Attack()
    {
        _animator.SetTrigger("T_Attack");
    }

    public override void SetSpeed(float speed)
    {
        //_animator.SetFloat("F_Speed", speed);
    }

    public override void Die()
    {
        _animator.enabled = false;
        //ragDollController.ActivateRagdoll();
        Debug.Log("Enemy died");
    }


}


