using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CutsceneTriggerInfo
{
    public string CutscenePath;
    public string Label;
    public int TriggerLimit;
    public string GridLayer;
    public List<Vector2Int> TargetPositions;
}
