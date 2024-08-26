using System;
using System.Collections.Generic;
using ConstantsValues;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;


public class AIBrain : IAIBrain
{

    public static Action onEnemyDeath;
    public Action onLocalEnemyDeath;

    [Header("Enemy Type")]
    public ConstantsValues.EnemyType enemyType;

    [Header("Movement Settings")]
    public List<Transform> travelPoints = new();


    private float _stoppingDistance;
    [Header("References")]
    public Transform armSpawnPoint;
    private ZombieAnimationManager _enemyAnimations;
    private AIHealth _aiHealth;
    private SoundComponent _soundComponent;

    private IState _currentState;
    private bool _activeTargetInView;
    private bool _alive = true;
    private bool _alreadyNoticed;
    private Transform _currentTarget;
    public Transform basePoint;



    #region State Initialization

    private PatrolState _patrolState;
    private DeadState _deadState;
    private FollowTargetState _followTargetState;
    private AttackState _attackState;

    #endregion


    private void Awake()
    {
        travelPoints.Add(GameManager.playerBaseRef);
        _enemyAnimations = GetComponent<ZombieAnimationManager>();

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
        FactoryObjects.instance.CreateObject(new FactoryObject<EnemyWeaponInstructions>
            (FactoryObjectsType.EnemyWeapon, new EnemyWeaponInstructions(ConstantsValues.EnemyType.Melee, armSpawnPoint)));
        mockEnemyType.soundComponent = _soundComponent;
        mockEnemyType.aiBody = gameObject;
        mockEnemyType.navMeshAgent = GetComponent<NavMeshAgent>();
        mockEnemyType.navMeshAgent.speed = mockEnemyType.speed;
        mockEnemyType.travelPoints = travelPoints;
        mockEnemyType.armPrefab = armSpawnPoint.GetChild(0).gameObject;
        mockEnemyType.armPrefab.GetComponent<Weapon>().SetArmHandler(_enemyAnimations);


        _stoppingDistance = mockEnemyType.stoppingDistance;

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
            if (_activeTargetInView && Vector3.Distance(transform.position, _currentTarget.position) <= _stoppingDistance)
            {
                ChangeState(_attackState);
                Debug.LogWarning("Attack State");
            }
            else if (_activeTargetInView && Vector3.Distance(transform.position, _currentTarget.position) > _stoppingDistance)
            {
                ChangeState(_followTargetState);
                Debug.LogWarning("Follow State");
            }
            else if (!_activeTargetInView)
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
                _alreadyNoticed = false;
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



    public override void Death()
    {
        if (_alive)
        {
            base.Death();
            _alive = false;
            onEnemyDeath?.Invoke();
            CameraController.SlowMotion(0.2f);
            _enemyAnimations.Die();
            enabled = false;
            ChangeState(_deadState);
            Destroy(gameObject, 1f);
        }
    }

    public override void BaseInView(Transform basePoint)
    {
        this.basePoint = basePoint;
        _currentTarget = basePoint;
        _activeTargetInView = true;
        _attackState.SetTarget(basePoint);
        _followTargetState.SetTarget(basePoint);
    }

    public override void PlayerInView()
    {
        if (_currentTarget != GameManager.playerRef.transform)
            Destroy(_currentTarget.gameObject);
        _currentTarget = GameManager.playerRef.transform;
        _attackState.SetTarget(GameManager.playerRef);
        _followTargetState.SetTarget(GameManager.playerRef);
        _activeTargetInView = true;
    }

    public override void PlayerOutOfView()
    {
        Debug.LogWarningFormat("<color=reed>Add last seen target not player every time SOLVE</color>");
        _activeTargetInView = false;
        _patrolState.AddTravelPoint(GameManager.playerRef);
    }
}
