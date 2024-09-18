using System.Collections;
using System.Collections.Generic;
using TMPro;
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

   }

   #endregion
   public OptionsMenu OptionsMenu;

   public TextMeshProUGUI ambientVolumeText;
   public TextMeshProUGUI soundEffectVolumeText;


   void Start()
   {
      ambientVolumeText.text = $"Music - {OptionsMenu.AmbientMusicVolume * 100}%";
      soundEffectVolumeText.text = $"Effect - {OptionsMenu.SoundEffectVolume * 100}%";
   }

   public void SetAmbientVolume(float volume)
   {
      OptionsMenu.SetAmbientSound(volume);
      ambientVolumeText.text = $"Music - {OptionsMenu.AmbientMusicVolume * 100}%";
   }

   public void SetSoundEffectVolume(float volume)
   {
      OptionsMenu.SetSoundEffectVolume(volume);
      soundEffectVolumeText.text = $"Effect - {OptionsMenu.SoundEffectVolume * 100}%";
   }

}
