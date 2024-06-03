using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraRotation : MonoBehaviour
{
    public GameObject camera;
    public Vector2 angleOffset = new Vector2();
    [SerializeField] private float moveTime;

    private bool lvlSelect = false;
    private bool isInLeanTween = false;

    private void Start()
    {
        lvlSelect = false;
        isInLeanTween = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInLeanTween) return;

        Vector3 CameraRotation = camera.transform.rotation.eulerAngles;
        if (!lvlSelect)
        {
            CameraRotation.x = (-Input.mousePosition.y / 100) + angleOffset.x;
            CameraRotation.y = (Input.mousePosition.x / 100) + angleOffset.y;
        }
       

        camera.transform.rotation = Quaternion.Euler(CameraRotation);
    }

   
}