using UnityEngine;

public class DeadState : IState
{
    
    public Transform transform;

    public void OnInitState<T>(T gameObject)
    {
        if (gameObject is EnemyType aiBrainTransform)
            transform = aiBrainTransform.aiBody.transform;

    }

    public void OnEnter()
    {
        transform.GetComponent<BoxCollider>().enabled = false;
    }

    public void OnUpdate()
    {
        
    }

    public void OnExit()
    {
        
    }
}
