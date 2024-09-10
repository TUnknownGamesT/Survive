using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnemyArmBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakeDamage(2);
        }
    }
}
