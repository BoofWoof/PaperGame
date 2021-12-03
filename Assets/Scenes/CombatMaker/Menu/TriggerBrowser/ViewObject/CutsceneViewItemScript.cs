using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class CutsceneViewItemScript : MonoBehaviour
{
    public Button EditTargetsButton;

    public ViewMenuBaseScript SourceMenu;
    public GameObject AddMoreTargetsMenu;
    public GameObject AddLowHealthTriggerMenu;
    public GameObject AddPushObjectTriggerMenu;
    public GameObject AddPlayerEnterTriggerMenu;
    public GameObject AddTurnsPassedTriggerMenu;

    public GameObject[,] SourceGrid;

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
        if(targetCount == -1)
        {
            EditTargetsButton.interactable = false;
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
        AddCharactersMenu.GetComponent<AddMoreTargetsScript>().SourceGrid = SourceGrid;

        SourceMenu.gameObject.SetActive(false);
    }

    public void EditTriggerCount()
    {
        if(TriggerCountInput.text.Length > 0)
        {
            GridCrafter.CutsceneDataManager.GetTrigger(Label).TriggerLimit = Int32.Parse(TriggerCountInput.text);
            SourceMenu.UpdateCutsceneList();
        }
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
        if (trigger is PushObjectTriggerInfo)
        {
            GameObject AddCharactersMenu = Instantiate(AddPushObjectTriggerMenu);
            AddCharactersMenu.GetComponent<PushObjectTriggerScript>().SourceMenu = SourceMenu.gameObject;
            AddCharactersMenu.GetComponent<PushObjectTriggerScript>().LoadCutscene((PushObjectTriggerInfo)trigger, GridCrafter.CutsceneDataManager.GetTargets(Label));

            SourceMenu.gameObject.SetActive(false);
        }
        if (trigger is PlayerEnterTriggerInfo)
        {
            GameObject AddCharactersMenu = Instantiate(AddPlayerEnterTriggerMenu);
            AddCharactersMenu.GetComponent<PlayerEnterTriggerScript>().SourceMenu = SourceMenu.gameObject;
            AddCharactersMenu.GetComponent<PlayerEnterTriggerScript>().LoadCutscene((PlayerEnterTriggerInfo)trigger, GridCrafter.CutsceneDataManager.GetTargets(Label));

            SourceMenu.gameObject.SetActive(false);
        }
        if (trigger is TurnsPassedTriggerInfo)
        {
            GameObject AddCharactersMenu = Instantiate(AddTurnsPassedTriggerMenu);
            AddCharactersMenu.GetComponent<TurnsPassedTriggerScript>().SourceMenu = SourceMenu.gameObject;
            AddCharactersMenu.GetComponent<TurnsPassedTriggerScript>().LoadCutscene((TurnsPassedTriggerInfo)trigger, GridCrafter.CutsceneDataManager.GetTargets(Label));

            SourceMenu.gameObject.SetActive(false);
        }
    }
}