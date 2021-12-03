using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class TurnsPassedTriggerScript : CutsceneTriggerMenu
{
    public TMP_InputField Turns;

    public TurnsPassedTriggerInfo SourceScript;
    public bool LoadedInfo = false;

    public void SubmitCutscene()
    {
        List<Vector2Int> TargetPositions = new List<Vector2Int>();
        TurnsPassedTriggerInfo turnsPassedTrigger;
        string originalLabel = "";
        if (!LoadedInfo)
        {
            turnsPassedTrigger = new TurnsPassedTriggerInfo();
        } else
        {
            turnsPassedTrigger = SourceScript;
            originalLabel = SourceScript.Label;
        }

        if (CutscenePath.Length == 0) return;
        if (LabelInput.text.Length == 0) return;
        if (GridCrafter.CutsceneDataManager.CutsceneCollection.ContainsKey(LabelInput.text)) return;

        turnsPassedTrigger.CutscenePath = CutscenePath;
        turnsPassedTrigger.Label = LabelInput.text;
        turnsPassedTrigger.Turns = Int32.Parse(Turns.text);
        turnsPassedTrigger.TriggerLimit = TriggerLimit;
        turnsPassedTrigger.TargetPositions = TargetPositions;
        turnsPassedTrigger.GridLayer = "Tile";
        if (!LoadedInfo)
        {
            GridCrafter.CutsceneDataManager.AddTurnsPassedTrigger(turnsPassedTrigger);
            CloseChain();
        } else
        {
            GridCrafter.CutsceneDataManager.UpdateTrigger(originalLabel, turnsPassedTrigger.Label, TargetCharacters);
            Close();
        }
    }

    public void LoadCutscene(TurnsPassedTriggerInfo sourceScript, List<GridObject> targetSource)
    {
        LoadedInfo = true;
        SourceScript = sourceScript;
        UpdateCutscenePath(sourceScript.CutscenePath);
        TargetCharacters = targetSource;
        LabelInput.text = sourceScript.Label;
        TriggerLimit = sourceScript.TriggerLimit;
        Turns.text = sourceScript.Turns.ToString();
    }
}
