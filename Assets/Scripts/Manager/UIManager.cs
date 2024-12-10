using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private GameObject cardCollection, settingsMenu, pauseMenu;

    [SerializeField] private Selectable DefaultButton;

    public void ToggleCardCollection()
    {
        cardCollection.SetActive(!cardCollection.activeInHierarchy);
    }

    public void ToggleSettingsMenu()
    {
        if (settingsMenu.activeInHierarchy)
        {
            settingsMenu.SetActive(false);
            DefaultButton?.Select();
        }
        else
        {
            settingsMenu.SetActive(true);
            settingsMenu.GetComponentInChildren<Selectable>().Select();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("InGame");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void TogglePauseMenu()
    {
        if (pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(false);
        }
        else
        {
            pauseMenu.SetActive(true);
            pauseMenu.GetComponentInChildren<Selectable>().Select();
        }
    }
}
