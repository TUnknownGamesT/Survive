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
      Melee=0,
      Pistol=1,
      AK=2,
      Shotgun=3,
      Sniper=4,
      Minion=5,
   }
      
   public enum GunsType
   {
      Pistol=0,
      AK=1,
      Shotgun=2,
      Sniper=3,
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


