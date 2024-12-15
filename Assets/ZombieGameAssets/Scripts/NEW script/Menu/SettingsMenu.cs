using TMPro;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [Header("UI Elements")]
    //public Slider sfxVolumeSlider;
    //public Slider musicVolumeSlider;
    public TextMeshProUGUI resolutionText;
    public TextMeshProUGUI windowModeText;

    void Start()
    {
        if (SettingsManager.Instance != null)
        {
            //SettingsManager.Instance.OnSFXVolumeChanged.AddListener(UpdateSFXVolumeUI);
            //SettingsManager.Instance.OnMusicVolumeChanged.AddListener(UpdateMusicVolumeUI);
            SettingsManager.Instance.OnResolutionChanged.AddListener(UpdateResolutionUI);
            SettingsManager.Instance.OnWindowModeChanged.AddListener(UpdateWindowModeUI);
        }

        InitializeUI();
    }

    void OnDestroy()
    {
        if (SettingsManager.Instance != null)
        {
            //SettingsManager.Instance.OnSFXVolumeChanged.RemoveListener(UpdateSFXVolumeUI);
            //SettingsManager.Instance.OnMusicVolumeChanged.RemoveListener(UpdateMusicVolumeUI);
            SettingsManager.Instance.OnResolutionChanged.RemoveListener(UpdateResolutionUI);
            SettingsManager.Instance.OnWindowModeChanged.RemoveListener(UpdateWindowModeUI);
        }
    }

    void InitializeUI()
    {
        //sfxVolumeSlider.value = SettingsManager.Instance.GetSFXVolume();
        //musicVolumeSlider.value = SettingsManager.Instance.GetMusicVolume();

        UpdateResolutionUI();
        UpdateWindowModeUI();
    }

    // Методы для обработки событий UI


    //public void OnSFXVolumeChangedUI(float value)
    //{
    //    SettingsManager.Instance.SetSFXVolume(value);
    //}

    //public void OnMusicVolumeChangedUI(float value)
    //{
    //    SettingsManager.Instance.SetMusicVolume(value);
    //}

    public void OnResolutionLeftClick()
    {
        SettingsManager.Instance.PreviousResolution();
    }

    public void OnResolutionRightClick()
    {
        SettingsManager.Instance.NextResolution();
    }
    public void OnWindowModeToggleClick()
    {
        SettingsManager.Instance.ToggleWindowMode();
    }

    //void UpdateSFXVolumeUI()
    //{
    //    sfxVolumeSlider.value = SettingsManager.Instance.GetSFXVolume();
    //}

    //void UpdateMusicVolumeUI()
    //{
    //    musicVolumeSlider.value = SettingsManager.Instance.GetMusicVolume();
    //}

    void UpdateResolutionUI()
    {
        var res = SettingsManager.Instance.GetCurrentResolution();
        resolutionText.text = $"{res.width}x{res.height}";
    }

    void UpdateWindowModeUI()
    {
        bool isFullScreen = SettingsManager.Instance.IsFullScreen();
        windowModeText.text = isFullScreen ? "ВЫКЛ" : "ВКЛ";
    }
}
