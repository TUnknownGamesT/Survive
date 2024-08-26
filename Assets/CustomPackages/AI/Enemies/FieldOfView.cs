using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;

public class FieldOfView : MonoBehaviour
{

    public float radius;
    [Range(0, 360)]
    public float angle;
    public LayerMask obstructionMask;
    public LayerMask targetMask;
    [FormerlySerializedAs("canSeePlayer")]
    [HideInInspector]
    public bool targetInView;
    [HideInInspector]
    public GameObject _playerRef;

    private bool _alreadyInView;
    private CancellationTokenSource _cts;
    private IAIBrain _aiBrain;


    private void Awake()
    {
        _aiBrain = GetComponent<IAIBrain>();
        _cts = new CancellationTokenSource();
    }

    void OnEnable()
    {
        _aiBrain.onLocalEnemyDeath += EnemyDeath;
    }

    void OnDisable()
    {
        _aiBrain.onLocalEnemyDeath -= EnemyDeath;
    }


    private void Start()
    {
        _playerRef = GameObject.FindGameObjectWithTag("Player");
        FiledOfViewCheck();
    }


    public void EnemyDeath()
    {
        _cts.Cancel();
        targetInView = false;
    }

    private void FiledOfViewCheck()
    {
        UniTask.Void(async () =>
        {

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.05f)).WithCancellation(_cts.Token);

                Collider[] rangeChecks =
                    Physics.OverlapSphere(transform.position, radius, targetMask, QueryTriggerInteraction.Collide);

                if (rangeChecks.Length != 0)
                {
                    Transform target = rangeChecks[0].transform;
                    Vector3 directionToTarget = (target.position - transform.position).normalized;

                    if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
                    {
                        float distanceToTarget = Vector3.Distance(transform.position, target.position);
                        targetInView = !Physics.Raycast(transform.position, directionToTarget, distanceToTarget,
                            obstructionMask);

                        if (targetInView && !_alreadyInView)
                        {
                            Debug.LogWarning("Player in View");
                            _aiBrain.PlayerInView();
                            _alreadyInView = true;
                        }
                    }
                    else if (_alreadyInView)
                    {
                        targetInView = false;
                        _alreadyInView = false;
                        _aiBrain.PlayerOutOfView();
                    }
                }
                else if (_alreadyInView)
                {
                    targetInView = false;
                    _alreadyInView = false;
                    _aiBrain.PlayerOutOfView();
                }

                FiledOfViewCheck();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                _cts.Cancel();
            }
        });
    }
}