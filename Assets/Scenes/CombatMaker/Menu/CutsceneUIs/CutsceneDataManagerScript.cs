using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatTriggerType {LowHealth};

public class CutsceneDataManagerScript
{
    /*
     Cutscene type names.
     LowHealthTriggers "lowhealth"
         */
    // <label, (type, index)>
    public Dictionary<string, (CombatTriggerType, int, List<GridObject>)> CutsceneCollection = new Dictionary<string, (CombatTriggerType, int, List<GridObject>)>();

    public List<CutsceneTriggerInfo> LowHealthTriggers = new List<CutsceneTriggerInfo>();

    public void AddLowHealthTrigger(LowHealthTriggerInfo trigger, List<GridObject> targetCharacters)
    {
        CutsceneCollection.Add(trigger.Label, (CombatTriggerType.LowHealth, LowHealthTriggers.Count, targetCharacters));
        LowHealthTriggers.Add(trigger);
    }

    public CutsceneTriggerInfo GetTrigger(string label)
    {
        (CombatTriggerType triggerType, int triggerIdx, List<GridObject> targetCharacters) = CutsceneCollection[label];

        if(triggerType == CombatTriggerType.LowHealth)
        {
            return LowHealthTriggers[triggerIdx];
        }
        return null;
    }

    public int GetTargetCount(string label)
    {
        (CombatTriggerType triggerType, int triggerIdx, List<GridObject> targetCharacters) = CutsceneCollection[label];
        return targetCharacters.Count;
    }

    public void RemoveTrigger(string label)
    {
        (CombatTriggerType triggerType, int triggerIdx, List<GridObject> targetCharacters) = CutsceneCollection[label];
        if(triggerType == CombatTriggerType.LowHealth)
        {
            RemoveAndUpdateList(LowHealthTriggers, triggerIdx, CombatTriggerType.LowHealth);
        }
        CutsceneCollection.Remove(label);
    }

    public void RemoveAndUpdateList(List<CutsceneTriggerInfo> triggerList, int removeIdx, CombatTriggerType triggerType)
    {
        LowHealthTriggers.RemoveAt(removeIdx);
        for (int i = LowHealthTriggers.Count - 1; i >= removeIdx; i--)
        {
            (_, _, List<GridObject> targetCharacters) = CutsceneCollection[LowHealthTriggers[i].Label];
            CutsceneCollection[LowHealthTriggers[i].Label] = (triggerType, i, targetCharacters);
        }
    }

    public List<(string, CombatTriggerType)> TriggersContainingObject(GridObject searchObject)
    {
        List<(string, CombatTriggerType)> cutsceneLabels = new List<(string, CombatTriggerType)>();
        foreach (string key in CutsceneCollection.Keys)
        {
            (CombatTriggerType triggerType, int triggerIdx, List<GridObject> targetCharacters) = CutsceneCollection[key];
            if (targetCharacters.Contains(searchObject)) cutsceneLabels.Add((key, triggerType));
        }
        return cutsceneLabels;
    }
}
