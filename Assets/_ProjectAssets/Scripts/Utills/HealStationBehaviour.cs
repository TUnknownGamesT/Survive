using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class HealStationBehaviour : MonoBehaviour
{
    
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
            GameManager.playerRef.GetComponent<PlayerBrain>().Heal(healAmount);
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
