using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CutsceneDeconstruct: ScriptableObject
{
    DialogueContainer DialogueInput;
    string currentGUID;
    public string nextGUID = string.Empty;

    GameObject cutsceneSource;
    //Needed For Dialogue
    private string speakerName;
    bool textboxWait = false;
    private GameObject spawnedTextBox;

    //DataTracking

    public void Deconstruct(DialogueContainer input, string name, GameObject source)
    {
        currentGUID = FindNextNode(input, input.StartingGUID);
        DialogueInput = input;
        speakerName = name;
        cutsceneSource = source;
        GraphDeconstruct(DialogueInput);
    }

    public bool Update()
    {
        if (!string.IsNullOrEmpty(nextGUID))
        {
            currentGUID = nextGUID;
            textboxWait = false;
        }
        nextGUID = string.Empty;
        return GraphDeconstruct(DialogueInput);
    }

    private bool GraphDeconstruct(DialogueContainer input)
    {
        while (!string.IsNullOrEmpty(currentGUID) && !textboxWait)
        {
            //Dialogue Node
            if (input.DialogueNodeData.Any(x => x.Guid == currentGUID))
            {
                DialogueNodeData node = input.DialogueNodeData.First(x => x.Guid == currentGUID);
                textboxWait = true;

                SayDialogue dialogueCutscene = ScriptableObject.CreateInstance<SayDialogue>();
                if (!string.IsNullOrEmpty(node.TargetPlayer)){
                    dialogueCutscene.speakerName = node.TargetPlayer;
                } else {
                    dialogueCutscene.speakerName = speakerName;
                }
                dialogueCutscene.inputText = new TextAsset(node.DialogueText);
                dialogueCutscene.currentLinks = input.NodeLinks.Where(x => x.BaseNodeGuid == currentGUID).ToList();
                dialogueCutscene.deconstructerSource = this;
                CutsceneController.addCutsceneEvent(dialogueCutscene, cutsceneSource, true, OverworldController.gameModeOptions.Cutscene);
                continue;
            }
            //Animation Trigger
            if (input.AnimationTriggerNodeData.Any(x => x.Guid == currentGUID))
            {
                AnimationTriggerNodeData node = input.AnimationTriggerNodeData.First(x => x.Guid == currentGUID);

                AnimationTriggerCutscene animationTriggerCutscene = ScriptableObject.CreateInstance<AnimationTriggerCutscene>();
                animationTriggerCutscene.TriggerName = node.TriggerName;
                animationTriggerCutscene.TargetName = node.TargetPlayer;

                CutsceneController.addCutsceneEvent(animationTriggerCutscene, cutsceneSource, false, OverworldController.gameModeOptions.Cutscene);
                currentGUID = FindNextNode(input, currentGUID);
                continue;
            }
            //Set Flag
            if (input.SetFlagNodeData.Any(x => x.Guid == currentGUID))
            {
                SetFlagNodeData SetFlagNode = input.SetFlagNodeData.First(x => x.Guid == currentGUID);
                string FlagName = SetFlagNode.FlagName;
                string FlagTag = SetFlagNode.FlagTag;
                if (GameDataTracker.stringFlags.ContainsKey(FlagName))
                {
                    GameDataTracker.stringFlags[FlagName] = FlagTag;
                } else
                {
                    GameDataTracker.stringFlags.Add(FlagName, FlagTag);
                }
                currentGUID = FindNextNode(input, currentGUID);
                continue;
            }
            //Get Flag
            if (input.GetFlagNodeData.Any(x => x.Guid == currentGUID))
            {
                GetFlagNodeData GetFlagNode = input.GetFlagNodeData.First(x => x.Guid == currentGUID);
                string FlagName = GetFlagNode.FlagName;
                if (GameDataTracker.stringFlags.ContainsKey(FlagName))
                {
                    string FlagTag = GameDataTracker.stringFlags[FlagName];
                    if(input.NodeLinks.Any(x => x.PortName == FlagTag))
                    {
                        currentGUID = input.NodeLinks.First(x => x.PortName == FlagTag && x.BaseNodeGuid == currentGUID).TargetNodeGuid;
                    } else
                    {
                        currentGUID = input.NodeLinks.First(x => x.PortName == "Other" && x.BaseNodeGuid == currentGUID).TargetNodeGuid;
                    }
                } else
                {
                    currentGUID = input.NodeLinks.First(x => x.PortName == "Other" && x.BaseNodeGuid == currentGUID).TargetNodeGuid;
                }
                continue;
            }
            //Boolean Set Flag
            if (input.BooleanSetFlagNodeData.Any(x => x.Guid == currentGUID))
            {
                BooleanSetFlagNodeData BooleanSetFlagNode = input.BooleanSetFlagNodeData.First(x => x.Guid == currentGUID);
                string FlagName = BooleanSetFlagNode.FlagName;
                bool FlagBool = BooleanSetFlagNode.FlagBool;
                if (GameDataTracker.boolFlags.ContainsKey(FlagName))
                {
                    GameDataTracker.boolFlags[FlagName] = FlagBool;
                } else
                {
                    GameDataTracker.boolFlags.Add(FlagName, FlagBool);
                }
                currentGUID = FindNextNode(input, currentGUID);
                continue;
            }
            //Boolean Get Flag
            if (input.BooleanGetFlagNodeData.Any(x => x.Guid == currentGUID))
            {
                BooleanGetFlagNodeData BooleanGetFlagNode = input.BooleanGetFlagNodeData.First(x => x.Guid == currentGUID);
                string FlagName = BooleanGetFlagNode.FlagName;
                if (GameDataTracker.boolFlags.ContainsKey(FlagName))
                {
                    bool FlagBool = GameDataTracker.boolFlags[FlagName];
                    if (FlagBool)
                    {
                        currentGUID = input.NodeLinks.First(x => x.PortName == "True" && x.BaseNodeGuid == currentGUID).TargetNodeGuid;
                    }
                    else
                    {
                        currentGUID = input.NodeLinks.First(x => x.PortName == "False" && x.BaseNodeGuid == currentGUID).TargetNodeGuid;
                    }
                }
                else
                {
                    currentGUID = input.NodeLinks.First(x => x.PortName == "Other" && x.BaseNodeGuid == currentGUID).TargetNodeGuid;
                }
                continue;
            }
            //Move To Position
            if (input.MoveToPosNodeData.Any(x => x.Guid == currentGUID))
            {
                MoveToPosNodeData MoveToPosNode = input.MoveToPosNodeData.First(x => x.Guid == currentGUID);
                string TargetObject = MoveToPosNode.TargetObject;
                string ReferenceObject = MoveToPosNode.ReferenceObject;
                bool Wait = MoveToPosNode.Wait;
                Vector3 Offset = MoveToPosNode.PosOffset;

                MoveToPosition moveToPosition = ScriptableObject.CreateInstance<MoveToPosition>();
                moveToPosition.PositionOffset = Offset;
                moveToPosition.ReferenceObject = ReferenceObject;
                moveToPosition.TargetObject = TargetObject;
                moveToPosition.Wait = Wait;

                CutsceneController.addCutsceneEvent(moveToPosition, cutsceneSource, true, OverworldController.gameModeOptions.Cutscene);
                currentGUID = FindNextNode(input, currentGUID);
                continue;
            }
        }
        if(!textboxWait && string.IsNullOrEmpty(currentGUID))
        {
            return true;
        }
        return false;
    }

    private string FindNextNode(DialogueContainer input, string currentGUID)
    {
        if (input.NodeLinks.Any(x => x.BaseNodeGuid == currentGUID)){
            return input.NodeLinks.First(x => x.BaseNodeGuid == currentGUID).TargetNodeGuid;
        }
        return string.Empty;
    }
}
