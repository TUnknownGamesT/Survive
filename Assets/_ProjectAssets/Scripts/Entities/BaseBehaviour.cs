using System;
using UnityEngine;

public class BaseBehaviour : MonoBehaviour, IDamageable
{
    public static Action<float> onBaseHPCHnage;
    public static Action<int> onBaseMaxHealthChanged;
    public static Action onBaseDestroyed;

    public RefillAmoBehaviour refillAmoBehaviour;
    public HealStationBehaviour healStationBehaviour;
    public int baseMaxHP;

    private float baseCurrentHP;


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

    private void UpgradeBase(BaseUpgradesOptions upgrade, float amount)
    {
        switch (upgrade)
        {
            case BaseUpgradesOptions.Amo:
                refillAmoBehaviour.refillAmo += (int)amount;
                break;
            case BaseUpgradesOptions.MadKit:
                healStationBehaviour.healAmount += (int)amount;
                break;
            case BaseUpgradesOptions.Wall:
                baseMaxHP += (int)amount;
                onBaseHPCHnage?.Invoke(baseMaxHP);
                break;
        }
    }


    private void HealBase(int amount)
    {
        if (baseCurrentHP + amount <= baseMaxHP)
        {
            baseCurrentHP += amount;
            onBaseHPCHnage?.Invoke(baseCurrentHP);
        }
    }

    public void TakeDamage(float dmg)
    {
        baseCurrentHP -= dmg;
        onBaseHPCHnage?.Invoke(baseCurrentHP);
        if (baseCurrentHP <= 0)
        {
            onBaseDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
}
