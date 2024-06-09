using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollController : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody[] ragdollBodies;
    [SerializeField]
    private Animator animator;

    private void OnEnable()
    {
        // Find all rigidbodies in the children of this GameObject
        ragdollBodies = GetComponentsInChildren<Rigidbody>();

        // Optionally, deactivate the ragdoll on start
        DeactivateRagdoll();
    }

    public void ActivateRagdoll()
    {
        // Disable the Animator
        if (animator != null)
        {
            animator.enabled = false;
        }

        // Set all rigidbodies to be non-kinematic
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = false;
        }
    }

    public void DeactivateRagdoll()
    {
        // Enable the Animator
        if (animator != null)
        {
            animator.enabled = true;
        }

        // Set all rigidbodies to be kinematic
        foreach (Rigidbody rb in ragdollBodies)
        {
            rb.isKinematic = true;
        }
    }
    
    public void Die()
    {
        ActivateRagdoll();
    }
}
