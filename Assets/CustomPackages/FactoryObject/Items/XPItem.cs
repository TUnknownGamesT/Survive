using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XPItem : MonoBehaviour
{
   public float xpValue = 50;
   private void OnTriggerEnter(Collider other)
   {
      PlayerXP playerXp = other.GetComponent<PlayerXP>();
      if (playerXp != null)
      {
         playerXp.IncreaseXp(xpValue);
         Destroy(gameObject);
      }
   }
}
