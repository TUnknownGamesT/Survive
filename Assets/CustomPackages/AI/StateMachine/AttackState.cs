using System.Threading;
using UnityEngine;

public class AttackState : IState
{
    private Transform _armSpawnPoint;
    private float _spread;
    private bool _reloading;
    private Gun _armPrefab;
    private SoundComponent soundComponent;
    private GameObject _aiBody;
    private CancellationTokenSource _cts;
    private float _damping;
    private Transform _currentTarget;

    public void OnInitState<T>(T gameObject)
    {
        if(gameObject is EnemyType enemyType)
        {
            _armSpawnPoint = enemyType.armSpawnPoint;
            _aiBody = enemyType.aiBody;
            _damping = enemyType.damping;

            _armPrefab = Object.Instantiate(enemyType.armPrefab, _armSpawnPoint.position, Quaternion.identity, _armSpawnPoint.transform).GetComponent<Gun>();
            _armPrefab.transform.localPosition = Vector3.zero;
            _armPrefab.transform.localRotation = Quaternion.identity;
            _armPrefab.GetComponent<BoxCollider>().enabled = false;
            _armPrefab.GetComponent<Rigidbody>().useGravity = false;
        }
    }
    
    
    public void OnEnter()
    {
        _cts = new CancellationTokenSource();
    }

    public void OnUpdate()
    {
        if (_armPrefab.CanShoot())
        {
            _armPrefab.Shoot();
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
    
    
    public void OnExit()
    {
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
