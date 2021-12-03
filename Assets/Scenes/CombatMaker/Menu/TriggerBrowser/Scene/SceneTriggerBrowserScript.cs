using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneTriggerBrowserScript : TriggerBrowserBaseScript
{
    public GameObject AddTurnsPassedTriggerMenu;

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

    public void AddTurnsPassedTrigger()
    {
        GameObject PushTriggerMenu = Instantiate(AddTurnsPassedTriggerMenu);
        PushTriggerMenu.GetComponent<TurnsPassedTriggerScript>().SourceMenu = gameObject;
        PushTriggerMenu.GetComponent<TurnsPassedTriggerScript>().TargetCharacters = TargetCharacters;
        PushTriggerMenu.GetComponent<TurnsPassedTriggerScript>().TriggerLimit = (int)TriggerLimitSlider.value;

        gameObject.SetActive(false);
    }
}
