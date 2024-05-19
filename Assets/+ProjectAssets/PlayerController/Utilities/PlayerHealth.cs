using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public static Action<int> onPlayerGetDamage;
    public static Action<int> onPlayerGetHeal;
    public static Action onPlayerDeath;

    public float currentHealth;

    private float maxLife;


    private void OnEnable()
    {
       
    }
    
    private void OnDisable()
    {
        
    }

    private void Start()
    {
       
        maxLife = currentHealth;
    }

    public void TakeDamage(int damageReceived)
    {
        currentHealth -= damageReceived;
        onPlayerGetDamage?.Invoke(damageReceived);
        if (currentHealth<=0)
        {
            Cursor.visible = true;
            onPlayerDeath?.Invoke();
        }
    }

    public void IncreaseLife(int amount)
    {
        if (currentHealth + amount > maxLife)
        {
            currentHealth = maxLife;
        }
        else
        {
            currentHealth += amount;
        }

        onPlayerGetHeal?.Invoke(amount);
    }

    private void IncreaseMaxHealth(int amount)
    {
        maxLife += amount;
        currentHealth = maxLife;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(2);
        }
    }
}
