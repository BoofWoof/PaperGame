using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveTriggerLists
{
    public List<LowHealthTriggerInfo> LowHealthTriggerList = new List<LowHealthTriggerInfo>();
    public List<PushObjectTriggerInfo> PushObjectTriggerList = new List<PushObjectTriggerInfo>();
    public List<PlayerEnterTriggerInfo> PlayerEnterTriggerList = new List<PlayerEnterTriggerInfo>();
    public List<TurnsPassedTriggerInfo> TurnsPassedTriggerList = new List<TurnsPassedTriggerInfo>();
    public List<RenameObjectInfo> RenameObjectTriggerList = new List<RenameObjectInfo>();
}
