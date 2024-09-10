using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum FactoryObjectsType
{
    KillingText,
    Enemy,
    Blood,
    EnemyWeapon,
    XPItem
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
    public GameObject xpItem;

    [Header("Text")]
    [ColorUsage(true, true)]
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
                CreateEnemyWeapon(factoryObject.Instructions);
                break;
            case FactoryObjectsType.XPItem:
                CreateXpItem(factoryObject.Instructions);
                break;
            default:
                Debug.LogWarning("FactoryObject type not found!");
                break;
        }
    }


    private void CreateBlood<T>(T position)
    {
        if (position is Collider collider)
            Instantiate(blood, collider.ClosestPointOnBounds(collider.transform.position), Quaternion.identity);
        if (position is Collision collision)
            Instantiate(blood, collision.contacts[0].point, Quaternion.identity);
    }

    private void CreateEnemyWeapon<T>(T instructions)
    {
        if (instructions is EnemyWeaponInstructions enemyDetails)
        {
            Instantiate(enemyGuns.Find(x => x.enemyType == enemyDetails.enemyType).gameObject,
                enemyDetails.parent.position, Quaternion.identity, enemyDetails.parent);
        }
    }

    private void CreateXpItem<T>(T position)
    {
        if (position is Vector3 castedPosition)
            Instantiate(xpItem, castedPosition, Quaternion.identity);
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

    private void CreateEnemy<T>(T enemyInstructions)
    {
        if (enemyInstructions is EnemyInstructions instructions)
        {
            Transform enemy = Instantiate(instructions.enemyPrefab, instructions.Position, Quaternion.identity).transform;
            onEnemySpawned?.Invoke(enemy);
        }
    }

    #endregion

}
