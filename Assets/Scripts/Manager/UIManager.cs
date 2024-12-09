using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private GameObject cardCollection, settingsMenu;

    [SerializeField] private Selectable DefaultButton;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
}
