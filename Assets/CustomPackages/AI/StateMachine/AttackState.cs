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

    private BoxCollider boxCollider;

    public void OnInitState<T>(T gameObject)
    {
        if (gameObject is EnemyType enemyType)
        {
            _aiBody = enemyType.aiBody;
            _damping = enemyType.damping;
            _pauseBetweenAttacks = enemyType.pauseBteweenAttacks;
            boxCollider = enemyType.armSpawnPoint.GetComponent<BoxCollider>();
            _enemyAnimations = _aiBody.gameObject.GetComponent<EnemyAnimations>();
        }
    }


    public void OnEnter()
    {
        _cts = new CancellationTokenSource();
    }

    public void OnUpdate()
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
