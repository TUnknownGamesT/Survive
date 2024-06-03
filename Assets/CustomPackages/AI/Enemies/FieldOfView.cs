using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    
    public float radius;
    [Range(0, 360)] 
    public float angle;
    public LayerMask obstructionMask;
    public LayerMask targetMask;
    [HideInInspector]
    public bool canSeePlayer;
    [HideInInspector]
    public GameObject _playerRef;

    private bool _alreadyInView;
    private CancellationTokenSource _cts;
    private AIBrain _aiBrain;
    

    private void Awake()
    {
        _aiBrain = GetComponent<AIBrain>();
        _cts = new CancellationTokenSource();
    }
    
    
    private void Start()
    {
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        FiledOfViewCheck();
    }


    public void EnemyDeath()
    {
        canSeePlayer = false;
        _cts.Cancel();
    }
    
    private void FiledOfViewCheck()
    {
        UniTask.Void(async () =>
        {
            
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.05f), cancellationToken: _cts.Token);
                
                Collider[] rangeChecks =
                    Physics.OverlapSphere(transform.position, radius, targetMask, QueryTriggerInteraction.Collide);

                if (rangeChecks.Length != 0)
                {
                    Transform target = rangeChecks[0].transform;
                    Vector3 directionToTarget = (target.position - transform.position).normalized;
                
                    if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                    {
                        float distanceToTarget = Vector3.Distance(transform.position, target.position);
                        canSeePlayer = !Physics.Raycast(transform.position, directionToTarget, distanceToTarget,
                            obstructionMask);
                        Debug.LogWarning(canSeePlayer);
                        if (canSeePlayer && !_alreadyInView)
                        {
                            _aiBrain.PlayerInView();
                            _alreadyInView = true;
                        }
                        else if(!canSeePlayer && _alreadyInView)
                        {
                            canSeePlayer = false;
                            _alreadyInView = false;
                            _aiBrain.PlayerOutOfView();
                        }
                    }
                    else if(_alreadyInView)
                    {
                        canSeePlayer = false;
                        _alreadyInView = false;
                        _aiBrain.PlayerOutOfView();
                    }
                }else if(_alreadyInView)
                {
                    canSeePlayer = false;
                    _alreadyInView = false;
                    _aiBrain.PlayerOutOfView();
                }

                FiledOfViewCheck();
            }
            catch (Exception e)
            {
                Debug.Log("Thread Miss reference",this);
                Debug.Log(e);
                _cts.Cancel();
            }
        });
    }
}