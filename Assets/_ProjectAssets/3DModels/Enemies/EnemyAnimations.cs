using UnityEngine;

public class EnemyAnimations : AnimationManager
{

    public override void Attack()
    {
        _animator.SetTrigger("T_Attack");
    }

    public override void Die()
    {
        _animator.enabled = false;
    }

}


