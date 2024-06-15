using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Button continueButton;
    public TextMeshProUGUI continueButtonText;

    public void Start()
    {
        TryToLoadSettings();
        //TryToLoadSave();
        ToggleContinue(ProgressManager.progressManager.GetNextLevel() != 1);
    }

    public void OnEnable()
    {
        ToggleContinue(ProgressManager.progressManager.GetNextLevel() != 1);
    }

    private void TryToLoadSave()
    {
        Progress progress = SaveSystem.LoadProgress(1);
        if (progress == null)
            return;
        progress.OpenedLevels.CopyTo(ProgressManager.progressManager.OpenedLevels, 0);
    }

    private void TryToLoadSettings()
    {
        Settings settings = SaveSystem.LoadSettings();
        if (settings == null)
            return;
        audioMixer.SetFloat("volume", settings.volume);
        Debug.Log(settings.resolutionWidth);
        Screen.SetResolution(settings.resolutionWidth, settings.resolutionHeight, settings.fullScreen);
        ProgressManager.progressManager.hasDLC = settings.hasDLC;
    }
    
    public void NewGame()
    {
        ProgressManager.progressManager.NewGame();
        SceneManager.LoadScene("Scenes/Hub");
    }

    public void Continue()
    {
        SceneManager.LoadScene("Level" + ProgressManager.progressManager.GetNextLevel());
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void ToggleContinue(bool isActive)
    {
        continueButton.interactable = isActive;
        continueButtonText.color = new Color32(255, 255, 255, (byte)(isActive ? 255 : 50));
    }
    
}
