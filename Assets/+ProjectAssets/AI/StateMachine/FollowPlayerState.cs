using UnityEngine;
using UnityEngine.AI;

public class FollowPlayerState : IState
{
    private NavMeshAgent _navMeshAgent;
    private Transform _enemyBody;
    private float _stoppingDistance;

    public void OnInitState<T>(T gameObject)
    {
        if(gameObject is GameObject aiBrain)
        {
            _navMeshAgent = aiBrain.GetComponent<NavMeshAgent>();
            _enemyBody = aiBrain.transform;
            _stoppingDistance = aiBrain.GetComponent<AIBrain>().stoppingDistance;
        }
    }

    public void OnEnter()
    {
       
    }

    public void OnUpdate()
    {
        _navMeshAgent.destination = Vector3.Distance(_enemyBody.position, GameObject.FindWithTag("Player").transform.position) > _stoppingDistance 
            ? GameObject.FindWithTag("Player").transform.position : _enemyBody.position;
    }

    public void OnExit()
    {
        _navMeshAgent.destination = _enemyBody.position;
    }
    
}
