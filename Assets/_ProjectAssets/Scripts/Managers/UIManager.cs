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
    public Canvas pauseMenuCanvas;
    public Canvas deathMenuCanvas;
    public Canvas mainMenuCanvas;
    public Canvas optionsCanvas;

    [Header("Player UI")]
    public Slider playerHealthBar;
    public Slider playerXpBar;
    public Slider baseHealthBar;
    public Image lifeBar;
    public Gradient lifeBarColor;
    public bool isPaused = false;
    public Image counterBackground;
    public TextMeshProUGUI counter;
    [Header("Player Death Menu")]
    public TextMeshProUGUI score;

    [Header("Weapon UI")]
    public GameObject gunsWrapper;
    public Image grenade;
    public List<IWeaponDisplayer> weaponDisplayer = new();
    private int _currentWeaponIndex = 0;

    [Header("Upgrade UI")]
    public UpgradePanelBehaviour upgradePanel;


    private bool inMainMenu = true;
    private List<Canvas> history = new();
    private bool playerDied;

    private void OnEnable()
    {
        EnemySpawner.onPauseStart += DisplayCounter;
        BaseBehaviour.onBaseHPCHnage += SetBaseSliderHP;
        BaseBehaviour.onBaseMaxHealthChanged += SetBaseSliderMax;
        EnemySpawner.onAllEnemiesDead += ShowUpgradeUI;
        PlayerHealth.onPlayerHealthChanged += SetPlayerHP;
        PlayerXP.onPlayerXpChanged += SetPlayerXP;
        PlayerXP.onPlayerLevelUp += SetPlayerLevel;
        GameManager.onGameEnd += PlayerDie;

    }


    private void OnDisable()
    {
        EnemySpawner.onPauseStart -= DisplayCounter;
        EnemySpawner.onAllEnemiesDead -= ShowUpgradeUI;
        PlayerHealth.onPlayerHealthChanged -= SetPlayerHP;
        BaseBehaviour.onBaseHPCHnage -= SetBaseSliderHP;
        BaseBehaviour.onBaseMaxHealthChanged -= SetBaseSliderMax;
        PlayerXP.onPlayerXpChanged += SetPlayerXP;
        PlayerXP.onPlayerLevelUp += SetPlayerLevel;
        UserInputController._pause.started -= Pause;
        GameManager.onGameEnd -= PlayerDie;
    }
    private void Start()
    {
        history.Add(mainMenuCanvas);
        foreach (Transform element in gunsWrapper.transform)
        {
            weaponDisplayer.Add(element.GetComponent<IWeaponDisplayer>());
        }

        weaponDisplayer.Reverse();

        UserInputController._pause.started += Pause;
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

    public void RestartGame()
    {
        ActivateCanvas(playerCanvas);
        ActivateCanvas(pauseMenuCanvas);
        //ScenesManager.instance.ReloadCurrentScene();
    }

    public void UnPause()
    {
        Back();
        ActivateCanvas(playerCanvas);
        Time.timeScale = 1f;
        isPaused = !isPaused;
    }

    private void Pause(InputAction.CallbackContext obj)
    {
        if (playerDied) return;
        Cursor.visible = !Cursor.visible;
        isPaused = !isPaused;
        if (isPaused)
        {
            if (history.Count != 0)
            {
                DeactivateCanvas(history[^1]);
            }
            history.Add(pauseMenuCanvas);
            DeactivateCanvas(playerCanvas);
            ActivateCanvas(pauseMenuCanvas);
        }
        else
        {
            Back();
            ActivateCanvas(playerCanvas);
        }
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void EnableOptionsMenu()
    {
        Cursor.visible = true;
        ActivateCanvas(optionsCanvas);
        history.Add(optionsCanvas);
        DeactivateCanvas(history[^2]);
    }

    public void CloseOptionMenu()
    {
        Cursor.visible = true;
        Back();
        //ActivateCanvas(inMainMenu ? mainMenuCanvas : pauseMenuCanvas);
    }


    public void ExitMainMenu()
    {
        Cursor.visible = false;
        DeactivateCanvas(mainMenuCanvas);
        inMainMenu = false;
    }

    private void PlayerDie()
    {
        playerDied = true;
        Cursor.visible = true;
        SetScore();
        ActivateCanvas(deathMenuCanvas);
    }

    private void SetScore()
    {
        UniTask.Void(async () =>
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.4f));
            LeanTween.value(0, ScoreKeeper.Score, 1f).setOnUpdate((float value) =>
            {
                score.text = Mathf.RoundToInt(value).ToString();
            }).setEaseInQuad();
        });
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
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                secondsRemains--;
                counter.text = secondsRemains.ToString();
                if (secondsRemains == 0)
                    counter.text = "";
            } while (secondsRemains != 0);
            counterBackground.enabled = false;
        });
    }


    public void ChangeWeaponIcon(int index)
    {
        weaponDisplayer[_currentWeaponIndex].Deactivate();
        weaponDisplayer[index].Activate();
        _currentWeaponIndex = index;
    }


    private void SetPlayerHP(float value)
    {
        playerHealthBar.value = value;
        lifeBar.color = lifeBarColor.Evaluate((playerHealthBar.normalizedValue));
    }

    private void SetPlayerXP(float value)
    {
        playerXpBar.value = value;
    }

    private void SetPlayerLevel(int value)
    {
        //TODO: implement this
        Debug.Log("Implement this!!");
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
