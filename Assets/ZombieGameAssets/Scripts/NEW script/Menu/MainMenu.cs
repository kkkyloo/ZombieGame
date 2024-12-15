using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject chooseGameMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseGameMenuAndSettings();
        }
    }

    public void StartArenaScene()
    {
        SceneManager.LoadScene(1); // 1 из Build scenes
    }
    
    public void StartLevelsScene()
    {
        SceneManager.LoadScene(2); // 2 из Build scenes
    }

    public void OpenGameMenu()
    {
        mainMenu.SetActive(false);
        chooseGameMenu.SetActive(true);
    }

    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void CloseGameMenuAndSettings()
    {
        settingsMenu.SetActive(false);
        chooseGameMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


}
