using System;
using UnityEngine;

public class EnemyAnimations : AnimationController
{
    public WeaponAnimations myWeapon;
    [HideInInspector]
    public bool throwUp = false;
    
    private int WeaponType;


    protected override void Start()
    {
        base.Start();
        animator = transform.GetChild(0).GetComponent<Animator>();
        /*//TODO: set the movement somewhere
        EnemyMovement movement = gameObject.GetComponent<EnemyMovement>();
        if (throwUp)
        {
            ThrowUp(true);
        }else
        if (movement != null && movement.travelPoints.Count == 0) 
        {
            Smoke(true);
        }*/
    }

    private void Update()
    {
        SetWalkingAnimation();
    }

    public void SetWeapon()
    {
        //set weapon code
        throw new NotImplementedException();
    }


    public void Smoke(bool value)
    {
        animator.SetBool("smoke", value);
    }

    public void ThrowUp(bool value)
    {
        animator.SetBool("throwUp", value);
    }
    
}
