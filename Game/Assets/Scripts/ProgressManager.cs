using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager progressManager;
    public bool hasDLC;
    public bool[] OpenedLevels;
    
    public void OpenLevel(int levelId)
    {
        OpenedLevels[levelId - 1] = true;
    }

    public bool IsLevelOpen(int levelId)
    {
        return OpenedLevels[levelId - 1];
    }

    //Returns the farthest unlocked level (Level 1 returns '1')
    public int GetNextLevel()
    {
        int openedLevel = 0;
        for (int i = 0; i < OpenedLevels.Length; i++)
        {
            if (!OpenedLevels[i])
            {
                openedLevel = i;
                break;
            }
        }

        return openedLevel;
    }

    public void NewGame()
    {
        OpenedLevels = new bool[10];
        OpenedLevels[0] = true;
    }
    
    private void Awake()
    {
        if (progressManager != null)
        {
            Destroy(gameObject);
            return;
        }
        
        progressManager = this;
        DontDestroyOnLoad(gameObject);

        OpenedLevels = new bool[10];
        OpenedLevels[0] = true;
    }
    
}
