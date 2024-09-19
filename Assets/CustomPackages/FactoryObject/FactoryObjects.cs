using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Constants = ConstantsValues;

public enum FactoryObjectsType
{
    DamageText,
    Enemy,
    Blood,
    EnemyWeapon,
    XPItem
}

public struct BloodInstructions
{
    public Vector3 Position;
    public Constants.EnemyType enemyType;

    public BloodInstructions(Vector3 position, Constants.EnemyType enemyType)
    {
        Position = position;
        this.enemyType = enemyType;
    }
}

public struct DamageTextInstructions
{
    public Vector3 Position;
    public int Value;

    public DamageTextInstructions(Vector3 position, int value)
    {
        Position = position;
        Value = value;
    }
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

    public GameObject golemBlood;
    public GameObject xpItem;

    public TextPopUpBehaviour damageText;

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
                Debug.LogWarning("EnemyWeapon type not found!");
                // CreateEnemyWeapon(factoryObject.Instructions);
                break;
            case FactoryObjectsType.XPItem:
                CreateXpItem(factoryObject.Instructions);
                break;
            case FactoryObjectsType.DamageText:
                CreateDamageText(factoryObject.Instructions);
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
        if (position is Vector3 castedPosition)
            Instantiate(blood, castedPosition, Quaternion.identity);

        if (position is BloodInstructions bloodInstructions)
        {
            switch (bloodInstructions.enemyType)
            {
                case Constants.EnemyType.Golem:
                    Instantiate(golemBlood, bloodInstructions.Position, Quaternion.identity);
                    break;
                case Constants.EnemyType.Minion:
                    Instantiate(blood, bloodInstructions.Position, Quaternion.identity);
                    break;
                case Constants.EnemyType.Goblin:
                    Instantiate(blood, bloodInstructions.Position, Quaternion.identity);
                    break;
                default:
                    break;
            }

        }
    }

    // private void CreateEnemyWeapon<T>(T instructions)
    // {
    //     if (instructions is EnemyWeaponInstructions enemyDetails)
    //     {
    //         Instantiate(enemyGuns.Find(x => x.enemyType == enemyDetails.enemyType).gameObject,
    //             enemyDetails.parent.position, Quaternion.identity, enemyDetails.parent);
    //     }
    // }

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


    #region CreateDamageText
    private void CreateDamageText<T>(T value)
    {
        if (value is DamageTextInstructions instructions)
        {
            TextPopUpBehaviour text = Instantiate(damageText, instructions.Position, damageText.transform.rotation);
            text.SetTextAndColor(Color.white, instructions.Value.ToString());
        }
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
