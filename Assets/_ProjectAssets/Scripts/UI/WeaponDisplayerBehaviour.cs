using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponDisplayerBehaviour : MonoBehaviour
{
    public TextMeshProUGUI currentAmoText;
    public Image armIcon;

    private Firearm gunn;

    private int _currentAmo;

    private int _rezervAmo;

    public void SetDisplayer(Firearm gunn)
    {
        armIcon.sprite = gunn.weaponIcon;
        ListenGun(gunn);
        SetAmoText(gunn.currentAmunition, gunn.rezervAmo);
    }


    public void SetAmoText(int currentAmo, int rezervAmo)
    {
        _currentAmo = currentAmo;
        _rezervAmo = rezervAmo;
        currentAmoText.text = $"<size=140%>{currentAmo}</size>/{rezervAmo}";
    }

    private void SetCurrentAmoUI()
    {
        _currentAmo--;
        currentAmoText.text = $"<size=140%>{_currentAmo}</size>/{_rezervAmo}";
    }

    private void ListenGun(Firearm newGunn)
    {
        if (gunn != null)
        {
            gunn.onShoot -= SetCurrentAmoUI;
            gunn.onFinishReload -= SetAmoText;
        }

        gunn = newGunn;

        gunn.onShoot += SetCurrentAmoUI;
        gunn.onFinishReload += SetAmoText;
    }

}
