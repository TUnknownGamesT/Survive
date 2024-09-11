using UnityEngine;
using UnityEngine.AI;

public class FollowTargetState : IState
{
    private NavMeshAgent _navMeshAgent;
    private Transform _enemyBody;
    private float _stoppingDistance;
    private AnimationManager _enemyAnimations;

    private Transform _currentTarget;


    public void OnInitState<T>(T gameObject)
    {
        if (gameObject is EnemyType enemyStats)
        {
            _navMeshAgent = enemyStats.navMeshAgent;
            _navMeshAgent.speed = enemyStats.speed;
            _navMeshAgent.angularSpeed = enemyStats.damping;
            _navMeshAgent.stoppingDistance = enemyStats.stoppingDistance;
            _enemyBody = enemyStats.aiBody.transform;
            _stoppingDistance = enemyStats.stoppingDistance;
            if (_enemyBody.GetComponent<ZombieAnimationManager>() == null)
                _enemyAnimations = _enemyBody.gameObject.GetComponent<EnemyAnimations>();
            else
                _enemyAnimations = enemyStats.aiBody.GetComponent<ZombieAnimationManager>();
        }
    }

    public void OnEnter()
    {
        _enemyAnimations.SetIsWalking(true);
    }

    public void OnUpdate()
    {
        if (Vector3.Distance(_enemyBody.position, _currentTarget.transform.position) > _stoppingDistance)
        {
            _navMeshAgent.destination = _currentTarget.position;
        }
    }

    public void OnExit()
    {
        _navMeshAgent.destination = _enemyBody.position;
        _enemyAnimations.SetIsWalking(false);
    }

    public void SetTarget(Transform targetPosition)
    {
        _currentTarget = targetPosition;
    }

}
