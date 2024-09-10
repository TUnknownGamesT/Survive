using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagDollComponent : MonoBehaviour
{
    public List<Rigidbody> ragdollBodies;
    public List<Collider> ragdollColliders;
    public GameObject doll;


    public void ActivateRagDoll()
    {
        foreach (var rb in ragdollBodies)
        {
            rb.isKinematic = false;
        }

        foreach (var col in ragdollColliders)
        {
            col.isTrigger = false;
        }
    }


    [ContextMenu("PrepareModel")]
    public void PrepareModel()
    {
        ragdollBodies = new List<Rigidbody>(doll.GetComponentsInChildren<Rigidbody>());
        foreach (var rb in ragdollBodies)
        {
            rb.isKinematic = true;
        }

        ragdollColliders = new List<Collider>(doll.GetComponentsInChildren<Collider>());
        foreach (var col in ragdollColliders)
        {
            col.enabled = true;
            col.isTrigger = true;
        }

    }
}
