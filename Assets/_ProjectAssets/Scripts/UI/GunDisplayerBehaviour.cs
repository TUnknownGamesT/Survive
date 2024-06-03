using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunDisplayerBehaviour : MonoBehaviour
{
    public TextMeshProUGUI currentAmoText;
    public TextMeshProUGUI totalAmmunition;
    public Gun gunn;
    public Image armIcon;
    private int totalAmmo;
    
    private CanvasGroup _canvasGroup;


    private void OnEnable()
    {
        if (gunn != null)
        {
            gunn.onShoot += SetCurrentAmoUI;
            gunn.onFinishReload += SetAmoText;
        }
        
    }
    
    private void OnDisable()
    {
        if (gunn!= null)
        {
            gunn.onShoot -= SetCurrentAmoUI;
            gunn.onFinishReload -= SetAmoText;
        }
      
    }
     
    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        if(gunn!=null)
            SetAmoText(gunn.currentAmunition, gunn.rezervAmo);
    }

    public void SetGunDisplayer(GameObject gun)
    {
        Gun gunScript = gun.GetComponent<Gun>();
        gunn = gunScript;
        armIcon.sprite = gunScript.weaponIcon;
        Color c = armIcon.color;
        c.a = 1;
        armIcon.color = c;
        SetAmoText(gunn.currentAmunition, gunn.rezervAmo);
        ListenGun(gunScript);
    }
    
    public void SetAmoText(int currentAmo, int rezervAmo)
    {
        currentAmoText.text = $"{currentAmo}/{rezervAmo}";
        totalAmmo = currentAmo + rezervAmo;
    }
    
    public void Activate()
    {
        GetComponent<RectTransform>().localScale = new Vector3(0.7f,0.7f,0.7f);
        _canvasGroup.alpha = 1;
        totalAmmunition.gameObject.SetActive(false);
        currentAmoText.gameObject.SetActive(true);
    }
    
    public void Deactivate()
    {
        GetComponent<RectTransform>().localScale = new Vector3(0.5f,0.5f,0.5f);
        _canvasGroup.alpha = 0.27f;
        currentAmoText.gameObject.SetActive(false);
        totalAmmunition.gameObject.SetActive(true);
        totalAmmunition.text = totalAmmo.ToString();
        
    }
    
    private void SetCurrentAmoUI()
    {
        string[] split = currentAmoText.text.Split('/');
        int amo = int.Parse(split[0]);
        amo--;
        currentAmoText.text = $"{amo}/{split[1]}";
    }

    private void ListenGun(Gun newGunn)
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
