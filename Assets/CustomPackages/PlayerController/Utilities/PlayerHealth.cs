using System;
using ConstantsValues;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{

    public static Action<float> onPlayerHealthChanged;
    public static Action onPlayerDeath;

    public float currentHealth;

    private float maxLife;

    private float _shield;


    private void Start()
    {
        maxLife = currentHealth;
        onPlayerHealthChanged?.Invoke(maxLife);
    }

    public void TakeDamage(float damageReceived)
    {
        float damage = damageReceived - CustomMath.GetPercentage(damageReceived, _shield);
        currentHealth -= damage;
        onPlayerHealthChanged?.Invoke(currentHealth);
        if (currentHealth <= 0)
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

    public void IncreaseMaxHealth(int amount)
    {
        maxLife += CustomMath.GetPercentage((int)maxLife, amount);
        maxLife += (int)Math.Round((double)(100 * amount) / maxLife);
        currentHealth = maxLife;
    }

    public void IncreaseShield(int amount)
    {
        _shield += CustomMath.GetPercentage(_shield, amount);
    }
}
