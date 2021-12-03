using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class PlayerEnterTriggerScript : CutsceneTriggerMenu
{
    public Toggle PlayerCheckInput;
    public Toggle PartnerCheckInput;

    public PlayerEnterTriggerInfo SourceScript;
    public bool LoadedInfo = false;

    public void SubmitCutscene()
    {
        List<Vector2Int> TargetPositions = new List<Vector2Int>();
        foreach (BlockTemplate targetObject in TargetCharacters)
        {
            TargetPositions.Add(targetObject.pos);
        }
        PlayerEnterTriggerInfo playerEnterTrigger;
        string originalLabel = "";
        if (!LoadedInfo)
        {
            playerEnterTrigger = new PlayerEnterTriggerInfo();
        } else
        {
            playerEnterTrigger = SourceScript;
            originalLabel = SourceScript.Label;
        }

        if (CutscenePath.Length == 0) return;
        if (LabelInput.text.Length == 0) return;
        if (TargetCharacters.Count == 0) return;
        if (GridCrafter.CutsceneDataManager.CutsceneCollection.ContainsKey(LabelInput.text)) return;

        playerEnterTrigger.CutscenePath = CutscenePath;
        playerEnterTrigger.Label = LabelInput.text;
        playerEnterTrigger.Clip = PlayerCheckInput.isOn;
        playerEnterTrigger.Partner = PartnerCheckInput.isOn;
        playerEnterTrigger.TriggerLimit = TriggerLimit;
        playerEnterTrigger.TargetPositions = TargetPositions;
        playerEnterTrigger.GridLayer = "Tile";
        if (!LoadedInfo)
        {
            GridCrafter.CutsceneDataManager.AddPlayerEnterTrigger(playerEnterTrigger, TargetCharacters);
            CloseChain();
        } else
        {
            GridCrafter.CutsceneDataManager.UpdateTrigger(originalLabel, playerEnterTrigger.Label, TargetCharacters);
            Close();
        }
    }

    public void LoadCutscene(PlayerEnterTriggerInfo sourceScript, List<GridObject> targetSource)
    {
        LoadedInfo = true;
        SourceScript = sourceScript;
        UpdateCutscenePath(sourceScript.CutscenePath);
        TargetCharacters = targetSource;
        LabelInput.text = sourceScript.Label;
        TriggerLimit = sourceScript.TriggerLimit;
        PlayerCheckInput.isOn = sourceScript.Clip;
        PartnerCheckInput.isOn = sourceScript.Partner;
    }
}
