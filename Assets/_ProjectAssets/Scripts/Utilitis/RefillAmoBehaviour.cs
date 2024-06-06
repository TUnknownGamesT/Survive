using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RefillAmoBehaviour : MonoBehaviour
{

    [Tooltip("How many bullets refill per refill")]
    public int refillAmo;

    [Tooltip("The amount of time between each refill")]
    public float timeBetweenRefills;


    private CancellationTokenSource _cts;
    
    // Start is called before the first frame update
    void Start()
    {
        _cts= new CancellationTokenSource();
    }

    private void RefillAmo()
    {
        
        UniTask.Void(async () =>
        {
            await UniTask.Delay(TimeSpan.FromSeconds(timeBetweenRefills),cancellationToken:_cts.Token);
            GameManager.playerRef.GetComponent<PlayerBrain>().RefillAmo(refillAmo);
            RefillAmo();
        });
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RefillAmo();
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
