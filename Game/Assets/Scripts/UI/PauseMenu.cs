using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool IsPaused = false;
    public GameObject PauseMenuUI;

    public void Start()
    {
        UnificControl.EscButton += OnEscape;
    }

    void OnEscape(object sender, EventArgs e)
    {
        if (IsPaused)
            Resume();
        else
            Pause();
    }

    private void Pause()
    {
        Debug.Log(PauseMenuUI);
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0;
        IsPaused = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1;
        IsPaused = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void MainMenu()
    {
        IsPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    void OnDestroy()
    {
        UnificControl.EscButton -= OnEscape;
    }
    
}
