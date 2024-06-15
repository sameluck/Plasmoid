using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{

    [SerializeField] private string newGameLevel;
    [SerializeField] private int numToOpenLevel;

    public void NewGameButton()
    {
        //SaveSystem.SaveParams(1, null);
        LoadLevel(newGameLevel);
    }

    public void ContinueGameButton()
    {
        if (PlayerPrefs.HasKey("LevelSaved"))
        {
            string levelToLoad = PlayerPrefs.GetString("LevelSaved");
            SceneManager.LoadScene(levelToLoad);
        }
    }

    public static void LoadLevel(string levelName)
    {
        Debug.Log("Loading:"+levelName);
        SceneManager.LoadScene(levelName);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetString("LevelSaved", newGameLevel);
            ProgressManager.progressManager.OpenLevel(numToOpenLevel);
            StopAllCoroutines();
            LoadLevel(newGameLevel);
            gameObject.SetActive(false);
        }
    }
    
}
