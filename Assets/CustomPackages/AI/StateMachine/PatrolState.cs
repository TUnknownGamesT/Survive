using System;
using System.Collections.Generic;
using System.Threading;
using ConstantsValues;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PatrolState : IState
{
    private List<Transform> travelPoints;
    private float pauseBetweenMovement;
    private float stoppingDistance;
    private Transform enemyBody;
    
    private NavMeshAgent _navMeshAgent;
    private CancellationTokenSource _cts;
    private int _travelPointIndex = 0;
    private ZombieAnimationManager _zombieAnimations;

    
   

    public void OnInitState<T>(T gameObject)
    {
        if (gameObject is EnemyType enemyStats)
        {
            _navMeshAgent = enemyStats.navMeshAgent;
            travelPoints = enemyStats.travelPoints;
            pauseBetweenMovement = enemyStats.pauseBetweenMovement;
            stoppingDistance = enemyStats.stoppingDistance;
            enemyBody = enemyStats.aiBody.transform;
            _zombieAnimations = enemyStats.aiBody.GetComponent<ZombieAnimationManager>();
        }
        
    }
    
    public void OnEnter()
    {
        _cts = new CancellationTokenSource();
        _zombieAnimations.SetSpeed(1);
        if (travelPoints.Count > 0)
        {
            if (travelPoints.Contains(GameManager.playerRef.transform))
            {
                _travelPointIndex = travelPoints.IndexOf(GameManager.playerRef.transform);
            }
            Travel();
        }
    }

    public void OnUpdate()
    {
       ShootRaycast();
    }

    public void OnExit()
    {
        _cts.Cancel();
        if(travelPoints.Contains(GameObject.FindWithTag("Player").transform))
            travelPoints.Remove(GameObject.FindWithTag("Player").transform);
        _navMeshAgent.destination = enemyBody.position;
        _zombieAnimations.SetSpeed(0);
    }
    
    private void ShootRaycast()
    {
        RaycastHit hit;
        Vector3 direction =  GameManager.playerBaseRef.position - enemyBody.position;
        if (Physics.Raycast(enemyBody.position, direction, out hit, Mathf.Infinity, Constants.instance.baseLayer))
        {
            GameObject playerBaseHitPoint = new GameObject
            {
                transform =
                {
                    position = new Vector3(hit.point.x,0, hit.point.z)
                }
            };
            enemyBody.gameObject.GetComponent<AIBrain>().BaseInView(playerBaseHitPoint.transform);
        }
    }
    
    public void AddTravelPoint(Transform newPoint)
    {
        travelPoints.Add(newPoint);
    }

    private void Travel()
    {
        UniTask.Void(async () =>
        {
            try
            {
                if(travelPoints.Count == 0)
                    return;
                
                _navMeshAgent.destination = travelPoints[_travelPointIndex].position;

                _travelPointIndex = Random.Range(0, travelPoints.Count);

                await UniTask.WaitUntil(()=>Vector3.Distance(enemyBody.position,_navMeshAgent.destination) <= stoppingDistance, cancellationToken: _cts.Token);

                await UniTask.Delay(TimeSpan.FromSeconds(pauseBetweenMovement), cancellationToken: _cts.Token);
                
                if(travelPoints.Contains(GameObject.FindWithTag("Player").transform))
                    travelPoints.Remove(GameObject.FindWithTag("Player").transform);

                Travel();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.Log("Miss Reference");
            }
        });
    }
}
