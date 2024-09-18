using System;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{

    public Canvas staticObjects;

    public GameObject pauseMenu;
    public GameObject startMenu;
    public EndGameMenuBehaviour endGameMenu;

    public Canvas optionMenu;

    public bool pauseCanvasActive = false;


    private GameObject _currentCanvas;


    void OnDisable()
    {
        GameManager.onPlayerLost -= EndGameMenu;
        UserInputController._pause.started -= ActivatePauseMenu;
    }

    void Start()
    {
        GameManager.onPlayerLost += EndGameMenu;
        _currentCanvas = startMenu.gameObject;
    }

    public void CloseStartMenu()
    {
        UserInputController._pause.started += ActivatePauseMenu;
        DeactivateCanvas(startMenu.gameObject);
        staticObjects.enabled = false;
    }

    public void EndGameMenu()
    {
        UserInputController._pause.started -= ActivatePauseMenu;
        endGameMenu.UpdateScoreText();
        staticObjects.enabled = true;
        ActivateCanvas(endGameMenu.gameObject);
        DeactivateCanvas(pauseMenu.gameObject);
        Time.timeScale = 0;
    }

    public void ActivateOptionMenu()
    {
        DeactivateCanvas(_currentCanvas);
        ActivateCanvas(optionMenu.gameObject);
    }

    public void DeactivateOptionMenu()
    {
        DeactivateCanvas(optionMenu.gameObject);
        ActivateCanvas(_currentCanvas);
    }

    public void ActivatePauseMenu(InputAction.CallbackContext context)
    {
        if (pauseCanvasActive)
        {
            DeactivateCanvas(pauseMenu.gameObject);
            DeactivateCanvas(optionMenu.gameObject);
            staticObjects.enabled = false;
        }
        else
        {
            _currentCanvas = pauseMenu.gameObject;
            ActivateCanvas(pauseMenu.gameObject);
            staticObjects.enabled = true;
        }
        pauseCanvasActive = !pauseCanvasActive;
    }

    private void DeactivateCanvas(GameObject canvas)
    {
        canvas.GetComponent<Canvas>().enabled = false;
        canvas.GetComponent<GraphicRaycaster>().enabled = false;
    }

    private void ActivateCanvas(GameObject canvas)
    {
        canvas.GetComponent<Canvas>().enabled = true;
        canvas.GetComponent<GraphicRaycaster>().enabled = true;
    }
}
