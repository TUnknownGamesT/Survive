using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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


    public static Action<Constants.Resources> onSliderFull;
    public static Action onSliderEmpty;

    public Canvas playerCanvas;
    public Canvas pauseMenuCanvas;
    public Canvas deathMenuCanvas;
    public Canvas mainMenuCanvas;
    public Canvas optionsCanvas;
    
    [Header("Player UI")]
    public Slider playerHealthBar;
    public Slider baseHealthBar;
    public Image lifeBar;
    public Gradient lifeBarColor;
    public bool isPaused = false;
    public TextMeshProUGUI counter;
    [Header("Player Death Menu")]
    public TextMeshProUGUI score;

    [Header("Weapon UI")] 
    public GameObject gunsWrapper;
    public Image grenade;
    public List<IWeaponDisplayer> weaponDisplayer = new();
    private int _currentWeaponIndex = 0;

    [Header("Resources UI")] 
    public Slider computerSlider;
    public Slider engineSlider;
    public Slider pressureSlider;
    private Slider currentResource;

    [Header("Upgrade UI")]
    public UpgradePanelBehaviour upgradePanel;
    
    
    private  bool inMainMenu = true;
    private List<Canvas> history = new ();
    private bool playerDied;

    private void OnEnable()
    {
        EnemySpawner.onPauseStart += DisplayCounter;
        BaseBehaviour.onBaseHPCHnage += SetBaseSliderHP;
        BaseBehaviour.onBaseMaxHealthChanged += SetBaseSliderMax; 
        EnemySpawner.onAllEnemiesDead += ShowUpgradeUI;
        PlayerHealth.onPlayerHealthChanged += SetPlayerHP;
        GameManager.onGameEnd += PlayerDie;
        
    }

    
    private void OnDisable()
    {
        EnemySpawner.onPauseStart -= DisplayCounter;
        EnemySpawner.onAllEnemiesDead -= ShowUpgradeUI;
        PlayerHealth.onPlayerHealthChanged -= SetPlayerHP;
        BaseBehaviour.onBaseHPCHnage -= SetBaseSliderHP;
        BaseBehaviour.onBaseMaxHealthChanged -= SetBaseSliderMax;
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
        inMainMenu= false;
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
            await  UniTask.Delay(TimeSpan.FromSeconds(0.4f));
           LeanTween.value(0,ScoreKeeper.Score,1f).setOnUpdate((float value) =>
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
        counter.text = secondsRemains.ToString();
        UniTask.Void(async () =>
        {
            do
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1));
                secondsRemains--;
                counter.text = secondsRemains.ToString();
                if(secondsRemains == 0)
                    counter.text = "";
            } while (secondsRemains != 0);
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
        grenade.color=c;
    }
    
    public void NoGrenade()
    {
        Color c = grenade.color;
        c.a = 0.2f;
        grenade.color=c;
    }

    #endregion

    #region Resources UI

    
    public float GetSliderValue(Constants.Resources resource)
    {
        switch (resource)
        {
            case Constants.Resources.ComputerBoard:
                return computerSlider.value;
            case Constants.Resources.Engine:
                return engineSlider.value;
            case Constants.Resources.Pressure:
                return pressureSlider.value;
        }
        return 0;
    }

    private void HighLightResource(Constants.Resources resource)
    {
        switch (resource)
        {
            case Constants.Resources.ComputerBoard:
                HighLightSlider(computerSlider);
                break;
            case Constants.Resources.Engine:
                HighLightSlider(engineSlider);
                break;
            case Constants.Resources.Pressure:
                HighLightSlider(pressureSlider);
                break;
        }
    }

    private void HighLightSlider(Slider slider)
    {
        if (currentResource != null)
        {
            currentResource.GetComponent<CanvasGroup>().alpha = 0.5f;
            currentResource.GetComponent<RectTransform>().localScale = Vector3.one;
            currentResource.gameObject.SetActive(false);
        }
        
        currentResource = slider;
        
        currentResource.gameObject.SetActive(true);
        currentResource.GetComponent<CanvasGroup>().alpha = 1;
        currentResource.GetComponent<RectTransform>().localScale = Vector3.one;

       
    }
    
    public void AddSlidersValue(Constants.Resources resource,float value)
    {
        
        Debug.Log(value);
        switch (resource)
        {
            case Constants.Resources.ComputerBoard:
                AddSliderValue(value,computerSlider);
                break;
            case Constants.Resources.Engine:
                AddSliderValue(value,engineSlider);
                break;
            case Constants.Resources.Pressure:
                AddSliderValue(value,pressureSlider);
                break;
        }
    }
    
    private void AddSliderValue(float value,Slider slider)
    {
        slider.value += value;
        if(computerSlider.value >= computerSlider.maxValue)
            onSliderFull?.Invoke(Constants.Resources.ComputerBoard);
    }
    
    public void SetSlidersValue(Constants.Resources resource,float value)
    {
        switch (resource)
        {
            case Constants.Resources.ComputerBoard:
                SetSliderValue(value,computerSlider,Constants.Resources.ComputerBoard);
                break;
            case Constants.Resources.Engine:
                SetSliderValue(value,engineSlider,Constants.Resources.Engine);
                break;
            case Constants.Resources.Pressure:
                SetSliderValue(value,pressureSlider,Constants.Resources.Pressure);
                break;
        }
    }

    private void SetSliderValue(float value,Slider slider,Constants.Resources resource)
    {
        Debug.Log(Mathf.Floor(value));
        if (value > slider.maxValue)
            return;
        
        slider.value = value;
        if (slider.value == 0)
        {
            onSliderEmpty?.Invoke();
        }else if (Mathf.Floor(value) >= slider.maxValue)
        {
            onSliderFull?.Invoke(resource);
        }

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
