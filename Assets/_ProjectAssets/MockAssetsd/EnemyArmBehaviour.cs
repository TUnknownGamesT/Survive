using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnemyArmBehaviour : MonoBehaviour
{

    public float damage;

    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("EnemyArmBehaviour: OnTriggerEnter");
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(damage);
        }
    }
}
