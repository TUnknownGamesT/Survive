using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

public static class XPItemExtensions
{
   public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveInTargetLocalSpace(this Transform transform, Transform target, Vector3 targetLocalEndPosition, float duration)
   {
      var t = DOTween.To(
          () => transform.position - target.transform.position, // Value getter
          x => transform.position = x + target.transform.position, // Value setter
          targetLocalEndPosition,
          duration);
      t.SetTarget(transform);
      return t;
   }
}

public class XPItem : MonoBehaviour
{
   public float xpValue = 10;


   private void Start()
   {
      transform.DOMoveInTargetLocalSpace(GameManager.playerRef, Vector3.zero, 1f).SetEase(Ease.InElastic);
   }




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
