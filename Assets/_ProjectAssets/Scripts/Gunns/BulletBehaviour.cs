using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BulletBehaviour : MonoBehaviour
{
    public int damage;
    public GameObject wallHitEffect;
    public GameObject bloodEffect;
    
    private void Start()
    {
        Destroy(gameObject, 5f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        { 
            Quaternion rot = Quaternion.FromToRotation(Vector3.back, collision.contacts[0].normal);
           GameObject wallMark =Instantiate(wallHitEffect, collision.contacts[0].point, rot);
           wallMark.transform.position += wallMark.transform.forward * -0.1f;
           wallMark.transform.SetParent(collision.transform);
           wallMark.transform.localScale = Vector3.one;
        }else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player"))
        {
            GameObject blood = Instantiate(bloodEffect, collision.contacts[0].point, Quaternion.LookRotation(transform.forward));
        }
        Destroy(gameObject);
    }
}
