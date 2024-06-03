using System.Collections;
using UnityEngine;

public class LifeTime : MonoBehaviour
{
    public float duration = 5f;

    private void OnEnable()
    {
        StartCoroutine(Die());
    }

    private IEnumerator Die()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
