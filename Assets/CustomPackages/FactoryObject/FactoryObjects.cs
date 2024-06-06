using System.Collections.Generic;
using UnityEngine;

public enum FactoryObjectsType
{
    KillingText,
    Enemy
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
    
    [Header("Objects")]
    public GameObject enemyPrefab;

    [Header("Text")] 
    [ColorUsage(true,true)]
    public List<Color> colors;
    public List<string> texts;
    
    
    public void CreateObject<T>(FactoryObject<T> factoryObject)
    {
        switch (factoryObject.FactoryObjectType)
        {
           case FactoryObjectsType.Enemy:
               CreateEnemy(factoryObject.Instructions);
               break;
           
           default:
               Debug.LogWarning("FactoryObject type not found!");
               break;
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
             Instantiate(enemyPrefab, position.position , Quaternion.identity);
         }
     }

    #endregion
    
}
