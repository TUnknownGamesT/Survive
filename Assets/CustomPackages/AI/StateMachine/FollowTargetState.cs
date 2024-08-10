using UnityEngine;
using UnityEngine.AI;

public class FollowTargetState : IState
{
    private NavMeshAgent _navMeshAgent;
    private Transform _enemyBody;
    private float _stoppingDistance;
    private ZombieAnimationManager _zombieAnimations;

    private Transform _currentTarget;


    public void OnInitState<T>(T gameObject)
    {
        if(gameObject is EnemyType enemyStats)
        {
            _navMeshAgent = enemyStats.navMeshAgent;
            _enemyBody = enemyStats.aiBody.transform;
            _stoppingDistance = enemyStats.stoppingDistance;
            _zombieAnimations = enemyStats.aiBody.GetComponent<ZombieAnimationManager>();
        }
    }

    public void OnEnter()
    {
        Debug.Log("Follow Target State");
        _zombieAnimations.SetSpeed(1);
    }

    public void OnUpdate() 
    {
        if (Vector3.Distance(_enemyBody.position, _currentTarget.transform.position) > _stoppingDistance)
        {
            Debug.Log("Following Target");
            _navMeshAgent.destination = _currentTarget.position;
        }
    }

    public void OnExit()
    {
        _navMeshAgent.destination = _enemyBody.position;
        _zombieAnimations.SetSpeed(0);
    }

    public void SetTarget(Transform targetPosition)
    {
        _currentTarget= targetPosition;
    }
    
}
