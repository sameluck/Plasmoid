using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Settings
{
    
    public float volume;
    public int resolutionWidth;
    public int resolutionHeight;
    public bool fullScreen;
    public bool hasDLC;

    public Settings(float volume, int resolutionWidth, int resolutionHeight, bool fullScreen, bool hasDLC)
    {
        this.resolutionWidth = resolutionWidth;
        this.resolutionHeight = resolutionHeight;
        this.volume = volume;
        this.fullScreen = fullScreen;
        this.hasDLC = hasDLC;
    }
    
}
