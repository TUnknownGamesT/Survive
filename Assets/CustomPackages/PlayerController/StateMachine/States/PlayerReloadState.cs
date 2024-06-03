using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PlayerReloadState : IState
{

    public UpperBodyStateMachine upperBodyStateMachine;

    public void OnInitState<T>(T gameObject)
    {
        if(gameObject is GameObject player)
        {
            upperBodyStateMachine = player.GetComponent<UpperBodyStateMachine>();
        }
    }

    public void OnEnter()
    {
        upperBodyStateMachine.currentArm.Reload();
        UniTask.Delay(TimeSpan.FromSeconds(upperBodyStateMachine.currentArm.reloadTime)).ContinueWith(() =>
        {
            upperBodyStateMachine._reload = false;
        });
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
       
    }
}
