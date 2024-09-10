using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;


public class AIBrain : IAIBrain
{

    public static Action onEnemyDeath;

    [Header("Enemy Type")]
    public ConstantsValues.EnemyType enemyType;

    [Header("Movement Settings")]
    public List<Transform> travelPoints = new();


    private float _stoppingDistance;
    [Header("References")]
    public VisibleChecker visibleChecker;
    public Transform armSpawnPoint;
    private ZombieAnimationManager _enemyAnimations;
    private AIHealth _aiHealth;
    private SoundComponent _soundComponent;

    private IState _currentState;
    private bool _activeTargetInView;
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
        _enemyAnimations = GetComponent<ZombieAnimationManager>();

        _aiHealth = GetComponent<AIHealth>();

        _patrolState = new PatrolState();
        _deadState = new DeadState();
        _followTargetState = new FollowTargetState();
        _attackState = new AttackState();
    }


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
        if (!_alive && !visibleChecker.isVisible)
        {
            Destroy(gameObject);
        }
        else if (_alive)
        {
            MakeDecision();
            _currentState?.OnUpdate();
        }

    }

    private void MakeDecision()
    {

        if (_activeTargetInView && Vector3.Distance(transform.position, _currentTarget.position) <= _stoppingDistance)
        {
            ChangeState(_attackState);
        }
        else if (_activeTargetInView && Vector3.Distance(transform.position, _currentTarget.position) > _stoppingDistance)
        {
            ChangeState(_followTargetState);
        }
        else if (!_activeTargetInView)
        {
            ChangeState(_patrolState);
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
            ChangeState(_deadState);
            base.Death();
            _alive = false;
            onEnemyDeath?.Invoke();
            CameraController.SlowMotion(0.2f);
            _enemyAnimations.Die();
            StartCoroutine(DestroyGameObject());
            FactoryObjects.instance.CreateObject(
                new FactoryObject<Vector3>(
                    FactoryObjectsType.XPItem, 
                    gameObject.transform.position
                    ));
        }
    }

    IEnumerator DestroyGameObject()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    public override void BaseInView(Transform basePoint)
    {
        _currentTarget = basePoint;
        _activeTargetInView = true;
        _attackState.SetTarget(basePoint);
        _followTargetState.SetTarget(basePoint);
    }

    public override void PlayerInView()
    {

        if (_currentTarget != null)
        {
            _attackState.SetTarget(GameManager.playerRef);
            _followTargetState.SetTarget(GameManager.playerRef);
            if (_currentTarget != GameManager.playerRef.transform)
            {
                Transform mock = _currentTarget;
                _currentTarget = GameManager.playerRef.transform;
                Destroy(mock.gameObject);
            }
        }
        else
        {
            _currentTarget = GameManager.playerRef.transform;
            _attackState.SetTarget(GameManager.playerRef);
            _followTargetState.SetTarget(GameManager.playerRef);
        }

        _activeTargetInView = true;

    }

    public override void PlayerOutOfView()
    {
        Debug.Log("<color=yellow>Player out of view</color>");
        _activeTargetInView = false;
        //_patrolState.AddTravelPoint(GameManager.playerRef);
    }
}
