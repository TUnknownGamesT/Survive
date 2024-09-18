using UnityEngine;
using UnityEngine.InputSystem;

public class UserInputController : MonoBehaviour
{

    private static PlayerInput _playerMovement;

    public static InputAction _mousePosition;
    public static InputAction _movementAction;
    public static InputAction _leftClick;
    public static InputAction _EAction;
    public static InputAction _pause;
    public static InputAction _reload;
    public static InputAction _rightClick;
    public static InputAction _throwGrenade;
    public static InputAction _mouseScrollY;

    private void Awake()
    {
        _playerMovement = new();
        _playerMovement.Enable();

        _mousePosition = _playerMovement.Player.MousePosition;
        _movementAction = _playerMovement.Player.Movement;
        _leftClick = _playerMovement.Player.LeftClick;
        _EAction = _playerMovement.Player.Interact;
        _pause = _playerMovement.Player.Pause;
        _reload = _playerMovement.Player.Reload;
        _rightClick = _playerMovement.Player.LookForward;
        _throwGrenade = _playerMovement.Player.ThrowGrenade;
        _mouseScrollY = _playerMovement.Player.MouseScroll;

    }

}
