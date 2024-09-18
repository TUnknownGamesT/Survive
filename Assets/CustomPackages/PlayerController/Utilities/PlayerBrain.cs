using System;
using ConstantsValues;
using UnityEngine;

public class PlayerBrain : MonoBehaviour
{
    #region Singleton/Refferences

    public static PlayerBrain instance;

    private void Awake()
    {
        instance = FindObjectOfType<PlayerBrain>();

        if (instance == null)
        {
            instance = this;
        }

        _playerRotation = GetComponent<PlayerRotation>();
        _upperBodyStateMachine = GetComponent<UpperBodyStateMachine>();
        _lowerBodyStateMachine = GetComponent<LowerBodyStateMachine>();
        _playerHealth = GetComponent<PlayerHealth>();
        _boxCollider = GetComponent<BoxCollider>();
    }


    #endregion

    private UpperBodyStateMachine _upperBodyStateMachine;
    private LowerBodyStateMachine _lowerBodyStateMachine;
    private PlayerRotation _playerRotation;
    private PlayerHealth _playerHealth;

    private BoxCollider _boxCollider;


    private void OnEnable()
    {
        PlayerHealth.onPlayerDeath += PlayerDeath;
        UpgradeInterpretor.onPlayerGetUpgrade += ApplyUpgrade;
    }

    private void OnDisable()
    {
        PlayerHealth.onPlayerDeath -= PlayerDeath;
        UpgradeInterpretor.onPlayerGetUpgrade -= ApplyUpgrade;
    }

    private void PlayerDeath()
    {
        _boxCollider.enabled = false;
        _upperBodyStateMachine.Disable();
        _lowerBodyStateMachine.Disable();
        _playerRotation.enabled = false;
    }

    public void RefillAmo(int amount)
    {
        _upperBodyStateMachine.RefillCurrentArmAmmo(amount);
    }

    public void Heal(int amount)
    {
        Debug.Log("Heal");
        _playerHealth.IncreaseLife(amount);
    }

    public void ApplyUpgrade(PlayerUpgradesOptions upgradeType, int amount)
    {
        switch (upgradeType)
        {
            case PlayerUpgradesOptions.Speed:

                _lowerBodyStateMachine.speed += CustomMath.GetPercentage(amount, (int)_lowerBodyStateMachine.speed);
                break;

            case PlayerUpgradesOptions.Life:
                _playerHealth.IncreaseMaxHealth(amount);
                break;

            //TODO Implement Shield Upgrade
            case PlayerUpgradesOptions.Shield:
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
