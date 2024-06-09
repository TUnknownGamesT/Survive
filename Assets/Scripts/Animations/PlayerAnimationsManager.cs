using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAnimationsManager : AnimationManager
{
    [SerializeField]
    private Animator _animator;

    public override void Attack()
    {
        _animator.Play(_animator.GetLayerName(0)+"."+_currentWeaponType.ToString()+".Shoot");
    }
}
