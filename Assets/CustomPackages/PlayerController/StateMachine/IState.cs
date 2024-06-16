using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{

    public void OnInitState<T>(T gameObject);
    
    public void OnEnter();

    public void OnUpdate();

    public void OnExit();
}