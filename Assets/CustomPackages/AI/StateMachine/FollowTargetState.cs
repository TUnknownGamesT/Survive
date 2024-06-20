using UnityEngine;
using UnityEngine.AI;

public class FollowTargetState : IState
{
    private NavMeshAgent _navMeshAgent;
    private Transform _enemyBody;
    private float _stoppingDistance;
    private ZombieAnimationManager _zombieAnimations;

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
       _zombieAnimations.SetSpeed(1);
    }

    public void OnUpdate()
    {
        _navMeshAgent.destination = Vector3.Distance(_enemyBody.position, GameObject.FindWithTag("Player").transform.position) > _stoppingDistance 
            ? GameManager.playerRef.transform.position : _enemyBody.position;
    }

    public void OnExit()
    {
        _navMeshAgent.destination = _enemyBody.position;
        _zombieAnimations.SetSpeed(0);
    }
    
}
