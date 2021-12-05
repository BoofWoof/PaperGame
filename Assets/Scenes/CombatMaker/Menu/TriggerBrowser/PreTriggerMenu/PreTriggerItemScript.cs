using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PreTriggerItemScript : MonoBehaviour
{
    public PreTriggerMenuScript ParentScript;

    public TextMeshProUGUI TriggerName;
    public string Label;
    public TextMeshProUGUI MaxTriggerCountText;
    public int MaxTriggerCount;

    public Toggle TriggeredToggle;
    public TMP_InputField TriggersNeededCount;

    public void AvailableUpdate(string label, int maxTriggerCount)
    {
        Label = label;
        TriggerName.SetText(Label);
        MaxTriggerCount = maxTriggerCount;
        MaxTriggerCountText.SetText("Max Trigger: " + MaxTriggerCount.ToString());
        TriggersNeededCount.text = MaxTriggerCount.ToString();
    }

    public void SwitchSide()
    {
        ParentScript.SwitchSide(gameObject);
    }

    public void DisableChanges()
    {
        TriggeredToggle.interactable = false;
        TriggersNeededCount.interactable = false;
    }

    public void EnableChanges()
    {
        TriggeredToggle.interactable = true;
        TriggersNeededCount.interactable = true;
    }
}
