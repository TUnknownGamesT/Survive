using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackState : IState
{
    private float _spread;
    private bool _reloading;
    private Weapon _armPrefab;
    private SoundComponent soundComponent;
    private GameObject _aiBody;
    private CancellationTokenSource _cts;
    private ZombieAnimationManager _enemyAnimations;
    private float _damping;
    private Transform _currentTarget;

    public void OnInitState<T>(T gameObject)
    {
        if (gameObject is EnemyType enemyType)
        {
            _aiBody = enemyType.aiBody;
            _damping = enemyType.damping;
            _armPrefab = enemyType.armPrefab.GetComponent<Weapon>();
            _enemyAnimations = _aiBody.GetComponent<ZombieAnimationManager>();
            _armPrefab.transform.localPosition = Vector3.zero;
            _armPrefab.transform.localRotation = Quaternion.identity;
            _armPrefab.GetComponent<BoxCollider>().enabled = false;
            _armPrefab.GetComponent<Rigidbody>().useGravity = false;
        }
    }


    public void OnEnter()
    {
        _cts = new CancellationTokenSource();
        _enemyAnimations.Attack();
    }

    public void OnUpdate()
    {
        _armPrefab.Tick(Mouse.current.leftButton.isPressed);

        RotateTowardThePlayer();
    }

    private void RotateTowardThePlayer()
    {
        var lookPos = _currentTarget.position - _aiBody.transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        _aiBody.transform.rotation = Quaternion.Slerp(_aiBody.transform.rotation, rotation, Time.deltaTime * _damping);
    }


    public void OnExit()
    {
        _armPrefab.Tick(false);
        _cts.Cancel();
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
