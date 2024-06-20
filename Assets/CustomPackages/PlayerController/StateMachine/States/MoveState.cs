using UnityEngine;

public class MoveState : IState
{
    private CharacterController _characterController;
    private FootStepsSound _footStepsSound;
    private AnimationManager _animationManager;
    private float _speed;
    
    public void OnInitState<T>(T gameObject)
    {
        if(gameObject is GameObject player)
        {
            _footStepsSound = player.GetComponent<FootStepsSound>();
            _characterController = player.GetComponent<CharacterController>();
            _speed = player.GetComponent<LowerBodyStateMachine>().speed;
            _animationManager = player.GetComponent<AnimationManager>();
        }
    }

    public void OnEnter()
    {
        
    }

    public void OnUpdate()
    {
        Vector2 direction = UserInputController._movementAction.ReadValue<Vector2>();
        _characterController.Move(new Vector3(direction.x, 0, direction.y)*Time.deltaTime *_speed);
        if (direction.magnitude != 0)
        {
            MakeFootStepsSound();
            _animationManager?.SetSpeed(_speed);
        }
        else
        {
            StopMakingFootStepsSound();
            _animationManager?.SetSpeed(0);
        }
        
    }

    public void OnExit()
    {
       
    }
    
    private void MakeFootStepsSound()
    {
        _footStepsSound.StartWalking();
    }
    
    private void StopMakingFootStepsSound()
    {
        _footStepsSound.StopWalking();
    }
    
}

