using System.Threading;
using UnityEngine;

public class AttackState : IState
{
    private Weapon _armPrefab;
    private GameObject _aiBody;
    private CancellationTokenSource _cts;
    private AnimationManager _enemyAnimations;
    private float _damping;
    private Transform _currentTarget;

    private float _pauseBetweenAttacks;
    private float _time;

    //TODO delete this after prototyping
    private bool _prototypeEnemy;
    private BoxCollider boxCollider;

    public void OnInitState<T>(T gameObject)
    {
        if (gameObject is EnemyType enemyType)
        {
            _aiBody = enemyType.aiBody;
            _damping = enemyType.damping;
            _prototypeEnemy = enemyType.prototypeEnemy;
            _pauseBetweenAttacks = enemyType.pauseBteweenAttacks;

            if (!_prototypeEnemy)
            {
                _armPrefab = enemyType.armPrefab.GetComponent<Weapon>();
                _armPrefab.transform.localPosition = Vector3.zero;
                _armPrefab.transform.localRotation = Quaternion.identity;
                _armPrefab.GetComponent<BoxCollider>().enabled = false;
                _armPrefab.GetComponent<Rigidbody>().useGravity = false;
            }
            else
            {
                boxCollider = enemyType.armSpawnPoint.GetComponent<BoxCollider>();
            }


            if (_aiBody.GetComponent<ZombieAnimationManager>())
                _enemyAnimations = _aiBody.GetComponent<ZombieAnimationManager>();
            else
                _enemyAnimations = _aiBody.gameObject.GetComponent<EnemyAnimations>();


        }
    }


    public void OnEnter()
    {
        Debug.Log(_currentTarget.name);
        _cts = new CancellationTokenSource();
    }

    public void OnUpdate()
    {
        if (!_prototypeEnemy)
        {
            _armPrefab.Tick(true);
        }
        else
        {
            if (_time + _pauseBetweenAttacks < Time.time)
            {
                _enemyAnimations.SetIdle(false);
                _time = Time.time;
                _enemyAnimations.Attack();
            }
            else
            {
                _enemyAnimations.SetIdle(true);
            }
        }

        RotateTowardThePlayer();
    }

    private void RotateTowardThePlayer()
    {
        var lookPos = _currentTarget.position - _aiBody.transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        _aiBody.transform.rotation = Quaternion.Slerp(_aiBody.transform.rotation, rotation, Time.deltaTime * _damping);
    }


    public void EnableArmCollider()
    {
        boxCollider.enabled = true;
    }

    public void DisableArmCollider()
    {
        boxCollider.enabled = false;
    }

    public void OnExit()
    {
        if (!_prototypeEnemy)
            _armPrefab.Tick(false);
        _cts.Cancel();
        _enemyAnimations.SetIdle(false);
        _enemyAnimations.ResetTriger("T_Attack");
        _enemyAnimations.SetIsWalking(true);
    }

    public void SetTarget(Transform target)
    {
        _currentTarget = target;
    }

    public void DropArm()
    {
        _armPrefab.GetComponent<BoxCollider>().enabled = true;
        _armPrefab.GetComponent<Rigidbody>().useGravity = true;
        _armPrefab.transform.SetParent(null);
    }
}
