using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class LowHealthTriggerScript : CutsceneTriggerMenu
{
    public TMP_InputField HealthTriggerInput;

    public void SubmitCutscene()
    {
        List<Vector2Int> TargetPositions = new List<Vector2Int>();
        foreach (FighterClass targetCharacter in TargetCharacters)
        {
            TargetPositions.Add(targetCharacter.pos);
        }

        LowHealthTriggerInfo lowHeathTrigger = new LowHealthTriggerInfo();

        if (CutscenePath.Length == 0) return;
        if (LabelInput.text.Length == 0) return;
        if (HealthTriggerInput.text.Length == 0) return;
        if (TargetCharacters.Count == 0) return;

        lowHeathTrigger.CutscenePath = CutscenePath;
        lowHeathTrigger.Label = LabelInput.text;
        lowHeathTrigger.TriggerValue = Int32.Parse(HealthTriggerInput.text);
        lowHeathTrigger.TriggerLimit = TriggerLimit;
        lowHeathTrigger.TargetPositions = TargetPositions;

        foreach (FighterClass targetCharacter in TargetCharacters)
        {
            targetCharacter.LowHealthTriggers.Add(lowHeathTrigger);
        }
        GridCrafter.CutsceneDataManager.AddLowHealthTrigger(lowHeathTrigger, TargetCharacters);

        CloseChain();
    }
}
