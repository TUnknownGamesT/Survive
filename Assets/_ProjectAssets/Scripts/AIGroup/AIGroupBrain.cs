using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class AIGroupBrain : IAIBrain
{
    public static Action onEnemyDeath;

    #region Formation

    [HideInInspector]
    public FormationBase _formation;
    [SerializeField] private GameObject _unitPrefab;
    public float _unitSpeed = 2;

    public readonly List<Minion> _spawnedUnits = new List<Minion>();
    public List<Vector3> _points = new List<Vector3>();
    public Transform _parent;


    #endregion

    #region States

    WalkingStateGroup walkingStateGroup = new WalkingStateGroup();
    AttackStateGroup attackStateGroup = new AttackStateGroup();

    #endregion

    public float _stoppingDistance;
    private IState _currentState;
    private bool _activeTargetInView;
    private bool _alreadyNoticed;
    public Transform _currentTarget;
    public Transform basePoint;

    private void Awake()
    {
        _formation = GetComponent<FormationBase>();
        _parent = new GameObject("Unit Parent").transform;
    }

    private void Start()
    {
        _currentTarget = GameManager.playerBaseRef;
        _points = _formation.EvaluatePoints().ToList();

        if (_points.Count > _spawnedUnits.Count)
        {
            var remainingPoints = _points.Skip(_spawnedUnits.Count);
            Spawn(remainingPoints);
        }

        walkingStateGroup.OnInitState(this);
        attackStateGroup.OnInitState(this);

        ChangeState(walkingStateGroup);
    }

    private void Update()
    {
        _currentState?.OnUpdate();
        MakeDecision();
    }

    private void MakeDecision()
    {
        SetFormation();
        if (_alive)
        {

            if (_activeTargetInView &&
                Vector3.Distance(transform.position, _currentTarget.position) <= _stoppingDistance)
            {
                ChangeState(attackStateGroup);
            }
            else if (!_activeTargetInView)
            {
                ChangeState(walkingStateGroup);
            }
        }
    }


    private void SetFormation()
    {
        _points = _formation.EvaluatePoints().ToList();

        for (var i = 0; i < _spawnedUnits.Count; i++)
        {
            _spawnedUnits[i].transform.position = Vector3.MoveTowards(_spawnedUnits[i].transform.position,
                _currentTarget.position + _points[i], _unitSpeed * Time.deltaTime);
        }
        transform.position = _spawnedUnits.Aggregate(Vector3.zero, (acc, unit) => acc + unit.transform.position) / _spawnedUnits.Count;
    }

    private void ChangeState(IState newState)
    {
        if (_currentState != newState)
        {
            _currentState?.OnExit();
            _currentState = newState;
            _currentState?.OnEnter();
        }
    }

    private void Spawn(IEnumerable<Vector3> points)
    {
        foreach (var pos in points)
        {
            var unit = Instantiate(_unitPrefab, transform.position + pos, Quaternion.identity, _parent);
            unit.GetComponent<Minion>().SetParent(this);
            _spawnedUnits.Add(unit.GetComponent<Minion>());
        }
    }

    public void Kill(Minion minion)
    {
        if (_spawnedUnits.Contains(minion))
            _spawnedUnits.Remove(minion);
        if (_spawnedUnits.Count == 0)
        {
            base.Death();
            onEnemyDeath?.Invoke();
            Destroy(gameObject);
        }
    }


    public override void BaseInView(Transform basePoint)
    {
        this.basePoint = basePoint;
        _currentTarget = basePoint;
        _activeTargetInView = true;
        // _attackState.SetTarget(basePoint);
        // _followTargetState.SetTarget(basePoint);
    }

    public override void PlayerInView()
    {
        Debug.LogWarning("Player in view");
        _activeTargetInView = true;
        _currentTarget = GameManager.playerRef;
        // _attackState.SetTarget(GameManager.playerRef);
        // _followTargetState.SetTarget(GameManager.playerRef);
    }

    public override void PlayerOutOfView()
    {
        _currentTarget = GameManager.playerBaseRef;
        Debug.LogWarningFormat("<color=reed>Add last seen target not player every time SOLVE</color>");
        _activeTargetInView = false;
    }


}
