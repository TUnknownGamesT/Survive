using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeInteraction : MonoBehaviour, ISpecialInteraction
{
    public Animator animator;
    public void Interact()
    {
        animator.SetTrigger("freeze");
        StartCoroutine(WaitForDeath());
    }

    private IEnumerator WaitForDeath()
    {
        yield return new WaitForSeconds(9f);
        GetComponent<ZombieAnimationManager>().Die();
    }
}
