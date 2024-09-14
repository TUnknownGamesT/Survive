using UnityEngine;

public class FloatingPanelBheaviour : MonoBehaviour
{
    public PauseMenuBehaviour pauseMenuWrapper;
    public StartMenuBehaviour startMenu;
    public EndGameMenuBehaviour endGameMenu;


    void OnEnable()
    {
        GameManager.onPlayerLost += EndGameMenu;

    }

    void OnDisable()
    {
        GameManager.onPlayerLost -= EndGameMenu;
    }

    public void CloseStartMenu()
    {
        startMenu.gameObject.SetActive(false);
        pauseMenuWrapper.gameObject.SetActive(true);
        Time.timeScale = 1;
    }

    public void EndGameMenu()
    {
        endGameMenu.gameObject.SetActive(true);
        endGameMenu.UpdateScoreText();
        pauseMenuWrapper.gameObject.SetActive(false);
        Time.timeScale = 0;
    }
}
