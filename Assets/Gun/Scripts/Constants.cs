using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Constants : MonoBehaviour
{
   #region Singleton

   public static Constants instance;

   private void Awake()
   {
      instance = FindObjectOfType<Constants>();
      
      if (instance == null)
      {
         instance = this;
      }
   }

   #endregion


   public enum Resources
   {
      Engine,
      Pressure,
      ComputerBoard,
      None
   }

   public enum Upgrades
   {
      Player,
      Guns
   }
   
   public enum Tags
   {
      Player,
      Walls,
      Blocks
   }
   
   public enum EnemyType
   {
      Male=0,
      Pistol=1,
      AKA47=2,
      ShotGun=3,
      Sniper=4,
   }
   
   public enum GunsType
   {
      Pistol=0,
      AKA47=1,
      ShotGun=2,
      Sniper=3,
   }

   public static int PistolEnemyHealth = 1;
   public static int Ak47EnemyHealth = 1;
   public static int MaleEnemyHealth = 1;
   public static float wallPeacesMass = 1f;
   public static float doorPeacesMass = 1;
   public Material defaultMaterialBackPack;
   
   
   public GameObject pressECanvas;

   public AudioClip openDoorSound;

   [FormerlySerializedAs("gunnHighLight")] public Material highLightInteractable;
   [FormerlySerializedAs("gunnUnHighLight")] public Material unhighlightInteractable;

}
