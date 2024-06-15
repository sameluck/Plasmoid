using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Progress
{
    public bool[] OpenedLevels;

    public Progress()
    {
        OpenedLevels = new bool[10];
        OpenedLevels[0] = true;
    }
    
    public Progress(bool[] openedLevels)
    {
        OpenedLevels = new bool[10];
        openedLevels.CopyTo(OpenedLevels, 0);
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

}
