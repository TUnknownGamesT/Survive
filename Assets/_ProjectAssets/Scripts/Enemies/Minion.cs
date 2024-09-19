using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class Minion : MonoBehaviour, IDamageable
{

    public BoxCollider boxCollider;
    public float timeBetweenAttacks = 1f;
    public float damage;
    public static float cumulateDmg;
    public AIGroupBrain parent;
    public bool isAlive = true;

    public BoxCollider armCollider;

    public SkinnedMeshRenderer meshRenderer;


    private bool _timeToAttack = true;
    private CancellationTokenSource _cancellationTokenSource;

    private EnemyAnimations _enemyAnimations;

    private VisibleChecker _visibleChecker;

    private RagDollComponent _ragDollComponent;

    private float health;

    private Transform _currentTarget;

    private NavMeshAgent _navMeshAgent;

    void Awake()
    {
        _visibleChecker = GetComponent<VisibleChecker>();
        _enemyAnimations = GetComponent<EnemyAnimations>();
        _ragDollComponent = GetComponent<RagDollComponent>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void OnDisable()
    {
        parent.onTargetChange -= SetTarget;
    }

    void Update()
    {
        if (!isAlive && !_visibleChecker.isVisible)
        {
            Destroy(gameObject);
        }
        else
        {
            RotateTowardsTarget();
        }


    }

    private void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        boxCollider = GetComponent<BoxCollider>();
    }

    public void OnInitValues(EnemyType enemyType)
    {
        health = enemyType.health;
        damage = enemyType.damage;
        _navMeshAgent.speed = enemyType.speed;
        _navMeshAgent.angularSpeed = enemyType.damping;
        armCollider.GetComponent<EnemyArmBehaviour>().damage = damage;
    }


    private void RotateTowardsTarget()
    {
        if (_currentTarget != null)
        {
            Vector3 direction = (_currentTarget.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public void SetTarget(Transform target)
    {
        _currentTarget = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.LogWarning(other.gameObject.name);
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("PlayerBase")
            && _timeToAttack)
        {
            _enemyAnimations.Attack();
            _timeToAttack = false;
            Rest();
        }
    }

    private void Rest()
    {
        UniTask.Void(async () =>
        {
            _cancellationTokenSource = new CancellationTokenSource();
            await UniTask.Delay(TimeSpan.FromSeconds(timeBetweenAttacks), cancellationToken: _cancellationTokenSource.Token);
            _timeToAttack = true;
        });
    }

    public void SetParent(AIGroupBrain parent)
    {
        this.parent = parent;
        parent.onTargetChange += SetTarget;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            FactoryObjects.instance.CreateObject(new FactoryObject<Vector3>(FactoryObjectsType.Blood, transform.position));
            _enemyAnimations.Die();
            _ragDollComponent.ActivateRagDoll();
            GetComponent<NavMeshAgent>().enabled = false;
            boxCollider.enabled = false;
            armCollider.enabled = false;
            parent.Kill(this);
            this.enabled = false;
        }
    }

    private void DamageEffect()
    {
        UniTask.Void(async () =>
        {
            meshRenderer.material.color = Color.red;
            await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            meshRenderer.material.color = Color.white;
        });


    }

    public void EnableArmCollider()
    {
        armCollider.enabled = true;
    }

    public void DisableArmCollider()
    {
        armCollider.enabled = false;
    }
}
