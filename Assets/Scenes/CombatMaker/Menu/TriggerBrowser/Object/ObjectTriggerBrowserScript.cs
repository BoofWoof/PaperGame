using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectTriggerBrowserScript : TriggerBrowserBaseScript
{
    public GameObject AddPushObjectTriggerMenu;

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

    public void AddMoreObjects()
    {
        GameObject AddCharactersMenu = Instantiate(AddMoreTargetsMenu);
        AddCharactersMenu.GetComponent<AddMoreTargetsScript>().SourceMenu = gameObject;
        AddCharactersMenu.GetComponent<AddMoreTargetsScript>().Targets = TargetCharacters;
        AddCharactersMenu.GetComponent<AddMoreTargetsScript>().SourceGrid = GridCrafter.objectGrid;

        gameObject.SetActive(false);
    }

    public void AddPushTrigger()
    {
        GameObject PushTriggerMenu = Instantiate(AddPushObjectTriggerMenu);
        PushTriggerMenu.GetComponent<PushObjectTriggerScript>().SourceMenu = gameObject;
        PushTriggerMenu.GetComponent<PushObjectTriggerScript>().TargetCharacters = TargetCharacters;
        PushTriggerMenu.GetComponent<PushObjectTriggerScript>().TriggerLimit = (int)TriggerLimitSlider.value;

        gameObject.SetActive(false);
    }
}
