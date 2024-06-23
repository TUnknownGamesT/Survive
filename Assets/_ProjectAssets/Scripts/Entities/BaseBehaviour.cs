using System;
using UnityEngine;

public class BaseBehaviour : MonoBehaviour
{
    public static Action<float> onBaseHPCHnage;
    public static Action<int> onBaseMaxHealthChanged;
    public static Action onBaseDestroyed;
    
    public RefillAmoBehaviour refillAmoBehaviour;
    public HealStationBehaviour healStationBehaviour;
    public int baseMaxHP;
    
    private int baseCurrentHP;


    private void OnEnable()
    {
        BaseUpgrade.onBaseUpgraded += UpgradeBase;
        RepairBaseBehaviour.onBaseRepaired += HealBase;
    }
    
    private void OnDisable()
    {
        BaseUpgrade.onBaseUpgraded -= UpgradeBase;
        RepairBaseBehaviour.onBaseRepaired -= HealBase;
    }


    private void Start()
    {
        baseCurrentHP = baseMaxHP;
        onBaseMaxHealthChanged?.Invoke(baseMaxHP);
        onBaseHPCHnage?.Invoke(baseMaxHP);
    }

    private void UpgradeBase(BaseUpgradesOptions upgrade,float amount)
    {
        switch (upgrade)
        {
            case BaseUpgradesOptions.Amo:
                refillAmoBehaviour.refillAmo += (int) amount;
                break;
            case BaseUpgradesOptions.MadKit:
                healStationBehaviour.healAmount += (int) amount;
                break;
            case BaseUpgradesOptions.Wall:
                baseMaxHP += (int)amount;
                onBaseHPCHnage?.Invoke(baseMaxHP);
                break;
        }
    }
    
    
    private void HealBase(int amount)
    {
        if (baseCurrentHP + amount < baseMaxHP)
        {
            onBaseHPCHnage?.Invoke(amount);
            baseCurrentHP+= amount;   
        }
    }

    private void TakeDmg(int dmg)
    {
        baseCurrentHP -= dmg;
        onBaseHPCHnage?.Invoke(baseCurrentHP);
        if (baseCurrentHP <= 0)
        {
            onBaseDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyArm"))
        {
            Debug.LogWarningFormat("<color=reed>Get the same damage from all the enemies SOLVE</color>");
            TakeDmg(1);
        }
    }
}
