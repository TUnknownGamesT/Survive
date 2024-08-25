using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class UpperBodyStateMachine : MonoBehaviour
{

    #region Singleton

    public static UpperBodyStateMachine instance;
    private void Awake()
    {
        instance = FindObjectOfType<UpperBodyStateMachine>();
        if (instance == null)
        {
            instance = this;
        }
    }

    #endregion

    [HideInInspector]
    public Weapon currentArm;
    public PlayerAnimationsManager animation;
    public Transform armSpawnPoint;

    //private bool _hasGrenade = false;

    [Header("Guns")]
    private float elapsedTime;
    public float changeWeaponScrollDuration;
    public List<GameObject> guns;
    private int _currentGunIndex = 0;

    [HideInInspector]
    public bool _reload;
    private bool _shoot;

    //States
    private IState _currentState;


    #region States

    readonly PlayerIdleState _idleState = new PlayerIdleState();
    readonly PlayerShootState _shootState = new PlayerShootState();
    readonly PlayerReloadState _reloadState = new PlayerReloadState();

    #endregion

    protected void OnDisable()
    {
        UserInputController._leftClick.started -= StartShooting;
        UserInputController._leftClick.canceled -= StopShooting;
        UserInputController._reload.started -= Reload;
        UserInputController._mouseScrollY.performed -= SwitchGunFromInventory;
    }


    private void Start()
    {
        UserInputController._leftClick.started += StartShooting;
        UserInputController._leftClick.canceled += StopShooting;
        UserInputController._reload.started += Reload;
        UserInputController._mouseScrollY.performed += SwitchGunFromInventory;

        animation = GetComponent<PlayerAnimationsManager>();

        InitStates();


        ChangeState(_idleState);
        ChangeArm(guns[0]);
    }

    private void Update()
    {
        if (_shoot && !_reload && currentArm != null)
        {
            ChangeState(_shootState);

        }
        else if (_reload)
        {
            ChangeState(_reloadState);
        }
        else
        {
            ChangeState(_idleState);
        }

        _currentState?.OnUpdate();
    }


    private void InitStates()
    {
        _idleState.OnInitState(this);
        _shootState.OnInitState(currentArm);
        _reloadState.OnInitState(currentArm);

    }


    private void ChangeState(IState newState)
    {
        if (_currentState != newState)
        {
            _currentState?.OnExit();
            _currentState = newState;
            _currentState?.OnEnter();
        }
    }



    private void StartShooting(InputAction.CallbackContext obj)
    {
        _shoot = true;
    }

    private void StopShooting(InputAction.CallbackContext obj)
    {

        _shoot = false;
    }

    private void Reload(InputAction.CallbackContext obj)
    {
        _reload = true;
    }

    private void SwitchGunFromInventory(InputAction.CallbackContext obj)
    {
        float scrollDirection = obj.ReadValue<float>();
        elapsedTime += Time.deltaTime;

        if (elapsedTime > changeWeaponScrollDuration)
        {
            if (scrollDirection > 0)
            {
                _currentGunIndex++;
                if (_currentGunIndex >= guns.Count)
                {
                    _currentGunIndex = 0;
                }
            }
            else if (scrollDirection < 0)
            {
                _currentGunIndex--;
                if (_currentGunIndex < 0)
                {
                    _currentGunIndex = guns.Count - 1;
                }
            }
            elapsedTime = 0;
        }

        UIManager.instance.ChangeWeaponIcon(_currentGunIndex);
        ChangeArm(guns[_currentGunIndex]);
    }

    private void ChangeArm(GameObject arm)
    {
        if (currentArm != null)
        {
            currentArm.gameObject.transform.SetParent(null);
            currentArm.gameObject.transform.localPosition = Vector3.zero;
            currentArm.GetComponent<Rigidbody>().isKinematic = false;
            currentArm.GetComponent<BoxCollider>().enabled = true;
        }

        currentArm = arm.GetComponent<Weapon>();
        currentArm.GetComponent<Rigidbody>().isKinematic = true;
        currentArm.GetComponent<BoxCollider>().enabled = false;

        var transform1 = currentArm.transform;
        transform1.parent = armSpawnPoint;
        transform1.localPosition = Vector3.zero;
        transform1.localRotation = Quaternion.identity;



        _shootState.OnInitState(gameObject);
        _reloadState.OnInitState(gameObject);
        animation?.SetWeaponType((int)currentArm.enemyType);
        currentArm.SetArmHandler(animation);
    }

    public void RefillCurrentArmAmmo(int amount)
    {
        currentArm.RefillAmmunition(amount);
    }

    public void Disable()
    {
        ChangeState(_idleState);
        enabled = false;
    }
}
