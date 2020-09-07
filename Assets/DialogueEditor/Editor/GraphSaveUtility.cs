using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine.UIElements;

public class GraphSaveUtility
{
    private DialogueGraphView _targetGraphView;
    private DialogueContainer _containerCache;

    private List<Edge> Edges => _targetGraphView.edges.ToList();
    private List<DialogueNode> DialogueNodes => _targetGraphView.nodes.ToList().Where(x => x is DialogueNode).Cast<DialogueNode>().ToList();
    private List<AnimationTriggerNode> AnimationTriggerNodes => _targetGraphView.nodes.ToList().Where(x => x is AnimationTriggerNode).Cast<AnimationTriggerNode>().ToList();
    private List<GetFlagNode> GetFlagNodes => _targetGraphView.nodes.ToList().Where(x => x is GetFlagNode).Cast<GetFlagNode>().ToList();
    private List<SetFlagNode> SetFlagNodes => _targetGraphView.nodes.ToList().Where(x => x is SetFlagNode).Cast<SetFlagNode>().ToList();
    private List<BooleanGetFlagNode> BooleanGetFlagNodes => _targetGraphView.nodes.ToList().Where(x => x is BooleanGetFlagNode).Cast<BooleanGetFlagNode>().ToList();
    private List<BooleanSetFlagNode> BooleanSetFlagNodes => _targetGraphView.nodes.ToList().Where(x => x is BooleanSetFlagNode).Cast<BooleanSetFlagNode>().ToList();
    private List<NodeTemplate> AllNodes;

    public static GraphSaveUtility GetInstance(DialogueGraphView targetGraphView)
    {
        return new GraphSaveUtility
        {
            _targetGraphView = targetGraphView
        };
    }

    public void SaveGraph(string filename)
    {
        var dialogueContainer = ScriptableObject.CreateInstance<DialogueContainer>();
        SaveNodeLinks(dialogueContainer);
        SaveDialogueNodes(dialogueContainer);
        SaveAnimationTriggerNodes(dialogueContainer);
        SaveGetFlagNodes(dialogueContainer);
        SaveSetFlagNodes(dialogueContainer);
        SaveBooleanGetFlagNodes(dialogueContainer);
        SaveBooleanSetFlagNodes(dialogueContainer);
        //SaveExposedProperties(dialogueContainer);
        var initialNode = DialogueNodes.First(node => node.EntryPoint);
        dialogueContainer.StartingGUID = initialNode.GUID;
            //dialogueContainer.NodeLinks.First(link => link.BaseNodeGuid == initialNode.GUID).TargetNodeGuid;

        AssetDatabase.CreateAsset(dialogueContainer, $"Assets/DialogueEditor/Resources/{filename}.asset");
        AssetDatabase.SaveAssets();
    }

    public void SaveNodeLinks(DialogueContainer dialogueContainer)
    {
        var connectedPorts = Edges.Where(x => x.input.node != null).ToArray();
        for (var i = 0; i < connectedPorts.Length; i++)
        {
            var outputNode = connectedPorts[i].output.node as NodeTemplate;
            var inputNode = connectedPorts[i].input.node as NodeTemplate;

            dialogueContainer.NodeLinks.Add(new NodeLinkData
            {
                BaseNodeGuid = outputNode.GUID,
                PortName = connectedPorts[i].output.portName,
                TargetNodeGuid = inputNode.GUID
            });
        }
    }

    public void SaveDialogueNodes(DialogueContainer dialogueContainer)
    {
        foreach (var dialogueNode in DialogueNodes.Where(node => !node.EntryPoint))
        {
            dialogueContainer.DialogueNodeData.Add(new DialogueNodeData
            {
                Guid = dialogueNode.GUID,
                DialogueText = dialogueNode.DialogueText,
                TargetPlayer = dialogueNode.TargetPlayer,
                Position = dialogueNode.GetPosition().position
            });
        }

        return;
    }

    public void SaveAnimationTriggerNodes(DialogueContainer dialogueContainer)
    {
        foreach (var animationTriggerNode in AnimationTriggerNodes.Where(node => !node.EntryPoint))
        {
            dialogueContainer.AnimationTriggerNodeData.Add(new AnimationTriggerNodeData
            {
                Guid = animationTriggerNode.GUID,
                TriggerName = animationTriggerNode.TriggerName,
                TargetPlayer = animationTriggerNode.TargetPlayer,
                Position = animationTriggerNode.GetPosition().position
            });
        }

        return;
    }

