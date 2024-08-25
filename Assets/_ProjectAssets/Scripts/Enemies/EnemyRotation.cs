using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class EnemyRotation : MonoBehaviour
{
    public float damping;
    public Vector2 rotationAngles;
    public float pauseBetweenRotation;

    private bool _staringAtPlayer;
    private float _angle;
    private bool _rotationDirection;
    private Quaternion _rotation;
    private float _rotationVelocity;
    private CancellationTokenSource _cts;
    private Vector2 _dynamicRotation;

    public void PlayerInView()
    {
        _cts.Cancel();
        _staringAtPlayer = true;
    }

    public void PlayerOutOfView()
    {
        _cts = new CancellationTokenSource();
        _staringAtPlayer = false;
    }

    private void Start()
    {
        _cts = new CancellationTokenSource();
    }

    // Update is called once per frame
    void Update()
    {
        if (_staringAtPlayer)
        {
            RotateTowardThePlayer();
        }

    }

    public void StopLookingAround()
    {
        _cts.Cancel();
        _cts = new CancellationTokenSource();
    }

    public void StartLookingAround()
    {

        _cts = new CancellationTokenSource();

        _dynamicRotation.x = transform.rotation.eulerAngles.y + rotationAngles.x;
        _dynamicRotation.y = transform.rotation.eulerAngles.y - rotationAngles.y;

        // NailAngle(ref _dynamicRotation.x);
        // NailAngle(ref _dynamicRotation.y);

        _angle = _dynamicRotation.x;

        RotateAround();
    }

    public void PlayerDeath()
    {
        _cts.Cancel();
        _staringAtPlayer = false;
    }


    private void RotateTowardThePlayer()
    {
        var lookPos = GameManager.playerRef.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
    }

    private void RotateAround()
    {
        UniTask.Void(async () =>
        {
            try
            {
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _angle, ref _rotationVelocity, 1f);
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, angle, transform.eulerAngles.z);
                if (Math.Abs(ReturnIn360Range(_angle) - transform.rotation.eulerAngles.y) < 3)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(pauseBetweenRotation), cancellationToken: _cts.Token);
                    _angle = _rotationDirection ? _dynamicRotation.y : _dynamicRotation.x;
                    _rotationDirection = !_rotationDirection;
                }

                await UniTask.Delay(TimeSpan.FromSeconds(0), cancellationToken: _cts.Token);

                RotateAround();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Debug.Log("Thread miss reference");
            }

        });
    }


    private void NailAngle(ref float angleToNail)
    {
        while (angleToNail > 180)
            angleToNail -= 180;
        while (angleToNail < -180)
            angleToNail += 180;
    }

    private float ReturnIn360Range(float angle)
    {
        while (angle > 360)
            angle -= 360;
        while (angle < 0)
            angle += 360;

        return angle;
    }

}