using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public enum EnemyUpgradesOptions
{
    Speed,
    Damage,
    Health
}

public class EnemyUpgrades : MonoBehaviour
{
   public static Action<EnemyUpgradesOptions,float> onEnemyUpgraded;
   public static Action onBaseUpgradeSelected;

    
    [Header("References")]
    public TextMeshProUGUI upgradeText;
    public Image upgradeImage;
    public Button upgradeButton;
    public AudioClip upgradeSound;
    public SoundComponent soundComponent;
    public GameObject title;

    [Header("Items Images")]
    public Sprite speedImage;
    public Sprite healthImage;
    public Sprite damageImage;
    
    [Header("Upgrades")]
    public float speedUpgrade;
    public float healthUpgrade;
    public float damageUpgrade;

    private readonly List<EnemyUpgradesOptions> _enemyUpgradesOptions = new ()
    {
        EnemyUpgradesOptions.Speed,EnemyUpgradesOptions.Health,EnemyUpgradesOptions.Damage
    };
    private float _upgradedStatus;
    private EnemyUpgradesOptions _randomUpgrade;
    
    private void Awake()
    {

    }
    
    private void OnEnable()
    {
        
        UpgradePanelBehaviour.onSecondaryCardDisappear += AddCardUpgradeAttribute;
        UpgradePanelBehaviour.onPanelDisappear += SwitchCardToIdle;
    }
    
    private void OnDisable()
    {
        UpgradePanelBehaviour.onSecondaryCardDisappear -= AddCardUpgradeAttribute;
        UpgradePanelBehaviour.onPanelDisappear -= SwitchCardToIdle;
    }

    public void GetRandomUpgrade()
    {
        _randomUpgrade = _enemyUpgradesOptions[UnityEngine.Random.Range(0, _enemyUpgradesOptions.Count)];
        soundComponent.PlaySound(upgradeSound);
        switch (_randomUpgrade)
        {
            case EnemyUpgradesOptions.Speed:
                onEnemyUpgraded?.Invoke(EnemyUpgradesOptions.Speed,speedUpgrade);
                _upgradedStatus = speedUpgrade;
                break;
            case EnemyUpgradesOptions.Damage:
                onEnemyUpgraded?.Invoke(EnemyUpgradesOptions.Damage ,damageUpgrade);
                _upgradedStatus = damageUpgrade;
                break;
            case EnemyUpgradesOptions.Health:
                onEnemyUpgraded?.Invoke(EnemyUpgradesOptions.Health ,healthUpgrade);
                _upgradedStatus = healthUpgrade;
                break;
        }
        
        onBaseUpgradeSelected?.Invoke();
    }

    private void AddCardUpgradeAttribute()
    {
        upgradeText.gameObject.SetActive(true);
        upgradeText.text = $"+ {_upgradedStatus} {_randomUpgrade.ToString()}";
        upgradeButton.gameObject.SetActive(false);
        upgradeImage.gameObject.SetActive(true);
        upgradeImage.sprite = _randomUpgrade switch
        {
            EnemyUpgradesOptions.Speed => speedImage,
            EnemyUpgradesOptions.Health=> healthImage,
            EnemyUpgradesOptions.Damage => damageImage,
            _ => throw new ArgumentOutOfRangeException()
        };
        title.gameObject.SetActive(false);
    }
    
    private void SwitchCardToIdle()
    {
        upgradeText.gameObject.SetActive(false);
        upgradeButton.gameObject.SetActive(true);
        upgradeImage.gameObject.SetActive(false);
        title.gameObject.SetActive(true);
    }

}
