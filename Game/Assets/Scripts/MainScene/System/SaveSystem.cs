using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{

    public static void SaveSettings(Settings settings)
    {
        Debug.Log("Saved: " + settings.volume + " " + settings.resolutionWidth + " " + settings.resolutionHeight + " " +
                  settings.fullScreen);
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/settings.wtf";
        FileStream stream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(stream, settings);
        stream.Close();
    }
    
    public static Settings LoadSettings()
    {
        string path = Application.persistentDataPath + "/settings.wtf";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Settings data = formatter.Deserialize(stream) as Settings;
            stream.Close();
            Debug.Log("Loaded: " + data.volume + " " + data.resolutionWidth + " " + data.resolutionHeight + " " +
                      data.fullScreen);
            return data;
        }
        else
        {
            return null;
        }

    }

    public static void SaveProgress(int slot, Progress progress)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/saveSlot"+slot+".wtf";
        FileStream stream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(stream, progress);
        stream.Close();
    }

    public static void DeleteProgress(int slot)
    {
        string path = Application.persistentDataPath + "/saveSlot"+slot+".wtf";
        File.Delete(path);
    }

    public static Progress LoadProgress(int slot)
    {
        string path = Application.persistentDataPath + "/saveSlot" + slot + ".wtf";
        Debug.Log(path);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Progress data = formatter.Deserialize(stream) as Progress;
            stream.Close();
            Debug.Log(data);
            return data;
        }
        else
        {
            //Debug.LogError("Save file not found in "+ path);
            return null;
        }

    }

}
