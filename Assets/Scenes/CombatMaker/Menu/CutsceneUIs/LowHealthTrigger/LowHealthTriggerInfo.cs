using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LowHealthTriggerInfo: CutsceneTriggerInfo
{
    public int TriggerValue;
    public List<Vector2Int> TargetPositions;
}
