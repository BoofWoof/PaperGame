using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTriggerManagerScript
{
    /*
     Cutscene type names.
     LowHealthTriggers "lowhealth"
         */
    // <label, (type, index)>
    public Dictionary<string, (List<CutsceneTriggerInfo>, int, List<GridObject>)> CutsceneCollection = new Dictionary<string, (List<CutsceneTriggerInfo>, int, List<GridObject>)>();
    public Dictionary<string, int> TriggerCount = new Dictionary<string, int>();

    public List<CutsceneTriggerInfo> LowHealthTriggers = new List<CutsceneTriggerInfo>();
    public List<CutsceneTriggerInfo> PushObjectTriggers = new List<CutsceneTriggerInfo>();
    public List<CutsceneTriggerInfo> PlayerEnterTriggers = new List<CutsceneTriggerInfo>();
    public List<CutsceneTriggerInfo> TurnsPassedTriggers = new List<CutsceneTriggerInfo>();
    public List<CutsceneTriggerInfo> RenameObjectTriggers = new List<CutsceneTriggerInfo>();

    public bool TriggerATrigger(string label)
    {
        (List<CutsceneTriggerInfo> triggerList, int triggerIdx, _) = CutsceneCollection[label];
        int triggerLimit = triggerList[triggerIdx].TriggerLimit;
        foreach(PreTriggerInfo preTriggerInfo in triggerList[triggerIdx].PreTriggerConditions)
        {
            CutsceneTriggerInfo preTrigger = GetTrigger(preTriggerInfo.TriggerName);
            if (preTriggerInfo.Triggered)
            {
                if (TriggerCount[preTrigger.Label] < preTriggerInfo.TriggersNeeded) return false;
            } else
            {
                if (TriggerCount[preTrigger.Label] >= preTriggerInfo.TriggersNeeded) return false;
            }
        }
        if (triggerLimit == 0) return true;
        int triggerCount = TriggerCount[label];
        if (triggerCount < triggerLimit)
        {
            TriggerCount[label] += 1;
            return true;
        }
        return false;
    }

    public void AddRenameObjectTrigger(RenameObjectInfo trigger, List<GridObject> targetObjects)
    {
        if (CutsceneCollection.ContainsKey(trigger.Label))
        {
            RemoveTrigger(trigger.Label);
        }
        foreach (CombatObject targetObject in targetObjects)
        {
            targetObject.ObjectInfo.ObjectName = trigger.Label;
        }
        CutsceneCollection.Add(trigger.Label, (RenameObjectTriggers, RenameObjectTriggers.Count, targetObjects));
        RenameObjectTriggers.Add(trigger);
    }

    public void AddLowHealthTrigger(LowHealthTriggerInfo trigger, List<GridObject> targetCharacters)
    {
        if (CutsceneCollection.ContainsKey(trigger.Label))
        {
            RemoveTrigger(trigger.Label);
            TriggerCount.Remove(trigger.Label);
        }
        foreach (FighterClass targetCharacter in targetCharacters)
        {
            targetCharacter.LowHealthTriggers.Add(trigger);
        }
        CutsceneCollection.Add(trigger.Label, (LowHealthTriggers, LowHealthTriggers.Count, targetCharacters));
        LowHealthTriggers.Add(trigger);
        TriggerCount.Add(trigger.Label, 0);
    }

    public void AddPushObjectTrigger(PushObjectTriggerInfo trigger, List<GridObject> targetObjects)
    {
        if (CutsceneCollection.ContainsKey(trigger.Label))
        {
            RemoveTrigger(trigger.Label);
            TriggerCount.Remove(trigger.Label);
        }
        foreach (CombatObject targetObject in targetObjects)
        {
            targetObject.PushObjectTriggers.Add(trigger);
        }
        CutsceneCollection.Add(trigger.Label, (PushObjectTriggers, PushObjectTriggers.Count, targetObjects));
        PushObjectTriggers.Add(trigger);
        TriggerCount.Add(trigger.Label, 0);
    }

    public void AddPlayerEnterTrigger(PlayerEnterTriggerInfo trigger, List<GridObject> targetTiles)
    {
        if (CutsceneCollection.ContainsKey(trigger.Label))
        {
            RemoveTrigger(trigger.Label);
            TriggerCount.Remove(trigger.Label);
        }
        foreach (BlockTemplate targetTile in targetTiles)
        {
            targetTile.PlayerEnterTriggers.Add(trigger);
        }
        CutsceneCollection.Add(trigger.Label, (PlayerEnterTriggers, PlayerEnterTriggers.Count, targetTiles));
        PlayerEnterTriggers.Add(trigger);
        TriggerCount.Add(trigger.Label, 0);
    }

    public void AddTurnsPassedTrigger(TurnsPassedTriggerInfo trigger)
    {
        if (CutsceneCollection.ContainsKey(trigger.Label))
        {
            RemoveTrigger(trigger.Label);
        }
        CutsceneCollection.Add(trigger.Label, (TurnsPassedTriggers, TurnsPassedTriggers.Count, null));
        TurnsPassedTriggers.Add(trigger);
        TriggerCount.Add(trigger.Label, 0);
    }

    public void UpdateTrigger(string originalLabel, string newLabel, List<GridObject> targetCharacters)
    {
        (List<CutsceneTriggerInfo> triggerList, int triggerIdx, _) = CutsceneCollection[originalLabel];
        CutsceneCollection.Remove(originalLabel);
        CutsceneCollection.Add(newLabel, (triggerList, triggerIdx, targetCharacters));
    }

    public CutsceneTriggerInfo GetTrigger(string label)
    {
        (List<CutsceneTriggerInfo> triggerList, int triggerIdx, List<GridObject> targetCharacters) = CutsceneCollection[label];
        
        return triggerList[triggerIdx];
    }

    public List<GridObject> GetTargets(string label)
    {
        (List<CutsceneTriggerInfo> triggerList, int triggerIdx, List<GridObject> targetCharacters) = CutsceneCollection[label];
        return targetCharacters;
    }

    public int GetTargetCount(string label)
    {
        (List<CutsceneTriggerInfo> triggerList, int triggerIdx, List<GridObject> targetList) = CutsceneCollection[label];
        if (targetList == null) return -1;
        if (!RemoveNullFromList(label)) return -1;
        return targetList.Count;
    }

    public bool RemoveNullFromList(string label)
    {
        (List<CutsceneTriggerInfo> triggerList, int triggerIdx, List<GridObject> targetList) = CutsceneCollection[label];
        if (targetList == null) return true;
        targetList.RemoveAll(trigger => trigger == null);
        if(targetList.Count == 0)
        {
            RemoveTrigger(label);
            return false;
        }
        return true;
    }

    public void RemoveTrigger(string label)
    {
        (List<CutsceneTriggerInfo> triggerList, int triggerIdx, List<GridObject> targetCharacters) = CutsceneCollection[label];
        RemoveAndUpdateList(triggerList, triggerIdx);
        CutsceneCollection.Remove(label);
    }

    public void RemoveAndUpdateList(List<CutsceneTriggerInfo> triggerList, int removeIdx)
    {
        triggerList.RemoveAt(removeIdx);
        for (int i = triggerList.Count - 1; i >= removeIdx; i--)
        {
            (_, _, List<GridObject> targetCharacters) = CutsceneCollection[triggerList[i].Label];
            CutsceneCollection[triggerList[i].Label] = (triggerList, i, targetCharacters);
        }
    }

    public List<(string, string)> TriggersContainingObject(GridObject searchObject)
    {
        List<(string, string)> cutsceneLabels = new List<(string, string)>();
        foreach (string key in CutsceneCollection.Keys)
        {
            (List<CutsceneTriggerInfo> triggerList, int triggerIdx, List<GridObject> targetCharacters) = CutsceneCollection[key];
            if (targetCharacters == null) {
                cutsceneLabels.Add((key, triggerList[0].GetType().ToString()));
                continue;
            } 
            if (targetCharacters.Contains(searchObject)) cutsceneLabels.Add((key, triggerList[0].GetType().ToString()));
        }
        return cutsceneLabels;
    }

    public void UpdateTargetPositions()
    {
        foreach (string label in CutsceneCollection.Keys)
        {
            if(!RemoveNullFromList(label)) continue;
            List<Vector2Int> targetPos = new List<Vector2Int>();
            (List<CutsceneTriggerInfo> triggerList, int triggerIdx, List<GridObject> targetGridObjects) = CutsceneCollection[label];
            if (targetGridObjects == null) continue;
            foreach (GridObject targetGridObject in targetGridObjects)
            {
                targetPos.Add(targetGridObject.pos);
            }
            triggerList[triggerIdx].TargetPositions = targetPos;
        }
    }

    private List<GridObject> GetCharactersAtPos(List<Vector2Int> targetPos, string gridLayer)
    {
        List<GridObject> targetList = new List<GridObject>();
        foreach (Vector2Int pos in targetPos)
        {
            if (gridLayer == "Character")
            {
                targetList.Add(GridCrafter.characterGrid[pos.x,pos.y].GetComponent<GridObject>());
            }
            else if (gridLayer == "Object")
            {
                targetList.Add(GridCrafter.objectGrid[pos.x, pos.y].GetComponent<GridObject>());
            }
            else if (gridLayer == "Tile")
            {
                targetList.Add(GridCrafter.blockGrid[pos.x, pos.y].GetComponent<GridObject>());
            }
        }
        return targetList;
    }

    public SaveTriggerLists SaveTriggers()
    {
        SaveTriggerLists datacache = new SaveTriggerLists();
        UpdateTargetPositions();
        foreach (LowHealthTriggerInfo lowHealthTrigger in LowHealthTriggers)
        {
            datacache.LowHealthTriggerList.Add(lowHealthTrigger);
        }
        foreach (PushObjectTriggerInfo pushObjectTrigger in PushObjectTriggers)
        {
            datacache.PushObjectTriggerList.Add(pushObjectTrigger);
        }
        foreach (PlayerEnterTriggerInfo playerEnterTrigger in PlayerEnterTriggers)
        {
            datacache.PlayerEnterTriggerList.Add(playerEnterTrigger);
        }
        foreach (TurnsPassedTriggerInfo turnsPassedObjectTrigger in TurnsPassedTriggers)
        {
            datacache.TurnsPassedTriggerList.Add(turnsPassedObjectTrigger);
        }
        foreach (RenameObjectInfo renameObjectTrigger in RenameObjectTriggers)
        {
            datacache.RenameObjectTriggerList.Add(renameObjectTrigger);
        }
        return datacache;
    }

    public void LoadTriggers(SaveTriggerLists datacache)
    {
        foreach (CutsceneTriggerInfo lowHealthTrigger in datacache.LowHealthTriggerList)
        {
            AddLowHealthTrigger((LowHealthTriggerInfo)lowHealthTrigger, GetCharactersAtPos(lowHealthTrigger.TargetPositions, lowHealthTrigger.GridLayer));
        }
        foreach (CutsceneTriggerInfo pushObjectTrigger in datacache.PushObjectTriggerList)
        {
            AddPushObjectTrigger((PushObjectTriggerInfo)pushObjectTrigger, GetCharactersAtPos(pushObjectTrigger.TargetPositions, pushObjectTrigger.GridLayer));
        }
        foreach (CutsceneTriggerInfo playerEnterTrigger in datacache.PlayerEnterTriggerList)
        {
            AddPlayerEnterTrigger((PlayerEnterTriggerInfo)playerEnterTrigger, GetCharactersAtPos(playerEnterTrigger.TargetPositions, playerEnterTrigger.GridLayer));
        }
        foreach (CutsceneTriggerInfo turnsPassedTrigger in datacache.TurnsPassedTriggerList)
        {
            AddTurnsPassedTrigger((TurnsPassedTriggerInfo)turnsPassedTrigger);
        }
        foreach (CutsceneTriggerInfo renameObjectTrigger in datacache.RenameObjectTriggerList)
        {
            AddRenameObjectTrigger((RenameObjectInfo)renameObjectTrigger, GetCharactersAtPos(renameObjectTrigger.TargetPositions, renameObjectTrigger.GridLayer));
        }
    }
}
