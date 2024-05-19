using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController), typeof(FootStepsSound))]
public class LowerBodyStateMachine : MonoBehaviour
{
    
    public float speed;
    
    private CharacterController _characterController;
    private FootStepsSound _footStepsSound;

    private IState _currentState;

    #region States

    readonly MoveState _moveState = new MoveState();
    readonly PlayerIdleState _idleState = new PlayerIdleState();

    #endregion
    
    private void Awake()
    {
        _footStepsSound = GetComponent<FootStepsSound>();
        _characterController = GetComponent<CharacterController>();
    }


    private void OnEnable()
    {

    }

    void Start()
    {
        _moveState.OnInitState(gameObject);
        
        ChangeState(_moveState);
    }
    
    void Update()
    {
        _currentState?.OnUpdate();
    }

    private void ChangeState(IState newState)
    {
        if(newState !=_currentState)
        {
            _currentState?.OnExit();
            _currentState = newState; 
            _currentState?.OnEnter();
        }
    }

    private void IncreaseSpeed(float amount)
    {
        speed += amount;
        _moveState.OnInitState(gameObject);
    }
    
    public void Disable()
    {
        ChangeState(_idleState);
        enabled = false;
    }
}
