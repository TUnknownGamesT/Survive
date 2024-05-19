using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpperBodyStateMachine : MonoBehaviour
{
    
    #region Singleton
    
    public static UpperBodyStateMachine instance;
    private void Awake()
    {
        instance = FindObjectOfType<UpperBodyStateMachine>();
        if (instance == null)
        {
            instance = this;
        }
    }
    
    #endregion
    
    
    public PlayerAnimation animation;

    private bool _hasGrenade = false;
    
    [Header("Guns")]
    private float elapsedTime;
    private int _currentGunIndex = 0;

    //States
    private IState _currentState;

    
    #region States
    
    readonly PlayerIdleState _idleState = new PlayerIdleState();

    #endregion
    
    protected void OnDisable()
    {
        
    }
    

    private void Start()
    {
        //animation = GetComponent<PlayerAnimation>();
        
        InitStates();
        
        ChangeState(_idleState);
        
    }

    private void Update()
    {
        _currentState?.OnUpdate();
    }


    private void InitStates()
    {
        _idleState.OnInitState(this);
        
    }
    
   
    private void ChangeState(IState newState)
    {
        if (_currentState != newState)
        {
            _currentState?.OnExit();
            _currentState = newState;
            _currentState?.OnEnter();
        }
    }
    
    public void Disable()
    {
        ChangeState(_idleState);
        enabled = false;
    }
}
