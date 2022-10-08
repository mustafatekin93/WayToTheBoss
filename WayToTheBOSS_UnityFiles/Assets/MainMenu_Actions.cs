using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu_Actions : MonoBehaviour
{
    public MainMenu_Button mainMenu_Button;
    public LevelTransition levelLoader;

    public void ButtonPressedStart()
    {
        Debug.Log("START");
        levelLoader.startAnimation();
    }

    public void ButtonPressedOptions()
    {
        mainMenu_Button.OnMouseExit();
        Debug.Log("OPTIONS");
    }

    public void ButtonPressedExit()
    {
        Application.Quit();
        Debug.Log("EXIT");
    }

    public void ButtonPressedReload()
    {
        levelLoader.restartAnimation();
    }

}
