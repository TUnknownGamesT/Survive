using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenuBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject optionMenu;
    public GameObject mainMenu;

    private bool isOptionMenuActive = false;
    private bool isMainMenuActive = false;


    void OnDisable()
    {
        UserInputController._pause.started -= PauseMenu;
    }

    void Start()
    {
        UserInputController._pause.started += PauseMenu;
    }


    private void PauseMenu(InputAction.CallbackContext context)
    {
        Menu();
    }


    public void OptionMenu()
    {
        isOptionMenuActive = !isOptionMenuActive;
        optionMenu.SetActive(isOptionMenuActive);
    }

    public void CloseOptionMenu()
    {
        optionMenu.SetActive(false);
    }

    public void Menu()
    {
        isMainMenuActive = !isMainMenuActive;
        if (isOptionMenuActive)
            isOptionMenuActive = false;

        optionMenu.SetActive(isOptionMenuActive);
        mainMenu.SetActive(isMainMenuActive);
    }
}
