using System;
using ConstantsValues;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{

    public static Action<float> onPlayerHealthChanged;
    public static Action onPlayerDeath;

    public float currentHealth;

    private float maxLife;

    void OnEnable()
    {

    }


    private void Start()
    {
        maxLife = currentHealth;
        onPlayerHealthChanged?.Invoke(maxLife);
    }

    public void TakeDamage(float damageReceived)
    {
        currentHealth -= damageReceived;
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
}
