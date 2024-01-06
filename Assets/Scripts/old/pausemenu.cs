
using UnityEngine;
using UnityEngine.SceneManagement;

public class pausemenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseGameMenu;
    [SerializeField] private GameObject interFace;
    [SerializeField] private GameObject settings;
    public AudioSource music;
    public float mVol;

    public void PauseDown()
    {
        pauseGameMenu.SetActive(true);
        interFace.SetActive(false);
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    public void PauseUp()
    {
        AudioListener.pause = false;
        pauseGameMenu.SetActive(false);
        interFace.SetActive(true);
        Time.timeScale = 1f;
        
    }
    public void Settings()
    {
        pauseGameMenu.SetActive(false);
        settings.SetActive(true);
        
    }
    public void SettingsBack()
    {
        pauseGameMenu.SetActive(true);
        settings.SetActive(false);
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1f;
    }



}
