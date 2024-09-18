using System;
using UnityEngine;

namespace ConstantsValues
{

   public enum Resources
   {
      Engine,
      Pressure,
      ComputerBoard,
      None
   }

   public enum UpgradeType
   {
      Player,
      Guns,
      Base,
      Enemy
   }

   public enum Tags
   {
      Player,
      Walls,
      Blocks
   }

   public enum EnemyType
   {
      Mele = 0,
      Minion = 5,
      Golem = 6,
      Goblin = 7,
   }

   public enum GunsType
   {
      Pistol = 0,
      AK = 1,
      Shotgun = 2,
      Sniper = 3,
   }

   public enum CardClass
   {
      COMMON,
      RARE,
      EPIC,
   }

   public enum UpgradesType
   {
      Player,
      Guns,
   }

   public enum PlayerUpgradesOptions
   {
      Speed,
      Life,
      Shield,
   }

   public enum WeaponUpgradeOptions
   {
      Damage,
      FireRate,
      Spread,
   }


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
      public LayerMask baseLayer;

      public Color32 commonColor;
      public Color32 rareColor;
      public Color32 epicColor;

      public Texture2D pistolIcon;
      public Texture2D akIcon;
      public Texture2D shotgunIcon;

   }

   public class CustomMath
   {
      public static float GetPercentage(float total, float percentage)
      {
         return (float)Math.Round((double)percentage / 100 * total, 3);
      }
   }
}


