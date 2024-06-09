using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{

    public static Action<int> onPlayerHealthChanged;
    public static Action onPlayerDeath;

    public int currentHealth;

    private int maxLife;

    private void Start()
    {
        maxLife = currentHealth;
        onPlayerHealthChanged?.Invoke(maxLife);
    }

    public void TakeDamage(int damageReceived)
    {
        currentHealth += damageReceived;
        onPlayerHealthChanged?.Invoke(damageReceived);
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
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(-2);
        }
    }
}
