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
    private int _travelPointIndex = 0;
    
   

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
       ShootRaycast();
    }

    public void OnExit()
    {
        _cts.Cancel();
        if(travelPoints.Contains(GameObject.FindWithTag("Player").transform))
            travelPoints.Remove(GameObject.FindWithTag("Player").transform);
        _navMeshAgent.destination = enemyBody.position;
    }
    
    private void ShootRaycast()
    {
        RaycastHit hit;
        
        if (Physics.Raycast(enemyBody.position, enemyBody.TransformDirection(Vector3.forward), out hit, 10, Constants.baseLayer))
        {
            Debug.Log("Base in View");
            GameObject playerBaseHitPoint = new GameObject();
            playerBaseHitPoint.transform.position = hit.point;
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
