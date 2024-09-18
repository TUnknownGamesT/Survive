using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;


[RequireComponent(typeof(AIHealth), typeof(FieldOfView), typeof(NavMeshAgent))]
[RequireComponent(typeof(SoundComponent))]
public class AIBrain : IAIBrain
{

    public static Action onEnemyDeath;


    [Header("Enemy Type")]
    public ConstantsValues.EnemyType enemyType;

    [Header("Movement Settings")]
    public List<Transform> travelPoints = new();


    private float _stoppingDistance;
    [Header("References")]

    public RagDollComponent ragDollComponent;
    public VisibleChecker visibleChecker;
    public Transform armSpawnPoint;
    private AnimationManager _enemyAnimations;
    private AIHealth _aiHealth;
    private NavMeshAgent _navMeshAgent;
    [HideInInspector]
    public SoundComponent _soundComponent;

    private IState _currentState;
    private bool _activeTargetInView;
    private bool _alreadyNoticed;
    private Transform _currentTarget;

    private bool _canSwitchState = true;

    private FootStepsSound _footStepsSound;


    #region State Initialization

    private PatrolState _patrolState;
    private DeadState _deadState;
    private FollowTargetState _followTargetState;
    private AttackState _attackState;

    #endregion


    private void Awake()
    {
        travelPoints.Add(GameManager.playerBaseRef);
        _aiHealth = GetComponent<AIHealth>();
        _footStepsSound = GetComponent<FootStepsSound>();
        _soundComponent = GetComponent<SoundComponent>();
        _navMeshAgent = GetComponent<NavMeshAgent>();

        _patrolState = new PatrolState();
        _deadState = new DeadState();
        _followTargetState = new FollowTargetState();
        _attackState = new AttackState();
    }


    void Start()
    {
        EnemyType mockEnemyType = EnemyInitiator.instance.GetEnemyStats(enemyType);
        mockEnemyType.armSpawnPoint = armSpawnPoint;
        _enemyAnimations = GetComponent<EnemyAnimations>();

        mockEnemyType.soundComponent = _soundComponent;
        mockEnemyType.aiBody = gameObject;
        mockEnemyType.navMeshAgent = GetComponent<NavMeshAgent>();
        mockEnemyType.navMeshAgent.speed = mockEnemyType.speed;
        mockEnemyType.travelPoints = travelPoints;
        armSpawnPoint.GetComponent<EnemyArmBehaviour>().damage = mockEnemyType.damage;

        _stoppingDistance = mockEnemyType.stoppingDistance;

        _aiHealth.Init(mockEnemyType.health);

        _patrolState.OnInitState(mockEnemyType);
        _deadState.OnInitState(mockEnemyType);
        _followTargetState.OnInitState(mockEnemyType);
        _attackState.OnInitState(mockEnemyType);
        _attackState.SetTarget(GameManager.playerBaseRef); ;
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
            MakeFootStepsSounds();
            _currentState?.OnUpdate();
        }

    }

    private void MakeFootStepsSounds()
    {
        if (_navMeshAgent.velocity.magnitude > 1f)
        {
            _footStepsSound.StartWalking();
        }
        else
        {
            _footStepsSound.StopWalking();
        }
    }

    private void MakeDecision()
    {

        if (!_activeTargetInView)
        {
            ChangeState(_patrolState);
        }
        else if (_activeTargetInView && Vector3.Distance(transform.position, _currentTarget.position) <= _stoppingDistance)
        {
            ChangeState(_attackState);
        }
        else if (_activeTargetInView && Vector3.Distance(transform.position, _currentTarget.position) > _stoppingDistance)
        {
            ChangeState(_followTargetState);
        }

    }


    private void ChangeState(IState newState)
    {
        if (newState != _currentState && _canSwitchState)
        {
            _currentState?.OnExit();
            _currentState = newState;
            _currentState.OnEnter();
        }
    }


    public void FalseSwitchState()
    {
        _canSwitchState = false;
    }

    public void TrueSwitchState()
    {
        _canSwitchState = true;
    }


    public override void Death()
    {
        if (_alive)
        {
            SoundManager.instance.PlaySoundEffect(enemyType, _soundComponent, SoundEffects.Death);
            ChangeState(_deadState);
            base.Death();
            _alive = false;
            onEnemyDeath?.Invoke();
            CameraController.SlowMotion(0.2f);
            _enemyAnimations.Die();
            ragDollComponent.ActivateRagDoll();
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
        Debug.Log("<color=yellow>Base in View</color>");
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
                Destroy(_currentTarget.gameObject);
                _currentTarget = GameManager.playerRef.transform;
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

    #region prototype
    //TODO make it happends in attackState
    public void EnableArmCollider()
    {
        _attackState.EnableArmCollider();
    }

    public void DisableArmCollider()
    {
        _attackState.DisableArmCollider();
    }

    #endregion

}
