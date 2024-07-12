using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateGroup : IState
{
    
    List<Minion> _spawnedUnits = new List<Minion>();
    
    
    public void OnInitState<T>(T gameObject)
    {
        if(gameObject is AIGroupBrain aiGroupBrain) {
            _spawnedUnits = aiGroupBrain._spawnedUnits;
        }
    }

    public void OnEnter()
    {
        
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
        
    }
    
}
