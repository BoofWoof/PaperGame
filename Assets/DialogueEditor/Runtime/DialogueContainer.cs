using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueContainer: ScriptableObject
{
    public string StartingGUID;
    public List<NodeLinkData> NodeLinks = new List<NodeLinkData>();
    public List<DialogueNodeData> DialogueNodeData = new List<DialogueNodeData>();
    public List<AnimationTriggerNodeData> AnimationTriggerNodeData = new List<AnimationTriggerNodeData>();
    public List<GetFlagNodeData> GetFlagNodeData = new List<GetFlagNodeData>();
    public List<SetFlagNodeData> SetFlagNodeData = new List<SetFlagNodeData>();
    public List<BooleanGetFlagNodeData> BooleanGetFlagNodeData = new List<BooleanGetFlagNodeData>();
    public List<BooleanSetFlagNodeData> BooleanSetFlagNodeData = new List<BooleanSetFlagNodeData>();
    public List<MoveToPosNodeData> MoveToPosNodeData = new List<MoveToPosNodeData>(); 
    public List<ExposedProperty> ExposedProperties = new List<ExposedProperty>();
}
