using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MonoBehaviour, IState
{

    private AnimationManager _enemyAnimations;


    public void OnInitState<T>(T gameObject)
    {
        if (gameObject is IAIBrain aiBrain)
        {
            _enemyAnimations = aiBrain.GetComponent<AnimationManager>();
        }
    }


    public void OnEnter()
    {
        _enemyAnimations.SetIdle(true);
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {
        _enemyAnimations.SetIdle(false);
    }

}
