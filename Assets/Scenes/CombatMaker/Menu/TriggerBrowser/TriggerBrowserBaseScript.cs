using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TriggerBrowserBaseScript : MenuBaseScript
{
    public GameObject AddMoreTargetsMenu;

    public List<GridObject> TargetCharacters;
    public Slider TriggerLimitSlider;
    public TextMeshProUGUI TriggerLimitText;
}
