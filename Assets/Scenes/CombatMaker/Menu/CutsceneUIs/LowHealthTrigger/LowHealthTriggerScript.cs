using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class LowHealthTriggerScript : CutsceneTriggerMenu
{
    public TMP_InputField HealthTriggerInput;

    public LowHealthTriggerInfo SourceScript;
    public bool LoadedInfo = false;

    public void SubmitCutscene()
    {
        List<Vector2Int> TargetPositions = new List<Vector2Int>();
        foreach (FighterClass targetCharacter in TargetCharacters)
        {
            TargetPositions.Add(targetCharacter.pos);
        }
        LowHealthTriggerInfo lowHealthTrigger;
        string originalLabel = "";
        if (!LoadedInfo)
        {
            lowHealthTrigger = new LowHealthTriggerInfo();
        } else
        {
            lowHealthTrigger = SourceScript;
            originalLabel = SourceScript.Label;
        }

        if (CutscenePath.Length == 0) return;
        if (LabelInput.text.Length == 0) return;
        if (HealthTriggerInput.text.Length == 0) return;
        if (TargetCharacters.Count == 0) return;

        lowHealthTrigger.CutscenePath = CutscenePath;
        lowHealthTrigger.Label = LabelInput.text;
        lowHealthTrigger.TriggerValue = Int32.Parse(HealthTriggerInput.text);
        lowHealthTrigger.TriggerLimit = TriggerLimit;
        lowHealthTrigger.TargetPositions = TargetPositions;
        lowHealthTrigger.GridLayer = "Character";
        if (!LoadedInfo)
        {
            GridCrafter.CutsceneDataManager.AddLowHealthTrigger(lowHealthTrigger, TargetCharacters);
            CloseChain();
        } else
        {
            GridCrafter.CutsceneDataManager.UpdateTrigger(originalLabel, lowHealthTrigger.Label, TargetCharacters);
            Close();
        }
    }

    public void LoadCutscene(LowHealthTriggerInfo sourceScript, List<GridObject> targetSource)
    {
        LoadedInfo = true;
        SourceScript = sourceScript;
        UpdateCutscenePath(sourceScript.CutscenePath);
        TargetCharacters = targetSource;
        LabelInput.text = sourceScript.Label;
        HealthTriggerInput.text = sourceScript.TriggerValue.ToString();
        TriggerLimit = sourceScript.TriggerLimit;
    }
}