    public void SaveGetFlagNodes(DialogueContainer dialogueContainer)
    {
        foreach (var getFlagNode in GetFlagNodes.Where(node => !node.EntryPoint))
        {
            dialogueContainer.GetFlagNodeData.Add(new GetFlagNodeData
            {
                Guid = getFlagNode.GUID,
                FlagName = getFlagNode.FlagName,
                Position = getFlagNode.GetPosition().position
            });
        }

        return;
    }

    public void SaveSetFlagNodes(DialogueContainer dialogueContainer)
    {
        foreach (var setFlagNode in SetFlagNodes.Where(node => !node.EntryPoint))
        {
            dialogueContainer.SetFlagNodeData.Add(new SetFlagNodeData
            {
                Guid = setFlagNode.GUID,
                FlagName = setFlagNode.FlagName,
                FlagTag = setFlagNode.FlagTag,
                Position = setFlagNode.GetPosition().position
            });
        }

        return;
    }

    public void SaveBooleanGetFlagNodes(DialogueContainer dialogueContainer)
    {
        foreach (var booleanGetFlagNode in BooleanGetFlagNodes.Where(node => !node.EntryPoint))
        {
            dialogueContainer.BooleanGetFlagNodeData.Add(new BooleanGetFlagNodeData
            {
                Guid = booleanGetFlagNode.GUID,
                FlagName = booleanGetFlagNode.FlagName,
                Position = booleanGetFlagNode.GetPosition().position
            });
        }

        return;
    }

    public void SaveBooleanSetFlagNodes(DialogueContainer dialogueContainer)
    {
        foreach (var booleanSetFlagNode in BooleanSetFlagNodes.Where(node => !node.EntryPoint))
        {
            dialogueContainer.BooleanSetFlagNodeData.Add(new BooleanSetFlagNodeData
            {
                Guid = booleanSetFlagNode.GUID,
                FlagName = booleanSetFlagNode.FlagName,
                FlagBool = booleanSetFlagNode.FlagBool,
                Position = booleanSetFlagNode.GetPosition().position
            });
        }

        return;
    }

    public void LoadGraph(string filename)
    {
        _containerCache = Resources.Load<DialogueContainer>(filename);
        if (_containerCache == null)
        {
            EditorUtility.DisplayDialog("File Not Found", "Target dialogue graph file does not exist!", "OK");
        }

        ClearGraph();
        CreateDialogueNodes();
        CreateAnimationTriggerNodes();
        CreateGetFlagNodes();
        CreateSetFlagNodes();
        CreateBooleanGetFlagNodes();
        CreateBooleanSetFlagNodes();
        AllNodes = CombineAllLists();
        ConnectNodes();
        //CreateExposedProperties();
    }

    private void CreateExposedProperties()
    {
        _targetGraphView.ClearBlackBoardAndExposedProperties();
        foreach(var exposedProperty in _containerCache.ExposedProperties)
        {
            _targetGraphView.AddPropertyToBlackBoard(exposedProperty);
        }
    }

