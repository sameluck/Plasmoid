using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveLoadMenu : MonoBehaviour
{
    public TextMeshProUGUI[] slotsInfo;
    public TextMeshProUGUI messenger;
    private int chosenSlot = 0;

    void OnEnable()
    {
        UpdateSlotInfo();
        chosenSlot = 0;
        messenger.SetText("");
    }

    public void ChooseSlot(int slot)
    {
        chosenSlot = slot;
    }
    
    public void SaveToSlot()
    {
        if (chosenSlot == 0) 
        {
            messenger.SetText("No save slot chosen");
            return;
        }
        Progress progress = new Progress(ProgressManager.progressManager.OpenedLevels);
        SaveSystem.SaveProgress(chosenSlot, progress);
        UpdateSlotInfo();
        messenger.SetText("Game saved successfully");
    }

    public void LoadFromSlot()
    {
        if (chosenSlot == 0)
        {
            messenger.SetText("No save slot chosen");
            return;
        }
        Progress progress = SaveSystem.LoadProgress(chosenSlot);
        if (progress == null)
        {
            messenger.SetText("Save slot is empty");
            return;
        }
        progress.OpenedLevels.CopyTo(ProgressManager.progressManager.OpenedLevels, 0);
        UpdateSlotInfo();
        messenger.SetText("Game loaded successfully");
    }
    
    public void DeleteSaveSlot()
    {
        if (chosenSlot == 0)
        {
            messenger.SetText("No save slot chosen");
            return;
        }
        Progress progress = SaveSystem.LoadProgress(chosenSlot);
        if (progress == null)
        {
            messenger.SetText("Save slot is empty");
            return;
        }

        SaveSystem.DeleteProgress(chosenSlot);
        UpdateSlotInfo();
        messenger.SetText("Save deleted successfully");
    }

    private void UpdateSlotInfo()
    {
        for (int i = 0; i < slotsInfo.Length; i++)
        {
            Progress progress = SaveSystem.LoadProgress(i + 1);
            if (progress == null)
            {
                slotsInfo[i].SetText(" - Empty");
            }
            else
            {
                slotsInfo[i].SetText(" - Level " + progress.GetNextLevel());
            }
        }
    }

}
