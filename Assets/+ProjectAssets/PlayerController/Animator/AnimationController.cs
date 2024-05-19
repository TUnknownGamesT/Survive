using System;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [HideInInspector]
    public Animator animator;
    protected Transform _transform;
    protected Vector3 _oldPosition;
    
    protected const float _animationMultiplier=30;
    
    protected readonly int frontWalking = Animator.StringToHash("frontWalking");
    protected readonly int sideWalking = Animator.StringToHash("sideWalking");
    
    [HideInInspector]
    public Rigidbody[] rigidbodies;
    

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        _transform = transform.GetChild(0);
        SetRagdollEnabled(false);
    }
    
    public void SetRagdollEnabled(bool isEnabled)
    {
        
        gameObject.GetComponent<CapsuleCollider>().enabled = !isEnabled;
        animator.enabled = !isEnabled;

        foreach (var rb in rigidbodies)
        {
            if (rb!=null)
            {
                rb.isKinematic = !isEnabled;
                rb.detectCollisions = isEnabled;
                if (isEnabled)
                {
                    rb.mass = 0.5f;
                }
            }
        }
        
    }
    // Update is called once per frame
    public void SetWalkingAnimation()
    {
        
        Vector3 newToOld = _oldPosition - _transform.position;

        float directionValue = Vector3.Dot(_transform.forward, newToOld);
        animator.SetFloat(frontWalking, directionValue*_animationMultiplier);
        
        directionValue = Vector3.Dot(_transform.right, newToOld);
        animator.SetFloat(sideWalking, directionValue*_animationMultiplier);
        
        _oldPosition = _transform.position;
    }
    
    public void ThrowGrenade()
    {
        animator.SetTrigger("grenade");
    }
    
    public virtual void Die()
    {
        SetRagdollEnabled(true);
    }
}
