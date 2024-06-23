using UnityEngine;
using UnityEngine.UI;

public class MeleWeaponDisplayer : MonoBehaviour, IWeaponDisplayer
{
    public Image armIcon;
    public Weapon arm;
    private CanvasGroup _canvasGroup;
    
    
    // Start is called before the first frame update
    void Start()
    { 
        armIcon.sprite = arm.GetComponent<Weapon>().weaponIcon;
        _canvasGroup = GetComponent<CanvasGroup>();        
    }

    
    public void SetDisplayer(Weapon gun)
    {
        armIcon.sprite = gun.weaponIcon;
        Color c = armIcon.color;
        c.a = 1;
        armIcon.color = c;
    }

    public void Activate()
    {
        GetComponent<RectTransform>().localScale = new Vector3(0.7f,0.7f,0.7f);
        _canvasGroup.alpha = 1;
    }
    
    public void Deactivate()
    {
        GetComponent<RectTransform>().localScale = new Vector3(0.5f,0.5f,0.5f);
        _canvasGroup.alpha = 0.27f;
    }
}
