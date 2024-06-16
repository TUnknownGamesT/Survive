using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class GravityForCharacterController : MonoBehaviour
{
    private float _gravity = -9.18f;
    private float _gravityMultiplier = 3f;
    private CharacterController controller;
    private Vector3 _direction = Vector3.zero;

    private float _velocity;
    
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {   
        ApplyGravity();
    }


    private void ApplyGravity()
    {
        if (controller.isGrounded && _velocity < 0)
            _velocity = -1f;
        else
            _velocity += _gravity * _gravityMultiplier * Time.deltaTime;
        
        _direction.y = _velocity; 
        
        controller.Move(_direction * Time.deltaTime);
    }
}
