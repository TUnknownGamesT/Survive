using UnityEngine;
using UnityEngine.AI;

public class FollowTargetState : IState
{
    private NavMeshAgent _navMeshAgent;
    private Transform _enemyBody;
    private float _stoppingDistance;
    private Transform _currentTarget;

    public void OnInitState<T>(T gameObject)
    {
        if(gameObject is EnemyType enemyStats)
        {
            _navMeshAgent = enemyStats.navMeshAgent;
            _enemyBody = enemyStats.aiBody.transform;
            _stoppingDistance = enemyStats.stoppingDistance;
        }
    }

    public void OnEnter()
    {
       
    }

    public void OnUpdate()
    {
        _navMeshAgent.destination = Vector3.Distance(_enemyBody.position, _currentTarget.transform.position) > _stoppingDistance 
            ? GameManager.playerRef.transform.position : _enemyBody.position;
    }

    public void OnExit()
    {
        _navMeshAgent.destination = _enemyBody.position;
    }

    public void SetTarget(Transform targetPosition)
    {
        _currentTarget= targetPosition;
    }
    
}
