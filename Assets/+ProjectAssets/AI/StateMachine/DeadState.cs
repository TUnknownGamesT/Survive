using UnityEngine;

public class DeadState : IState
{
    
    public Transform transform;

    public void OnInitState<T>(T gameObject)
    {
        if (gameObject is Transform aiBrainTransform)
            transform = aiBrainTransform;

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
