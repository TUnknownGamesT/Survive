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
    

    private float stoppingDistance;
    [Header("References")]
    public Transform armSpawnPoint;
    private EnemyAnimations _enemyAnimations;
    private AIHealth _aiHealth;
    private SoundComponent _soundComponent;
    
    private IState _currentState;
    private bool _playerInView;
    private bool _alive = true;
    private bool _alreadyNoticed;

    
    #region State Initialization

    private PatrolState _patrolState;
    private DeadState _deadState;
    private FollowPlayerState _followPlayerState;
    private AttackState _attackState;

    #endregion


    private void Awake()
    {
        
        
        travelPoints.Add(GameManager.playerBaseRef);
        _enemyAnimations= GetComponent<EnemyAnimations>();
        _aiHealth = GetComponent<AIHealth>();
        
        _patrolState = new PatrolState();
        _deadState = new DeadState();
        _followPlayerState = new FollowPlayerState();
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
        mockEnemyType.travelPoints = travelPoints;
        mockEnemyType.armPrefab.GetComponent<Gun>().SetArmHandler(_enemyAnimations);

        stoppingDistance = mockEnemyType.stoppingDistance;
        
        _aiHealth.Init(mockEnemyType.health);
        
        _patrolState.OnInitState(mockEnemyType);
        _deadState.OnInitState(mockEnemyType);
        _followPlayerState.OnInitState(mockEnemyType);
        _attackState.OnInitState(mockEnemyType);
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
            if (_playerInView &&  Vector3.Distance(transform.position, GameManager.playerRef.position) <= stoppingDistance)
            {
                ChangeState(_attackState);
                Debug.LogWarning("Attack State");
            }else if(_playerInView &&Vector3.Distance(transform.position, GameManager.playerRef.position) > stoppingDistance)
            {
                ChangeState(_followPlayerState);
                Debug.LogWarning("Follow Player State");
            }
            else if(!_playerInView)
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
            //_enemyAnimations.Die();
            enabled = false;
            _alive = false;
            ChangeState(_deadState);
            Destroy(gameObject,1f);
        }
    }

    public void PlayerInView()
    {
        _playerInView = true;
    }

    public void PlayerOutOfView()
    {
        Debug.LogWarning("Player Out of View");
        _playerInView = false;
        _patrolState.AddTravelPoint(GameManager.playerRef);
    }
}
