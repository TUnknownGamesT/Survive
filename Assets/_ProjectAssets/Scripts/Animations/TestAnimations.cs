using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum weaponIDs
{
    //Note: it is important that the names from here are identic to the sub_states names
    Pistol,
    AK,
    Shotgun
}
public class TestAnimations : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private int weaponID;


    #region Weapon

    public void Shoot()
    {
        animator.Play((weaponIDs)weaponID+".Shoot");
    }
    
    public void Reload()
    {
        animator.SetTrigger("T_Reload");
    }

    public void SetWeaponID(int id)
    {
        weaponID = id;
        animator.SetInteger("I_WeaponID", id);
    }

    public void ChangeWeapon()
    {
        animator.SetTrigger("T_ChangeWeapon");
    }

    public void ChangeWeaponWithID(int id)
    {
        SetWeaponID(id);
        ChangeWeapon();
    }
    #endregion

    #region Movements

    public void SetMovingSpeed(float frontWalking, float sideWalking)
    {
        animator.SetFloat("F_FrontWalking", frontWalking);
        animator.SetFloat("F_SideWalking", sideWalking);
    }

    #endregion
    public void ThrowGrenade()
    {
        animator.Play("ThrowGrenade");
    }

    
}
