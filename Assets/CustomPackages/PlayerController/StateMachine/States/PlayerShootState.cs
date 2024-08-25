using UnityEngine;

public class PlayerShootState : IState
{
    public Weapon currentArm;

    public void OnInitState<T>(T gameObject)
    {
        if (gameObject is GameObject player)
        {
            currentArm = player.GetComponent<UpperBodyStateMachine>().currentArm;
        }
    }

    public void OnEnter()
    {

    }

    public void OnUpdate()
    {
        currentArm.Tick(true);
    }

    public void OnExit()
    {
        currentArm.Tick(false);
    }
}
