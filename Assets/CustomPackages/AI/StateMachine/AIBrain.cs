using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;


public class AIBrain : MonoBehaviour
{
    
    public static Action onEnemyDeath;
    
    [Header("Enemy Type")]
    public Constants.EnemyType enemyType;
    
    [Header("Movement Settings")]
    public List<Transform> travelPoints =new();
    

    private float _stoppingDistance;
    private float _stoppingDistanceBase = 15f;
    private float _currentStoppingDistance;
    [Header("References")]
    public Transform armSpawnPoint;
    private EnemyAnimations _enemyAnimations;
    private AIHealth _aiHealth;
    private SoundComponent _soundComponent;
    
    private IState _currentState;
    private bool _activeTargetInView;
    private bool _alive = true;
    private bool _alreadyNoticed;
    private Transform _currentTarget;


    #region State Initialization

    private PatrolState _patrolState;
    private DeadState _deadState;
    private FollowTargetState _followTargetState;
    private AttackState _attackState;

    #endregion


    private void Awake()
    {
        
        
        travelPoints.Add(GameManager.playerBaseRef);
        _currentTarget = GameManager.playerBaseRef;
        _enemyAnimations= GetComponent<EnemyAnimations>();
        _aiHealth = GetComponent<AIHealth>();
        
        _patrolState = new PatrolState();
        _deadState = new DeadState();
        _followTargetState = new FollowTargetState();
        _attackState = new AttackState();
    }


    // Start is called before the first frame update
    void Start()
    {
        EnemyType mockEnemyType = EnemyInitiator.instance.GetEnemyStats(enemyType);
        mockEnemyType.armSpawnPoint = armSpawnPoint;
        mockEnemyType.soundComponent = _soundComponent;
        mockEnemyType.aiBody = gameObject;
        mockEnemyType.navMeshAgent = GetComponent<NavMeshAgent>();
        mockEnemyType.navMeshAgent.speed = mockEnemyType.speed;
        mockEnemyType.travelPoints = travelPoints;
        mockEnemyType.armPrefab.GetComponent<Gun>().SetArmHandler(_enemyAnimations);

        _stoppingDistance = mockEnemyType.stoppingDistance;
        _currentStoppingDistance = _stoppingDistanceBase;

        _aiHealth.Init(mockEnemyType.health);
        
        _patrolState.OnInitState(mockEnemyType);
        _deadState.OnInitState(mockEnemyType);
        _followTargetState.OnInitState(mockEnemyType);
        _attackState.OnInitState(mockEnemyType);
        _attackState.SetTarget(GameManager.playerBaseRef);
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
            if (_activeTargetInView &&  Vector3.Distance(transform.position, _currentTarget.position) <= _currentStoppingDistance)
            {
                ChangeState(_attackState);
                Debug.LogWarning("Attack State");
            }else if(_activeTargetInView &&Vector3.Distance(transform.position, _currentTarget.position) > _currentStoppingDistance)
            {
                ChangeState(_followTargetState);
                Debug.LogWarning("Follow Player State");
            }
            else if(!_activeTargetInView)
            {
                ChangeState(_patrolState);
                Debug.LogWarning("Patrol State");
            }
        }
    }

    public void Notice()
    {
        if (!_alreadyNoticed)
        {
            _patrolState.AddTravelPoint(GameManager.playerRef.transform);
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
        if (_alive)
        {
            onEnemyDeath?.Invoke();
            CameraController.SlowMotion(0.2f);
            //_enemyAnimations.Die();
            enabled = false;
            _alive = false;
            ChangeState(_deadState);
            Destroy(gameObject,1f);
        }
    }
    
    public void PlayerInView()
    {
        _currentStoppingDistance =  _stoppingDistance;
        _activeTargetInView = true;
        _attackState.SetTarget(GameManager.playerRef);
    }

    //Add last seen target not player every time
    public void PlayerOutOfView()
    {
        Debug.LogWarning("Player Out of View");
        _activeTargetInView = false;
        _patrolState.AddTravelPoint(GameManager.playerRef);
        _currentTarget = GameManager.playerBaseRef;
        _attackState.SetTarget(GameManager.playerBaseRef);
        _currentStoppingDistance = _stoppingDistanceBase;
    }
}
