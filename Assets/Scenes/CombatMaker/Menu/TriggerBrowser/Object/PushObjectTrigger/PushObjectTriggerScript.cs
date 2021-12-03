using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class PushObjectTriggerScript : CutsceneTriggerMenu
{
    public Toggle LeftInput;
    public Toggle RightInput;
    public Toggle UpInput;
    public Toggle DownInput;

    public PushObjectTriggerInfo SourceScript;
    public bool LoadedInfo = false;

    public void SubmitCutscene()
    {
        List<Vector2Int> TargetPositions = new List<Vector2Int>();
        foreach (CombatObject targetObject in TargetCharacters)
        {
            TargetPositions.Add(targetObject.pos);
        }
        PushObjectTriggerInfo pushObjectTrigger;
        string originalLabel = "";
        if (!LoadedInfo)
        {
            pushObjectTrigger = new PushObjectTriggerInfo();
        } else
        {
            pushObjectTrigger = SourceScript;
            originalLabel = SourceScript.Label;
        }

        if (CutscenePath.Length == 0) return;
        if (LabelInput.text.Length == 0) return;
        if (TargetCharacters.Count == 0) return;
        if (GridCrafter.CutsceneDataManager.CutsceneCollection.ContainsKey(LabelInput.text)) return;

        pushObjectTrigger.CutscenePath = CutscenePath;
        pushObjectTrigger.Label = LabelInput.text;
        pushObjectTrigger.Left = LeftInput.isOn;
        pushObjectTrigger.Right = RightInput.isOn;
        pushObjectTrigger.Up = UpInput.isOn;
        pushObjectTrigger.Down = DownInput.isOn;
        pushObjectTrigger.TriggerLimit = TriggerLimit;
        pushObjectTrigger.TargetPositions = TargetPositions;
        pushObjectTrigger.GridLayer = "Object";
        if (!LoadedInfo)
        {
            GridCrafter.CutsceneDataManager.AddPushObjectTrigger(pushObjectTrigger, TargetCharacters);
            CloseChain();
        } else
        {
            GridCrafter.CutsceneDataManager.UpdateTrigger(originalLabel, pushObjectTrigger.Label, TargetCharacters);
            Close();
        }
    }

    public void LoadCutscene(PushObjectTriggerInfo sourceScript, List<GridObject> targetSource)
    {
        LoadedInfo = true;
        SourceScript = sourceScript;
        UpdateCutscenePath(sourceScript.CutscenePath);
        TargetCharacters = targetSource;
        LabelInput.text = sourceScript.Label;
        TriggerLimit = sourceScript.TriggerLimit;
        LeftInput.isOn = sourceScript.Left;
        RightInput.isOn = sourceScript.Right;
        UpInput.isOn = sourceScript.Up;
        DownInput.isOn = sourceScript.Down;
    }
}
