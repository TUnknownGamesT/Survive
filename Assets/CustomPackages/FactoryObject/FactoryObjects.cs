using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum FactoryObjectsType
{
    KillingText,
    Enemy,
    Blood,
    EnemyWeapon
}


public class FactoryObject<T>
{
    
    public FactoryObjectsType FactoryObjectType { get; }

    public T Instructions { get; }

    public FactoryObject(FactoryObjectsType factoryObjectType, T instructions)
    {
        FactoryObjectType = factoryObjectType;
        Instructions = instructions;
    }
}

public class FactoryObjects : MonoBehaviour
{

    #region Singleton

    public static FactoryObjects instance;
    

    private void Awake()
    {
        instance = FindObjectOfType<FactoryObjects>();
        if (instance == null)
        {
            instance = this;
        }
    }

    #endregion
    
    public static Action<Transform> onEnemySpawned;
    
    [Header("Objects")]
    public List<GameObject> enemiesToSpawn;
    public List<Weapon> enemyGuns;

    public GameObject blood;

    [Header("Text")] 
    [ColorUsage(true,true)]
    public List<Color> colors;
    public List<string> texts;

    private void OnEnable()
    {
        EnemyStatusManager.onEnemyDamageChanged += UpgradeEnemyArms;
    }
    
    private void OnDisable()
    {
        EnemyStatusManager.onEnemyDamageChanged -= UpgradeEnemyArms;
    }

    public void CreateObject<T>(FactoryObject<T> factoryObject)
    {
        switch (factoryObject.FactoryObjectType)
        {
           case FactoryObjectsType.Enemy:
               CreateEnemy(factoryObject.Instructions);
               break;
           case FactoryObjectsType.Blood:
               CreateBlood(factoryObject.Instructions);
               break;
           case FactoryObjectsType.EnemyWeapon:
               CreateEnemyEnemyWeapon(factoryObject.Instructions);
               break;
           default:
               Debug.LogWarning("FactoryObject type not found!");
               break;
        }
    }


    private void CreateBlood<T>(T position)
    {
        if(position is Collider collider)
            Instantiate(blood, collider.ClosestPointOnBounds(collider.transform.position), Quaternion.identity);
        if(position is Collision collision)
            Instantiate(blood, collision.contacts[0].point, Quaternion.identity);
    }

    private void CreateEnemyEnemyWeapon<T>(T instructions)
    {
        if (instructions is EnemyWeaponInstructions enemyDetails)
        {
            Instantiate(enemyGuns.Find(x => x.enemyType == enemyDetails.enemyType).gameObject,
                enemyDetails.parent.position, Quaternion.identity, enemyDetails.parent);
        }
    }
    
    private void UpgradeEnemyArms(float amount)
    {
        foreach (var gun in enemyGuns)
        {
            gun.damage += amount;
        }
    }
    
    
    #region CreateKillingText
    
    
    
    private string GetRandomText()
    {
        return texts[UnityEngine.Random.Range(0, texts.Count)];
    }
    
    private Color32 GetRandomColor()
    {
        return colors[UnityEngine.Random.Range(0, colors.Count)];
    }

    #endregion

    #region CreateEnemy

     private void CreateEnemy<T>(T positionToSpawn)
     {
         if (positionToSpawn is Transform position)
         {
            Transform  enemy = Instantiate(enemiesToSpawn[Random.Range(0,enemiesToSpawn.Count)], position.position , Quaternion.identity).transform;
            onEnemySpawned?.Invoke(enemy);
         }
     }

    #endregion
    
}
