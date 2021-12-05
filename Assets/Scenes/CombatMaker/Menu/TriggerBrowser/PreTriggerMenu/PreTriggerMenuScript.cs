using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PreTriggerMenuScript : MenuBaseScript
{
    public string Label;

    private List<GameObject> AvailableTriggerList = new List<GameObject>();
    public GameObject AvailableContentScreen;
    private List<GameObject> SelectedTriggerList = new List<GameObject>();
    public GameObject SelectedContentScreen;
    public GameObject TriggerItem;

    public override void Start()
    {
        base.Start();
        UpdateCutsceneList();
        LoadList();
    }

    private void LoadList()
    {
        List<GameObject> ToSwitch = new List<GameObject>();
        foreach (PreTriggerInfo preTriggerInfo in GridCrafter.CutsceneDataManager.GetTrigger(Label).PreTriggerConditions)
        {
            foreach(GameObject AvailableTrigger in AvailableTriggerList)
            {
                PreTriggerItemScript preTrigger = AvailableTrigger.GetComponent<PreTriggerItemScript>();
                if(preTrigger.Label == preTriggerInfo.TriggerName)
                {
                    preTrigger.TriggeredToggle.isOn = preTriggerInfo.Triggered;
                    preTrigger.TriggersNeededCount.text = preTriggerInfo.TriggersNeeded.ToString();
                    ToSwitch.Add(AvailableTrigger);
                    continue;
                }
            }
        }
        foreach (GameObject toSwitch in ToSwitch)
        {
            SwitchSide(toSwitch);
        }
    }

    private void ClearLists()
    {
        for (int idx = 0; idx < AvailableTriggerList.Count; idx++)
        {
            Destroy(AvailableTriggerList[idx]);
        }
        AvailableTriggerList = new List<GameObject>();
        for (int idx = 0; idx < SelectedTriggerList.Count; idx++)
        {
            Destroy(SelectedTriggerList[idx]);
        }
        SelectedTriggerList = new List<GameObject>();
    }

    public void UpdateCutsceneList()
    {
        ClearLists();
        foreach (string label in GridCrafter.CutsceneDataManager.CutsceneCollection.Keys)
        {
            if (Label == label) continue;
            CutsceneTriggerInfo trigger = GridCrafter.CutsceneDataManager.GetTrigger(label);
            GameObject triggerInfoItem = Instantiate(TriggerItem);
            triggerInfoItem.GetComponent<PreTriggerItemScript>().AvailableUpdate(label, trigger.TriggerLimit);
            triggerInfoItem.GetComponent<PreTriggerItemScript>().ParentScript = this;

            AvailableTriggerList.Add(triggerInfoItem);
        }
        UpdateListPositions();
    }

    public void UpdateListPositions()
    {
        float contentHeight = 100 * AvailableTriggerList.Count;
        AvailableContentScreen.GetComponent<RectTransform>().sizeDelta = new Vector2(826, contentHeight);
        for (int idx = 0; idx < AvailableTriggerList.Count; idx++)
        {
            GameObject triggerInfoItem = AvailableTriggerList[idx];
            triggerInfoItem.transform.SetParent(AvailableContentScreen.transform);
            triggerInfoItem.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(0, idx * 100f - contentHeight / 2f + 50, 0);
        }

        contentHeight = 100 * SelectedTriggerList.Count;
        SelectedContentScreen.GetComponent<RectTransform>().sizeDelta = new Vector2(826, contentHeight);
        for (int idx = 0; idx < SelectedTriggerList.Count; idx++)
        {
            GameObject triggerInfoItem = SelectedTriggerList[idx];
            triggerInfoItem.transform.SetParent(SelectedContentScreen.transform);
            triggerInfoItem.GetComponent<RectTransform>().anchoredPosition =
                new Vector3(0, idx * 100f - contentHeight / 2f + 50, 0);
        }
    }

    public void SwitchSide(GameObject sourceObject)
    {
        if (AvailableTriggerList.Contains(sourceObject))
        {
            AvailableTriggerList.Remove(sourceObject);
            SelectedTriggerList.Add(sourceObject);
            sourceObject.GetComponent<PreTriggerItemScript>().DisableChanges();
            UpdateListPositions();
            return;
        }
        SelectedTriggerList.Remove(sourceObject);
        AvailableTriggerList.Add(sourceObject);
        sourceObject.GetComponent<PreTriggerItemScript>().EnableChanges();
        UpdateListPositions();
    }

    public void SubmitPreCondition()
    {
        List<PreTriggerInfo> PreTriggerInfoList = new List<PreTriggerInfo>();
        foreach (GameObject triggerInfoItem in SelectedTriggerList)
        {
            PreTriggerItemScript triggerInfoScript = triggerInfoItem.GetComponent<PreTriggerItemScript>();
            PreTriggerInfo newPreTriggerInfo = new PreTriggerInfo();
            newPreTriggerInfo.TriggerName = triggerInfoScript.Label;
            newPreTriggerInfo.TriggersNeeded = Int32.Parse(triggerInfoScript.TriggersNeededCount.text);
            newPreTriggerInfo.Triggered = triggerInfoScript.TriggeredToggle.isOn;
            PreTriggerInfoList.Add(newPreTriggerInfo);
        }
        GridCrafter.CutsceneDataManager.GetTrigger(Label).PreTriggerConditions = PreTriggerInfoList;

        Close();
    }
}
