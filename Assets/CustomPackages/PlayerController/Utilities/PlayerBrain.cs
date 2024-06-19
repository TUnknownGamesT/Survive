using System;
using UnityEngine;

public  class PlayerBrain : MonoBehaviour
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
        
        playerRotation = GetComponent<PlayerRotation>();
        upperBodyStateMachine = GetComponent<UpperBodyStateMachine>();
        lowerBodyStateMachine = GetComponent<LowerBodyStateMachine>();
        playerHealth = GetComponent<PlayerHealth>();
    }
    

    #endregion
    
    private UpperBodyStateMachine upperBodyStateMachine;
    private LowerBodyStateMachine lowerBodyStateMachine;
    private PlayerRotation playerRotation;
    private PlayerHealth playerHealth;


    private void OnEnable()
    {
        PlayerHealth.onPlayerDeath += PlayerDeath;
    }
    
    private void PlayerDeath()
    {
        upperBodyStateMachine.Disable();
        lowerBodyStateMachine.Disable();
        playerRotation.enabled = false;
    }

    public void RefillAmo(int amount)
    {
        upperBodyStateMachine.RefillCurrentArmAmmo(amount);
    }

    public void Heal(int amount)
    {
        Debug.Log("Heal");
        playerHealth.IncreaseLife(amount);
    }
}
