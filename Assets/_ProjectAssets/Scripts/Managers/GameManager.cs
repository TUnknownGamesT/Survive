using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ScenesManager))]
public class GameManager : MonoBehaviour
{

    #region Singleton 

    public static GameManager instance;

    private void Awake()
    {
        instance = FindObjectOfType<GameManager>();
        if (instance == null)
        {
            instance = this;
        }

        lvlSettings = lvlsSettings[currentLvl];
        Cursor.visible = false;
        playerRef = GameObject.FindGameObjectWithTag("Player").transform;
        crossHairRef = GameObject.FindGameObjectWithTag("CrossHair").transform;
        playerBaseRef = GameObject.FindGameObjectWithTag("PlayerBase").transform;
    }


    #endregion

    public static Action onPlayerLost;

    public static Action<float> onPauseStart;

    public static Action onNextLvl;

    public static Action onPuaseEnd;

    public static Transform playerRef;
    public static Transform crossHairRef;
    public static Transform playerBaseRef;


    public LvlSettings[] lvlsSettings;


    [SerializeField]
    private LvlSettings lvlSettings;

    public LvlSettings LvlSettings => lvlSettings;

    private int currentLvl = 0;

    public bool _isPaused = false;

    private void OnEnable()
    {
        BaseBehaviour.onBaseDestroyed += LostGame;
        PlayerHealth.onPlayerDeath += LostGame;
    }

    private void OnDisable()
    {
        BaseBehaviour.onBaseDestroyed -= LostGame;
        PlayerHealth.onPlayerDeath -= LostGame;
        UserInputController._pause.started -= PauseGame;

    }

    private void Start()
    {
        UserInputController._pause.started += PauseGame;
        Time.timeScale = 0;
    }

    private void PauseGame(InputAction.CallbackContext context)
    {
        _isPaused = !_isPaused;
        if (_isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
    }

    private void LostGame()
    {
        //Time.timeScale = 0;
        onPlayerLost?.Invoke();
    }

    public void RestartGame()
    {
        ScenesManager.ReloadCurrentScene();
    }

    public void NextWave()
    {
        onPauseStart?.Invoke(lvlSettings.pauseBetweenRounds);
        UniTask.Void(async () =>
        {
            await UniTask.Delay(TimeSpan.FromSeconds(lvlSettings.pauseBetweenRounds));

            onPuaseEnd?.Invoke();

            if (EnemySpawner.roundPassed >= lvlSettings.numberOfWaves)
            {
                Debug.Log("Next lvl");
                currentLvl++;
                onNextLvl?.Invoke();

                if (currentLvl > lvlsSettings.Length - 1)
                {
                    Debug.Log("You won");
                    return;
                }
                else
                {
                    lvlSettings = lvlsSettings[currentLvl];
                    EnemySpawner.ResetForNextLvl();
                }

            }
        });
    }

}
