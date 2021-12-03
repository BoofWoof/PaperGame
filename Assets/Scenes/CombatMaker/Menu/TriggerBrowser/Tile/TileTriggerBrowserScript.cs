using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TileTriggerBrowserScript : TriggerBrowserBaseScript
{
    public GameObject AddPlayerEnterTriggerMenu;

    public override void Start()
    {
        base.Start();
        transform.SetParent(SourceMenu.transform.parent);
        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

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

    public void AddMoreTiles()
    {
        GameObject AddCharactersMenu = Instantiate(AddMoreTargetsMenu);
        AddCharactersMenu.GetComponent<AddMoreTargetsScript>().SourceMenu = gameObject;
        AddCharactersMenu.GetComponent<AddMoreTargetsScript>().Targets = TargetCharacters;
        AddCharactersMenu.GetComponent<AddMoreTargetsScript>().SourceGrid = GridCrafter.blockGrid;

        gameObject.SetActive(false);
    }

    public void AddPlayerEnterTrigger()
    {
        GameObject PlayerEnterTriggerMenu = Instantiate(AddPlayerEnterTriggerMenu);
        PlayerEnterTriggerMenu.GetComponent<PlayerEnterTriggerScript>().SourceMenu = gameObject;
        PlayerEnterTriggerMenu.GetComponent<PlayerEnterTriggerScript>().TargetCharacters = TargetCharacters;
        PlayerEnterTriggerMenu.GetComponent<PlayerEnterTriggerScript>().TriggerLimit = (int)TriggerLimitSlider.value;

        gameObject.SetActive(false);
    }
}
