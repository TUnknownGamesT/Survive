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
      Zombie = 7,
   }

   public enum GunsType
   {
      Pistol = 0,
      AK = 1,
      Shotgun = 2,
      Sniper = 3,
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
   }
}


