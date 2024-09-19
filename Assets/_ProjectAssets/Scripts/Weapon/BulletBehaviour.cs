using System;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletBehaviour : MonoBehaviour
{
    public float damage;
    public GameObject wallHitEffect;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("PlayerBase"))
        {
            Quaternion rot = Quaternion.FromToRotation(Vector3.back, collision.contacts[0].normal);
            GameObject wallMark = Instantiate(wallHitEffect, collision.contacts[0].point, rot);
            wallMark.transform.position += wallMark.transform.forward * -0.1f;
            wallMark.transform.SetParent(collision.transform);
            wallMark.transform.localScale = Vector3.one;
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<IDamageable>().TakeDamage(damage);
            Destroy(gameObject);
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Player"))
        {
            FactoryObjects.instance.CreateObject(new FactoryObject<Collider>(FactoryObjectsType.Blood, other));
            other.GetComponent<IDamageable>().TakeDamage(damage);
            Destroy(gameObject);
        }


    }
}