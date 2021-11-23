using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutsceneViewItemScript : MonoBehaviour
{
    public TextMeshProUGUI LabelText;
    public TextMeshProUGUI TriggerTypeText;
    public TextMeshProUGUI TargetCountText;
    public TextMeshProUGUI TriggerCountText;

    public void UpdateDisplayData(string label, string triggerType, int targetCount, int triggerCount)
    {
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
}
