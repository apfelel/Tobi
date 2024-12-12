using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private GameObject cardCollection, settingsMenu, pauseMenu;

    [SerializeField] private Selectable DefaultButton;

    public bool inMenu, inCollection, inPause, inSettings;
    public bool inBoosterView;

    public void ToggleCardCollection()
    {
        if (inBoosterView) return;
        if (inPause)
        {
            TogglePauseMenu();
            return;
        }
        if (inSettings)
        {
            ToggleSettingsMenu();
            return;
        }
        
        cardCollection.SetActive(!cardCollection.activeInHierarchy);
        inCollection = cardCollection.activeInHierarchy;
        inMenu = inCollection;
        
        cardCollection.GetComponentInChildren<Selectable>().Select();
    }
    public void TogglePauseMenu()
    {
        if (inBoosterView) return;

        if (inSettings)
        {
            ToggleSettingsMenu();
            return;
        }
        if (inCollection)
        {
            ToggleCardCollection();
            return;
        }
        
        if (pauseMenu.activeInHierarchy)
        {
            inMenu = false;
            inPause = false;
            pauseMenu.SetActive(false);
        }
        else
        {
            inMenu = true;
            inPause = true;
            pauseMenu.SetActive(true);
            pauseMenu.GetComponentInChildren<Selectable>().Select();
        }
    }
    public void ToggleSettingsMenu()
    {
        if (inBoosterView) return;

        if (settingsMenu.activeInHierarchy)
        {
            inMenu = false;
            inSettings = false;
            settingsMenu.SetActive(false);
            DefaultButton?.Select();
            pauseMenu?.SetActive(true);
        }
        else
        {
            inMenu = true;
            inSettings = true;
            settingsMenu.SetActive(true);
            settingsMenu.GetComponentInChildren<Selectable>().Select();
            pauseMenu?.SetActive(false);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("InGame");
        inMenu = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

   
}
