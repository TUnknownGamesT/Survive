using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMenuBehaviour : MonoBehaviour
{

   #region Singleton

   public static OptionMenuBehaviour instance;
   
   private void Awake()
   {
      instance = FindObjectOfType<OptionMenuBehaviour>();

      if (instance == null)
      {
         instance = this;
      }
      
      canvas = GetComponent<Canvas>();
      graphicRaycaster = GetComponent<GraphicRaycaster>();

      ambientVolumeSlider.value = OptionsMenu.AmbientMusicVolume;
      soundEffectsVolumeSlider.value = OptionsMenu.SoundEffectVolume;
   }

   #endregion
   
   public Slider ambientVolumeSlider;
   public Slider soundEffectsVolumeSlider;
   public OptionsMenu OptionsMenu;

   private Canvas canvas;
   private GraphicRaycaster graphicRaycaster;
   
   
   public void AmbientVolumeChange()
   {
      OptionsMenu.SetAmbientSound(ambientVolumeSlider.value);
   }

   public void SoundEffectChange()
   {
      OptionsMenu.SetSoundEffectVolume(soundEffectsVolumeSlider.value);
   }
   
   public void DisableMainMenu()
   {
      canvas.enabled = false;
      graphicRaycaster.enabled = false;
   }

   public void EnableMainMenu()
   {
      canvas.enabled = true;
      graphicRaycaster.enabled = true;
   }
   
}
