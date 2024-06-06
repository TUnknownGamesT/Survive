using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GrenadeUIBheaviour : MonoBehaviour
{
    private Slider _slider;
    private bool prepareGrenade;
    private float time;
    private void Awake()
    {
        _slider = GetComponent<Slider>();
    }
    

    private void OnDisable()
    {
        UserInputController._throwGrenade.performed -= ChargePower;
        UserInputController._throwGrenade.canceled -= Release;
    }

    private void Start()
    {
        UserInputController._throwGrenade.performed += ChargePower;
        UserInputController._throwGrenade.canceled += Release;
    }

    private void Update()
    {
        if (prepareGrenade)
        {
            _slider.value = Mathf.Abs(Mathf.Sin(Time.time-time))*90;
        }
    }


    private void ChargePower(InputAction.CallbackContext callbackContext)
    {
        time = Time.time;
        prepareGrenade = true;
        Debug.Log("Perform");
       
    }

    private void Release(InputAction.CallbackContext callbackContext)
    {
        prepareGrenade = false;
        _slider.value = 0;
    }
}
