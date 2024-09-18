using System;
using UnityEngine;

public class AIHealth : MonoBehaviour, IDamageable
{

    [HideInInspector]
    public float health;
    //private EnemyBrain _enemyBrain;
    private float forceMultiplier = 10;
    private AIBrain _aiBrain;
    private Vector3 _collisionPoint;


    private void Awake()
    {
        _aiBrain = GetComponent<AIBrain>();
    }

    public void Init(int health)
    {
        this.health = health;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        SoundManager.instance.PlaySoundEffect(_aiBrain.enemyType, _aiBrain._soundComponent, SoundEffects.GetDamage);
        if (health <= 0)
        {
            //CameraController.instance.KillEffect();
            _aiBrain.Death();
            //ActivateRagDoll(_collisionPoint);
            //CameraController.SlowMotion(0.5f);
        }
    }

    private void ActivateRagDoll(Vector3 collisionPoint)
    {
        foreach (var rb in GetComponent<AnimationManager>().ragDollController.ragdollBodies)
        {
            Vector3 direction = collisionPoint - transform.position;
            direction.y = 0;
            float distance = direction.magnitude;

            // Calculate the force to be applied using an inverse square law
            float force = (2 - distance) * forceMultiplier;
            if (force < 0)
            {
                force = 0;
            }
            else
            {
                force = -force;
            }

            // Apply the force
            rb.AddForce(direction.normalized * force, ForceMode.Impulse);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            _collisionPoint = collision.contacts[0].point;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            _collisionPoint = other.transform.position;
        }
    }
}
