
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Melee : Weapon
{
    
    private BoxCollider _collider;
    private CancellationTokenSource _cts;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider>();
        _cts = new CancellationTokenSource();
    }

    private void Update()
    {
        timeSinceLastShot += Time.deltaTime;
    }

    public override bool CanShoot() => timeSinceLastShot > 1f / (fireRate / 60f);

    public override void Shoot()
    {
        if (CanShoot())
        { 
            timeSinceLastShot = 0;
           Attack();
           //_animationManager.Attack();
        }
    }

    private void Attack()
    {
        UniTask.Void(async () =>
        {
            try
            {
                _collider.enabled = true;
                await UniTask.Delay(TimeSpan.FromSeconds(0.1f), cancellationToken: _cts.Token);
                _collider.enabled = false;
            }
            catch (Exception e)
            {
                _cts.Cancel();
                _cts = new CancellationTokenSource();
            }
          
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamageable damageable)
            && other.gameObject.CompareTag("Player"))
        {
            damageable.TakeDamage(damage);
        }
    }
}
