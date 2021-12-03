using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterTriggerBrowserScript : TriggerBrowserBaseScript
{
    public GameObject AddLowHealthTriggerMenu;

    private void Update()
    {
        if(TriggerLimitSlider.value == 0)
        {
            TriggerLimitText.SetText("Trigger Limit: Inf");
        } else
        {
            TriggerLimitText.SetText($"Trigger Limit: {TriggerLimitSlider.value}");
        }
    }

    public void AddMoreCharacters()
    {
        GameObject AddCharactersMenu = Instantiate(AddMoreTargetsMenu);
        AddCharactersMenu.GetComponent<AddMoreTargetsScript>().SourceMenu = gameObject;
        AddCharactersMenu.GetComponent<AddMoreTargetsScript>().Targets = TargetCharacters;
        AddCharactersMenu.GetComponent<AddMoreTargetsScript>().SourceGrid = GridCrafter.characterGrid;

        gameObject.SetActive(false);
    }

    public void AddLowHealthTrigger()
    {
        GameObject AddCharactersMenu = Instantiate(AddLowHealthTriggerMenu);
        AddCharactersMenu.GetComponent<LowHealthTriggerScript>().SourceMenu = gameObject;
        AddCharactersMenu.GetComponent<LowHealthTriggerScript>().TargetCharacters = TargetCharacters;
        AddCharactersMenu.GetComponent<LowHealthTriggerScript>().TriggerLimit = (int)TriggerLimitSlider.value;

        gameObject.SetActive(false);
    }
}
