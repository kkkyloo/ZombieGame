using UnityEngine;
using UnityEngine.Events;

public class SettingsManager : SingletonManager<SettingsManager>
{
    //public UnityEvent OnSFXVolumeChanged = new UnityEvent();
    //public UnityEvent OnMusicVolumeChanged = new UnityEvent();
    public UnityEvent OnResolutionChanged = new UnityEvent();
    public UnityEvent OnWindowModeChanged = new UnityEvent();

    // ���������
    //private float sfxVolume;
    //private float musicVolume;

    private Resolution[] availableResolutions;
    private int currentResolutionIndex;

    // true - ������������� �����, false - ������� �����
    private bool isFullScreen;

    protected override void Awake()
    {
        base.Awake();
        LoadSettings();
    }

    public void LoadSettings()
    {
        // �������� ���������
        //sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        //musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);

        // �������� ����������
        availableResolutions = Screen.resolutions;
        int defaultWidth = Screen.currentResolution.width;
        int defaultHeight = Screen.currentResolution.height;
        int savedWidth = PlayerPrefs.GetInt("ResolutionWidth", defaultWidth);
        int savedHeight = PlayerPrefs.GetInt("ResolutionHeight", defaultHeight);

        // ���� ������ ����������� ����������
        currentResolutionIndex = 0;
        for (int i = 0; i < availableResolutions.Length; i++)
        {
            if (availableResolutions[i].width == savedWidth &&
                availableResolutions[i].height == savedHeight)
            {
                currentResolutionIndex = i;
                break;
            }
        }

        Screen.SetResolution(
            availableResolutions[currentResolutionIndex].width,
            availableResolutions[currentResolutionIndex].height,
            Screen.fullScreen
        );

        // �������� ������ ����
        // ���� � PlayerPrefs �� ���� ���������, �� ��������� ���������� ������� �������� ������
        int fullScreenPref = PlayerPrefs.GetInt("IsFullScreen", Screen.fullScreen ? 1 : 0);
        isFullScreen = (fullScreenPref == 1);
        Screen.fullScreen = isFullScreen;

        // ��������� ���������
        ApplyVolumes();

        // �������� �������
        //OnSFXVolumeChanged?.Invoke();
        //OnMusicVolumeChanged?.Invoke();
        OnResolutionChanged?.Invoke();
        OnWindowModeChanged?.Invoke();
    }

    public void SaveSettings()
    {
        //PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        //PlayerPrefs.SetFloat("MusicVolume", musicVolume);

        var currentRes = availableResolutions[currentResolutionIndex];
        PlayerPrefs.SetInt("ResolutionWidth", currentRes.width);
        PlayerPrefs.SetInt("ResolutionHeight", currentRes.height);

        PlayerPrefs.SetInt("IsFullScreen", isFullScreen ? 1 : 0);

        PlayerPrefs.Save();
    }

    private void ApplyVolumes()
    {
        // ������������, ��� SFXVolume ������������ ����� ��������, � MusicVolume � ������
        // ����� ������������ ������������ ��� ��������� AudioSource, � ������ ������� � ���������� ������:
        // AudioListener.volume = musicVolume; // ���� ����� ������������ ���������.
        // �� ����� ����� ���������� ������. ��� ������� ������� ���:
        // ����� ����� �������� ������, �������� ��������� ���������� AudioMixer �����.

        // ��������, � ��� ���� AudioMixer � ����������� "SFXVolume" � "MusicVolume"
        // ����� �� ������ ��������� �� ��� (��������� ������ �� Mixer):
        // audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
        // audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
    }

    // ������ ��� ��������� ��������

    //public void SetSFXVolume(float volume)
    //{
    //    sfxVolume = Mathf.Clamp01(volume);
    //    SaveSettings();
    //    OnSFXVolumeChanged?.Invoke();
    //    ApplyVolumes();
    //}

    //public void SetMusicVolume(float volume)
    //{
    //    musicVolume = Mathf.Clamp01(volume);
    //    SaveSettings();
    //    OnMusicVolumeChanged?.Invoke();
    //    ApplyVolumes();
    //}

    public void NextResolution()
    {
        currentResolutionIndex++;
        if (currentResolutionIndex >= availableResolutions.Length)
            currentResolutionIndex = 0;

        ApplyResolution();
    }

    public void PreviousResolution()
    {
        currentResolutionIndex--;
        if (currentResolutionIndex < 0)
            currentResolutionIndex = availableResolutions.Length - 1;

        ApplyResolution();
    }

    private void ApplyResolution()
    {
        var res = availableResolutions[currentResolutionIndex];
        Screen.SetResolution(res.width, res.height, isFullScreen);
        SaveSettings();
        OnResolutionChanged?.Invoke();
    }

    public void ToggleWindowMode()
    {
        isFullScreen = !isFullScreen;
        var res = availableResolutions[currentResolutionIndex];
        Screen.SetResolution(res.width, res.height, isFullScreen);
        SaveSettings();
        OnWindowModeChanged?.Invoke();
    }

    // ������ ��� ��������� ������� ��������
    //public float GetSFXVolume() => sfxVolume;
    //public float GetMusicVolume() => musicVolume;
    public Resolution GetCurrentResolution() => availableResolutions[currentResolutionIndex];
    public bool IsFullScreen() => isFullScreen;

}
