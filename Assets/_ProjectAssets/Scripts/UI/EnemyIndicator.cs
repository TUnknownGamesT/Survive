using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class EnemyIndicator : MonoBehaviour
{
    private Transform _target;
    private Quaternion tRot = Quaternion.identity;
    private Vector3 tPos = Vector3.zero;
    private RectTransform _rectTransform;


    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void RegisterTarget(Transform target)
    {
        _target = target;
    }

    private void Update()
    {
        RotateTowardsTarget();
        CheckIfTargetIsVisible();
    }
    
    
    private void CheckIfTargetIsVisible()
    {
        if(_target == null) return;
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(_target.position);
        if(screenPoint.z > 0 && screenPoint.x is > 0 and < 1 && screenPoint.y is > 0 and < 1)
            Destroy(gameObject);
    }
    
    private void RotateTowardsTarget()
    {
        if (_target == null) return;
        
        tPos = _target.position;

        Vector3 targetDir = Camera.main.transform.position - tPos;
        
        tRot = Quaternion.LookRotation(targetDir);
        tRot.z = -tRot.y;
        tRot.x = 0;
        tRot.y = 0;
        
        _rectTransform.localRotation = tRot;

    }
}
