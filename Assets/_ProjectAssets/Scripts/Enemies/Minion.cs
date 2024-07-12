using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class Minion : MonoBehaviour, IDamageable
{
    public BoxCollider boxCollider;
    public float timeBetweenAttacks = 1f;
    public float damage;
    public static float cumulateDmg;
    public AIGroupBrain parent;
    
    private bool _timeToAttack=true;
    private CancellationTokenSource _cancellationTokenSource;
    


    private void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning(other.gameObject.name);
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerBase")
            && _timeToAttack)
        {
            Debug.LogWarning("Player/Base hit");
            other.GetComponent<IDamageable>().TakeDamage(damage);
            _timeToAttack = false;
            Rest();
        }
    }

    private void Rest()
    {
        UniTask.Void(async () =>
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await UniTask.Delay(TimeSpan.FromSeconds(timeBetweenAttacks), cancellationToken: _cancellationTokenSource.Token);
            _timeToAttack = true; 
        });
    }
    
    public void SetParent(AIGroupBrain parent)
    {
        this.parent = parent;
    }

    public void TakeDamage(float damage)
    {
        parent.Kill(this);
        Destroy(gameObject);
    }
}
