using UnityEngine;

public class PlayerShootState : IState
{
    public Firearm currentArm;

    public void OnInitState<T>(T gameObject)
    {
        if(gameObject is GameObject player)
        {
            currentArm = player.GetComponent<UpperBodyStateMachine>().currentArm;
        }
    }

    public void OnEnter()
    {
        
    }

    public void OnUpdate()
    {
        currentArm.Shoot();
    }

    public void OnExit()
    {
        
    }
}
