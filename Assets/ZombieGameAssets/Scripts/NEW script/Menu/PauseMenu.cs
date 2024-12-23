using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject _canvas;


    public static bool GameIsPaused = false;
    public static bool SettingsISOpened = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused && SettingsISOpened)
            {
                CloseSettings();
            }
            else if (!GameIsPaused)
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        _canvas.GetComponent<Canvas>().enabled = true;

        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        if(_canvas != null)
        {
            _canvas.GetComponent<Canvas>().enabled = false;
        }
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void GameMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene(0);
    }
    public void OpenSettings()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
        SettingsISOpened = true;
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
        SettingsISOpened = false;

    }

    public void ContinueGame()
    {
        Resume();
    }

}
