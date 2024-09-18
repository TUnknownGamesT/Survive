using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ConstantsValues;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    #region Singleton

    public static UIManager instance;

    private void Awake()
    {
        instance = FindObjectOfType<UIManager>();
        if (instance == null)
        {
            instance = this;
        }
    }

    #endregion

    public Canvas playerCanvas;

    [Header("Player UI")]
    public Slider playerHealthBar;
    public Slider playerXpBar;
    public Slider baseHealthBar;
    public Image lifeBar;
    public Gradient lifeBarColor;
    public Image counterBackground;
    public TextMeshProUGUI counter;

    [Header("Weapon UI")]
    public GameObject gunsWrapper;
    public Image grenade;
    public WeaponDisplayerBehaviour weaponDisplayer;

    [Header("Upgrade UI")]
    public UpgradePanelBehaviour upgradePanel;


    private bool inMainMenu = true;
    private List<Canvas> history = new();
    private bool playerDied;

    private void OnEnable()
    {
        GameManager.onPauseStart += DisplayCounter;
        BaseBehaviour.onBaseHPCHnage += SetBaseSliderHP;
        BaseBehaviour.onBaseMaxHealthChanged += SetBaseSliderMax;
        PlayerHealth.onPlayerHealthChanged += SetPlayerHP;
        PlayerXP.onPlayerXpChanged += SetPlayerXp;
        PlayerXP.onPlayerLevelUp += ShowUpgradeUI;
        PlayerXP.onPlayerXpThresholdChanged += SetPlayerXpThreshold;

    }


    private void OnDisable()
    {
        GameManager.onPauseStart -= DisplayCounter;
        PlayerHealth.onPlayerHealthChanged -= SetPlayerHP;
        BaseBehaviour.onBaseHPCHnage -= SetBaseSliderHP;
        BaseBehaviour.onBaseMaxHealthChanged -= SetBaseSliderMax;
        PlayerXP.onPlayerXpChanged -= SetPlayerXp;
        PlayerXP.onPlayerLevelUp -= ShowUpgradeUI;
        PlayerXP.onPlayerXpThresholdChanged -= SetPlayerXpThreshold;
    }
    private void Start()
    {

    }

    #region UI Menu

    public void Back()
    {
        if (history.Count == 0) return;

        if (history.Count == 1)
        {
            DeactivateCanvas(history[^1]);
            history.RemoveAt(history.Count - 1);
        }
        else
        {
            DeactivateCanvas(history[^1]);
            history.RemoveAt(history.Count - 1);
            ActivateCanvas(history[^1]);
        }
    }

    public void UnPause()
    {
        Back();
        ActivateCanvas(playerCanvas);
    }


    public void CloseOptionMenu()
    {
        Cursor.visible = true;
        Back();
        //ActivateCanvas(inMainMenu ? mainMenuCanvas : pauseMenuCanvas);
    }


    public void Menu()
    {
        UnPause();
        SceneManager.LoadScene(0);
    }

    #endregion

    #region Player UI

    private void DisplayCounter(float value)
    {
        float secondsRemains = value;
        counterBackground.enabled = true;
        counter.text = secondsRemains.ToString();
        UniTask.Void(async () =>
        {
            do
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1), ignoreTimeScale: true);
                secondsRemains--;
                counter.text = secondsRemains.ToString();
                if (secondsRemains == 0)
                    counter.text = "";
            } while (secondsRemains != 0);
            counterBackground.enabled = false;
        });
    }


    public void ChangeWeaponIcon(Firearm firearm)
    {
        weaponDisplayer.SetDisplayer(firearm);
    }


    private void SetPlayerHP(float value)
    {
        playerHealthBar.value = value;
        lifeBar.color = lifeBarColor.Evaluate((playerHealthBar.normalizedValue));
    }

    private void SetPlayerXp(float value)
    {
        playerXpBar.value = value;
    }

    private void SetPlayerXpThreshold(float value)
    {
        playerXpBar.maxValue = value;
    }

    private void SetBaseSliderMax(int value)
    {
        baseHealthBar.maxValue = value;
    }

    private void SetBaseSliderHP(float value)
    {
        baseHealthBar.value = value;
    }

    public void HasGrenade()
    {
        Color c = grenade.color;
        c.a = 1;
        grenade.color = c;
    }

    public void NoGrenade()
    {
        Color c = grenade.color;
        c.a = 0.2f;
        grenade.color = c;
    }

    #endregion

    #region UpgradeUI

    public void ShowUpgradeUI()
    {
        upgradePanel.Activate();
    }


    #endregion


    private void DeactivateCanvas(Canvas canvas)
    {
        canvas.enabled = false;
        canvas.gameObject.GetComponent<GraphicRaycaster>().enabled = false;
    }

    private void ActivateCanvas(Canvas canvas)
    {
        canvas.enabled = true;
        canvas.GetComponent<GraphicRaycaster>().enabled = true;
    }


}
