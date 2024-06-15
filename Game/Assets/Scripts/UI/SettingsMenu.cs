using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    private List<Resolution> resolutions;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public TextMeshProUGUI DLCButtonText;

    public Slider volumeSlider;
    public Toggle fullScreenToggle;

    private float savedVolume;
    private bool savedFullScreen;
    private Resolution savedResolution;
    private bool hasDLC = false;

    void OnEnable()
    {
        resolutions = new List<Resolution>(Screen.resolutions);
        resolutions.Reverse();
        
        resolutionDropdown.ClearOptions();
        
        int currentRes = 0;
        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Count; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].height == Screen.currentResolution.height && resolutions[i].width == Screen.currentResolution.width) 
                currentRes = i;
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentRes;
        resolutionDropdown.RefreshShownValue();
        
        audioMixer.GetFloat("volume", out savedVolume);
        Debug.Log(Screen.currentResolution);
        savedFullScreen = Screen.fullScreen;
        savedResolution = Screen.currentResolution;
        
        fullScreenToggle.isOn = savedFullScreen;
        volumeSlider.value = savedVolume;
        hasDLC = ProgressManager.progressManager.hasDLC;
        if (hasDLC)
            DLCButtonText.SetText("Delete DLC");
    }

    public void saveSettings()
    {
        Settings settings = new Settings(savedVolume, savedResolution.width, savedResolution.height, savedFullScreen, hasDLC);
        SaveSystem.SaveSettings(settings);
    }
    
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        savedVolume = volume;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        savedFullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        savedResolution = resolution;
    }

    public void ToggleDLC()
    {
        if (hasDLC)
        {
            hasDLC = false;
            ProgressManager.progressManager.hasDLC = false;
            DLCButtonText.SetText("Buy DLC");
        }
        else
        {
            hasDLC = true;
            ProgressManager.progressManager.hasDLC = true;
            DLCButtonText.SetText("Delete DLC");
        }
    }
    
}
