using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using UnityEngine;


public class AIBrain : MonoBehaviour
{
    [Header("Movement Settings")]
    public List<Transform> travelPoints;
    public float stoppingDistance;
    public float pauseBetweenMovement;
    public int health;


    //References Components
    private EnemyAnimations _enemyAnimations;
    private AIHealth _aiHealth;
    
    private IState _currentState;
    private bool _playerInView;
    private bool _alive = true;
    private bool _alreadyNoticed;

    
    #region State Initialization

    private PatrolState _patrolState;
    private DeadState _deadState;

    #endregion


    private void Awake()
    {
        _enemyAnimations= GetComponent<EnemyAnimations>();
        _aiHealth = GetComponent<AIHealth>();
        
        _patrolState = new PatrolState();
        _deadState = new DeadState();
    }


    // Start is called before the first frame update
    void Start()
    {
        
        _aiHealth.Init(health);
        
        _patrolState.OnInitState(gameObject);
        _deadState.OnInitState(transform);
    }

    private void Update()
    {
        MakeDecision();
        _currentState?.OnUpdate();
    }

    private void MakeDecision()
    {
        if (_alive)
        {
            if(!_playerInView)
            {
                ChangeState(_patrolState);
            }
        }
    }

    public void Notice()
    {
        if (!_alreadyNoticed)
        {
            _patrolState.AddTravelPoint(GameObject.FindWithTag("Player").transform);
        }
        else
        {
            UniTask.Void(async () =>
            {
                await UniTask.Delay(TimeSpan.FromSeconds(4));
                _alreadyNoticed= false;
            });
        }
    }
    
    
    private void ChangeState(IState newState)
    {
        if (newState != _currentState)
        {
            _currentState?.OnExit();
            _currentState = newState;
            _currentState.OnEnter();
        }
    }
    

    public void Death()
    {
        _enemyAnimations.Die();
        _alive = false;
        ChangeState(_deadState);
        enabled = false;
    }

    public void PlayerInView()
    {
        throw new NotImplementedException();
    }

    public void PlayerOutOfView()
    {
        throw new NotImplementedException();
    }
}
