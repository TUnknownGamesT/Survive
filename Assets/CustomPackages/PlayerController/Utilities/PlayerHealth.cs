using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour,IDamageable
{

    public static Action<float> onPlayerHealthChanged;
    public static Action onPlayerDeath;

    public float currentHealth;

    private float maxLife;

    private void Start()
    {
        maxLife = currentHealth;
        onPlayerHealthChanged?.Invoke(maxLife);
    }

    public void TakeDamage(float damageReceived)
    {
        currentHealth -= damageReceived;
        onPlayerHealthChanged?.Invoke(currentHealth);
        if (currentHealth<=0)
        { 
            CameraController.instance.TakeDamageEffect();
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

        onPlayerHealthChanged?.Invoke(amount);
    }

    private void IncreaseMaxHealth(int amount)
    {
        maxLife += amount;
        currentHealth = maxLife;
    }
}
