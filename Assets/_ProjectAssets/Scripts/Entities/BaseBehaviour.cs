using System;
using UnityEngine;

public class BaseBehaviour : MonoBehaviour
{
    public static Action<float> onBaseHPCHnage;
    public static Action onBaseDestroyed;
    
    public RefillAmoBehaviour refillAmoBehaviour;
    public HealStationBehaviour healStationBehaviour;
    
    public float baseHP;


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
                baseHP += amount;
                onBaseHPCHnage?.Invoke(baseHP);
                break;
        }
    }
    
    
    private void HealBase(int amount)
    {
        onBaseHPCHnage?.Invoke(amount);
        baseHP += amount;
    }

    private void TakeDmg(int dmg)
    {
        baseHP -= dmg;
        onBaseHPCHnage?.Invoke(-dmg);
        if (baseHP <= 0)
        {
            onBaseDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Debug.Log(other.name);
            TakeDmg(1);
        }
    }
}
