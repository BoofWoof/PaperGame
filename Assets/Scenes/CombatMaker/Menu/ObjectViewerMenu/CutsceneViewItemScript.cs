using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class CutsceneViewItemScript : MonoBehaviour
{
    public CharacterViewMenuScript SourceMenu;
    public GameObject AddMoreTargetsMenu;
    public GameObject AddLowHealthTriggerMenu;

    public TextMeshProUGUI LabelText;
    public TextMeshProUGUI TriggerTypeText;
    public TextMeshProUGUI TargetCountText;
    public TextMeshProUGUI TriggerCountText;
    public TMP_InputField TriggerCountInput;

    public string Label;

    public void UpdateDisplayData(string label, string triggerType, int targetCount, int triggerCount)
    {
        Label = label;
        LabelText.SetText("Label: " + label);
        TriggerTypeText.SetText("Type: " + triggerType);
        TargetCountText.SetText($"Target Count: {targetCount}");
        if (triggerCount == 0)
        {
            TriggerCountText.SetText("Trigger Count: inf");
        } else
        {
            TriggerCountText.SetText($"Trigger Count: {triggerCount}");
        }
    }

    public void DeleteCutscene()
    {
        GridCrafter.CutsceneDataManager.RemoveTrigger(Label);
        SourceMenu.UpdateCutsceneList();
    }

    public void EditTargets()
    {
        GameObject AddCharactersMenu = Instantiate(AddMoreTargetsMenu);
        AddCharactersMenu.GetComponent<AddMoreTargetsScript>().SourceMenu = SourceMenu.gameObject;
        AddCharactersMenu.GetComponent<AddMoreTargetsScript>().Targets = GridCrafter.CutsceneDataManager.GetTargets(Label);
        AddCharactersMenu.GetComponent<AddMoreTargetsScript>().SourceGrid = GridCrafter.characterGrid;

        SourceMenu.gameObject.SetActive(false);
    }

    public void EditTriggerCount()
    {
        GridCrafter.CutsceneDataManager.GetTrigger(Label).TriggerLimit = Int32.Parse(TriggerCountInput.text);
        SourceMenu.UpdateCutsceneList();
    }

    public void EditTrigger()
    {
        CutsceneTriggerInfo trigger = GridCrafter.CutsceneDataManager.GetTrigger(Label);
        if (trigger is LowHealthTriggerInfo)
        {
            GameObject AddCharactersMenu = Instantiate(AddLowHealthTriggerMenu);
            AddCharactersMenu.GetComponent<LowHealthTriggerScript>().SourceMenu = SourceMenu.gameObject;
            AddCharactersMenu.GetComponent<LowHealthTriggerScript>().LoadCutscene((LowHealthTriggerInfo)trigger, GridCrafter.CutsceneDataManager.GetTargets(Label));

            SourceMenu.gameObject.SetActive(false);
        }
    }
}