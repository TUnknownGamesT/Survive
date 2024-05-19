using System;
using UnityEngine;

public  class PlayerBrain : MonoBehaviour
{
    #region Singleton
    
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
    }
    

    #endregion
    
    private UpperBodyStateMachine upperBodyStateMachine;
    private LowerBodyStateMachine lowerBodyStateMachine;
    private PlayerRotation playerRotation;


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
}
