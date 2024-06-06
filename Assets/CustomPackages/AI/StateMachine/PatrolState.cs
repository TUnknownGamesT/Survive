using System;
using System.Collections.Generic;
using System.Threading;
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
    private int _travelPointIndex;
    
   

    public void OnInitState<T>(T gameObject)
    {
        if (gameObject is EnemyType enemyStats)
        {
            _navMeshAgent = enemyStats.navMeshAgent;
            travelPoints = enemyStats.travelPoints;
            pauseBetweenMovement = enemyStats.pauseBetweenMovement;
            stoppingDistance = enemyStats.stoppingDistance;
            enemyBody = enemyStats.aiBody.transform;
        }
        
    }
    
    public void OnEnter()
    {
        _cts = new CancellationTokenSource();
        
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
       
    }

    public void OnExit()
    {
        _cts.Cancel();
        if(travelPoints.Contains(GameObject.FindWithTag("Player").transform))
            travelPoints.Remove(GameObject.FindWithTag("Player").transform);
        _navMeshAgent.destination = enemyBody.position;
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