    private void ClearGraph()
    {
        DialogueNodes.Find(x => x.EntryPoint).GUID = _containerCache.StartingGUID;

        foreach (var node in DialogueNodes)
        {
            if (node.EntryPoint) continue;
            Edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));
            _targetGraphView.RemoveElement(node);
        }
        foreach (var node in AnimationTriggerNodes)
        {
            if (node.EntryPoint) continue;
            Edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));
            _targetGraphView.RemoveElement(node);
        }
        foreach (var node in GetFlagNodes)
        {
            if (node.EntryPoint) continue;
            Edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));
            _targetGraphView.RemoveElement(node);
        }
        foreach (var node in SetFlagNodes)
        {
            if (node.EntryPoint) continue;
            Edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));
            _targetGraphView.RemoveElement(node);
        }
        foreach (var node in BooleanGetFlagNodes)
        {
            if (node.EntryPoint) continue;
            Edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));
            _targetGraphView.RemoveElement(node);
        }
        foreach (var node in BooleanSetFlagNodes)
        {
            if (node.EntryPoint) continue;
            Edges.Where(x => x.input.node == node).ToList().ForEach(edge => _targetGraphView.RemoveElement(edge));
            _targetGraphView.RemoveElement(node);
        }
    }

    private void CreateDialogueNodes()
    {
        foreach (var nodeData in _containerCache.DialogueNodeData)
        {
            var tempNode = _targetGraphView.CreateDialogueNode(nodeData.DialogueText, nodeData.TargetPlayer, Vector2.zero);
            tempNode.GUID = nodeData.Guid;
            _targetGraphView.AddElement(tempNode);

            var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
            nodePorts.ForEach(x => _targetGraphView.AddChoicePort(tempNode, x.PortName));

            tempNode.SetPosition(new Rect(
                nodeData.Position,
                _targetGraphView.defaultNodeSize
                ));
        }
    }

    private void CreateAnimationTriggerNodes()
    {
        foreach (var nodeData in _containerCache.AnimationTriggerNodeData)
        {
            var tempNode = _targetGraphView.CreateAnimationTriggerNode(nodeData.TriggerName, nodeData.TargetPlayer, Vector2.zero);
            tempNode.GUID = nodeData.Guid;
            _targetGraphView.AddElement(tempNode);

            tempNode.SetPosition(new Rect(
                nodeData.Position,
                _targetGraphView.defaultNodeSize
                ));
        }
    }

    private void CreateGetFlagNodes()
    {
        foreach (var nodeData in _containerCache.GetFlagNodeData)
        {
            var tempNode = _targetGraphView.CreateGetFlagNode(nodeData.FlagName, Vector2.zero);
            tempNode.GUID = nodeData.Guid;
            _targetGraphView.AddElement(tempNode);

            var nodePorts = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == nodeData.Guid).ToList();
            nodePorts.ForEach(x => {
                if (x.PortName != "Other") _targetGraphView.AddChoicePort(tempNode, x.PortName);
            });

            tempNode.SetPosition(new Rect(
                nodeData.Position,
                _targetGraphView.defaultNodeSize
                ));
        }
    }

    private void CreateSetFlagNodes()
    {
        foreach (var nodeData in _containerCache.SetFlagNodeData)
        {
            var tempNode = _targetGraphView.CreateSetFlagNode(nodeData.FlagTag, nodeData.FlagName, Vector2.zero);
            tempNode.GUID = nodeData.Guid;
            _targetGraphView.AddElement(tempNode);

            tempNode.SetPosition(new Rect(
                nodeData.Position,
                _targetGraphView.defaultNodeSize
                ));
        }
    }

    private void CreateBooleanGetFlagNodes()
    {
        foreach (var nodeData in _containerCache.BooleanGetFlagNodeData)
        {
            var tempNode = _targetGraphView.CreateBoolGetFlagNode(nodeData.FlagName, Vector2.zero);
            tempNode.GUID = nodeData.Guid;
            _targetGraphView.AddElement(tempNode);

            tempNode.SetPosition(new Rect(
                nodeData.Position,
                _targetGraphView.defaultNodeSize
                ));
        }
    }

    private void CreateBooleanSetFlagNodes()
    {
        foreach (var nodeData in _containerCache.BooleanSetFlagNodeData)
        {
            var tempNode = _targetGraphView.CreateBoolSetFlagNode(nodeData.FlagBool, nodeData.FlagName, Vector2.zero);
            tempNode.GUID = nodeData.Guid;
            _targetGraphView.AddElement(tempNode);

            tempNode.SetPosition(new Rect(
                nodeData.Position,
                _targetGraphView.defaultNodeSize
                ));
        }
    }

    private void ConnectNodes()
    {
        for (var i = 0; i < AllNodes.Count; i++)
        {
            var connections = _containerCache.NodeLinks.Where(x => x.BaseNodeGuid == AllNodes[i].GUID).ToList();
            for (var j = 0; j < connections.Count; j++)
            {
                var targetNodeGuid = connections[j].TargetNodeGuid;
                var targetNode = AllNodes.First(x => x.GUID == targetNodeGuid);
                
                LinkNodes(AllNodes[i].outputContainer.Q<Port>(name: connections[j].PortName), (Port)targetNode.inputContainer[0]);
            }
        }
    }

    private void LinkNodes(Port output, Port input)
    {
        var tempEdge = new Edge
        {
            output = output,
            input = input
        };

        tempEdge.input.Connect(tempEdge);
        tempEdge.output.Connect(tempEdge);
        _targetGraphView.Add(tempEdge);
    }

    private void SaveExposedProperties(DialogueContainer dialogueContainer)
    {
        dialogueContainer.ExposedProperties.AddRange(_targetGraphView.ExposedProperties);
    }

    private List<NodeTemplate> CombineAllLists()
    {
        List<NodeTemplate> tempAllList = new List<NodeTemplate>();
        foreach (var node in DialogueNodes) tempAllList.Add(node);
        foreach (var node in AnimationTriggerNodes) tempAllList.Add(node);
        foreach (var node in SetFlagNodes) tempAllList.Add(node);
        foreach (var node in GetFlagNodes) tempAllList.Add(node);
        foreach (var node in BooleanSetFlagNodes) tempAllList.Add(node);
        foreach (var node in BooleanGetFlagNodes) tempAllList.Add(node);
        return tempAllList;
    }
}
