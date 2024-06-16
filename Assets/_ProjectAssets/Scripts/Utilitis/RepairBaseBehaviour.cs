using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RepairBaseBehaviour : MonoBehaviour
{
    public static Action<int> onBaseRepaired;
    
    [Tooltip("Heal amount per heal")]
    public int healAmount;
    [Tooltip("The amount of time between each heal")]
    public float timeBetweenHeals;

    private CancellationTokenSource _cts;

    private void Start()
    {
        _cts= new CancellationTokenSource();
    }

    private void Heal()
    {
        UniTask.Void(async () =>
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timeBetweenHeals),cancellationToken:_cts.Token);
            onBaseRepaired?.Invoke(healAmount);
            Heal();
        });
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Heal();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _cts.Cancel();
            _cts = new CancellationTokenSource();
        }
    }
}
