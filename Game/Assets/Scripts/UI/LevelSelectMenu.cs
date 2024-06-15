using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectMenu : MonoBehaviour
{

    public Button[] levelButtons;
    public Button DLCButton;

    void OnEnable()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            levelButtons[i].gameObject.SetActive(ProgressManager.progressManager.OpenedLevels[i]);
        }
        DLCButton.gameObject.SetActive(ProgressManager.progressManager.hasDLC);
    }
    
    public void StartLevel(int levelIndex)
    {
        SceneManager.LoadScene("Level" + levelIndex);
    }
    
}
