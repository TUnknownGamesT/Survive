using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class AnimationsDebug : MonoBehaviour
{
    [SerializeField]
    private TestAnimations animations;

    [SerializeField]
    private int weaponID;

    [SerializeField]
    [Range(-5f, 5f)]
    private float frontWalking, sideWalking;


    private void Update()
    {
        animations.SetMovingSpeed(frontWalking,sideWalking);
    }

    [Button("Shoot")]
    private void Shoot()
    {
        animations.Shoot();
    }

    [Button("Reload")]
    private void Reload()
    {
        animations.Reload();
    }

    [Button("ChangeWeapon")]
    private void ChangeWeapon()
    {
        animations.SetWeaponID(weaponID);
        animations.ChangeWeapon();
    }

    [Button("ThrowGrenade")]
    private void ThrowGrenade()
    {
        animations.ThrowGrenade();
    }
}
