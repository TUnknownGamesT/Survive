using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuBehaviour : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject startMenu;

    public GameObject optionMenu;

    private bool isOptionMenuActive = false;

    public void OptionMenu()
    {
        isOptionMenuActive = !isOptionMenuActive;
        optionMenu.SetActive(isOptionMenuActive);
    }
}
