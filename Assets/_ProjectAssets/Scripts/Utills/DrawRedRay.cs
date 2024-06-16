using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawRedRay : MonoBehaviour
{

    public float maxDistance;
    public LayerMask layerMask;

    private float relatimeDistance;
    private LineRenderer _lineRenderer;


    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(transform.position,transform.forward, out RaycastHit hit, maxDistance,layerMask))
        {
            relatimeDistance = Vector3.Distance(transform.position, GameManager.playerRef.position);
            Vector3 rayFarthestPoint = _lineRenderer.GetPosition(1);
            rayFarthestPoint.z = relatimeDistance;
            _lineRenderer.SetPosition(1, rayFarthestPoint);
        }else
        {
            Vector3 rayFarthestPoint = _lineRenderer.GetPosition(1);
            rayFarthestPoint.z = maxDistance;
            _lineRenderer.SetPosition(1, rayFarthestPoint);
        }
    }
}
