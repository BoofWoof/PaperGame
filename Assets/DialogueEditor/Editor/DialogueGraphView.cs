using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

public class DialogueGraphView : GraphView
{
    public readonly Vector2 defaultNodeSize = new Vector2(150, 200);

    public Blackboard Blackboard;
    public List<ExposedProperty> ExposedProperties = new List<ExposedProperty>();
    private NodeSearchWindow _searchWindow;

    public DialogueGraphView(EditorWindow editorWindow)
    {
        StyleSheet ssheet = Resources.Load<StyleSheet>(path: "DialogueGraph");
        styleSheets.Add(styleSheet: ssheet);
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        var grid = new GridBackground();
        Insert(index: 0, grid);
        grid.StretchToParentSize();

        AddElement(GenerateEntryPointNode());
        AddSearchWindow(editorWindow);
    }

    private void AddSearchWindow(EditorWindow editorWindow)
    {
        _searchWindow = ScriptableObject.CreateInstance<NodeSearchWindow>();
        _searchWindow.Init(editorWindow, this);
        nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), _searchWindow);
    }

    private Port GeneratePort(Node node, Direction portDirection, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDirection, capacity, typeof(float)); //Arbitrary Type
    }

    private DialogueNode GenerateEntryPointNode()
    {
        var node = new DialogueNode
        {
            title = "START",
            GUID = Guid.NewGuid().ToString(),
            DialogueText = "ENTRYPOINT",
            EntryPoint = true
        };

        var generatedPort = GeneratePort(node, Direction.Output);
        generatedPort.portName = "Next";
        generatedPort.name = "Next";
        node.outputContainer.Add(generatedPort);

        node.capabilities &= ~Capabilities.Movable;
        node.capabilities &= ~Capabilities.Deletable;

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(100, 200, 100, 150));
        return node;
    }

    public DialogueNode CreateDialogueNode(string dialogue, string target, Vector2 mousePosition)
    {
        var dialogueNode = new DialogueNode
        {
            title = "DialogueNode",
            DialogueText = dialogue,
            TargetPlayer = target,
            GUID = Guid.NewGuid().ToString()
        };

        var inputPort = GeneratePort(dialogueNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        dialogueNode.inputContainer.Add(inputPort);

        dialogueNode.styleSheets.Add(Resources.Load<StyleSheet>("DialogueNode"));

        var button = new Button(clickEvent: () =>
        {
            AddChoicePort(dialogueNode);
        });
        button.text = "New Choice";
        dialogueNode.titleContainer.Add(button);

        var textFieldTarget = new TextField
        {
            name = "Target",
            value = target,
            label = "Target\n"
        };
        textFieldTarget.RegisterValueChangedCallback(evt =>
        {
            dialogueNode.TargetPlayer = evt.newValue;
        });
        dialogueNode.mainContainer.Add(textFieldTarget);

        var textField = new TextField
        {
            name = "Dialogue",
            value = dialogue,
            label = "Dialogue\n"
        };
        textField.RegisterValueChangedCallback(evt =>
        {
            dialogueNode.DialogueText = evt.newValue;
            //dialogueNode.title = evt.newValue;
        });
        //textField.SetValueWithoutNotify(dialogueNode.title);
        dialogueNode.mainContainer.Add(textField);

        dialogueNode.RefreshExpandedState();
        dialogueNode.RefreshPorts();
        dialogueNode.SetPosition(new Rect(mousePosition, defaultNodeSize));

        return dialogueNode;
    }

    public AnimationTriggerNode CreateAnimationTriggerNode(string trigger, string target, Vector2 mousePosition)
    {
        var animationTriggerNode = new AnimationTriggerNode
        {
            title = "AnimationTriggerNode",
            TriggerName = trigger,
            TargetPlayer = target,
            GUID = Guid.NewGuid().ToString()
        };

        var inputPort = GeneratePort(animationTriggerNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        animationTriggerNode.inputContainer.Add(inputPort);

        var generatedPort = GeneratePort(animationTriggerNode, Direction.Output);
        generatedPort.portName = "Next";
        generatedPort.name = "Next";
        animationTriggerNode.outputContainer.Add(generatedPort);

        animationTriggerNode.styleSheets.Add(Resources.Load<StyleSheet>("FlagNode"));

        var textFieldTarget = new TextField
        {
            name = "Target",
            value = target,
            label = "Target\n"
        };
        textFieldTarget.RegisterValueChangedCallback(evt =>
        {
            animationTriggerNode.TargetPlayer = evt.newValue;
        });
        animationTriggerNode.mainContainer.Add(textFieldTarget);

        var textField = new TextField
        {
            name = "Trigger",
            value = trigger,
            label = "Trigger\n"
        };
        textField.RegisterValueChangedCallback(evt =>
        {
            animationTriggerNode.TriggerName = evt.newValue;
            //dialogueNode.title = evt.newValue;
        });
        //textField.SetValueWithoutNotify(dialogueNode.title);
        animationTriggerNode.mainContainer.Add(textField);

        animationTriggerNode.RefreshExpandedState();
        animationTriggerNode.RefreshPorts();
        animationTriggerNode.SetPosition(new Rect(mousePosition, defaultNodeSize));

        return animationTriggerNode;
    }

    public SetFlagNode CreateSetFlagNode(string flagTag, string flagName, Vector2 mousePosition)
    {
        var setFlagNode = new SetFlagNode
        {
            title = "SetFlagNode",
            FlagName = flagName,
            FlagTag = flagTag,
            GUID = Guid.NewGuid().ToString()
        };

        var inputPort = GeneratePort(setFlagNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        setFlagNode.inputContainer.Add(inputPort);

        var generatedPort = GeneratePort(setFlagNode, Direction.Output);
        generatedPort.portName = "Next";
        generatedPort.name = "Next";
        setFlagNode.outputContainer.Add(generatedPort);

        setFlagNode.styleSheets.Add(Resources.Load<StyleSheet>("FlagNode"));

        var textFieldTarget = new TextField
        {
            name = "FlagName",
            value = flagName,
            label = "FlagName\n"
        };
        textFieldTarget.RegisterValueChangedCallback(evt =>
        {
            setFlagNode.FlagName = evt.newValue;
        });
        setFlagNode.mainContainer.Add(textFieldTarget);

        var textField = new TextField
        {
            name = "FlagTag",
            value = flagTag,
            label = "FlagTag\n"
        };
        textField.RegisterValueChangedCallback(evt =>
        {
            setFlagNode.FlagTag = evt.newValue;
            //dialogueNode.title = evt.newValue;
        });
        //textField.SetValueWithoutNotify(dialogueNode.title);
        setFlagNode.mainContainer.Add(textField);

        setFlagNode.RefreshExpandedState();
        setFlagNode.RefreshPorts();
        setFlagNode.SetPosition(new Rect(mousePosition, defaultNodeSize));

        return setFlagNode;
    }

    public GetFlagNode CreateGetFlagNode(string flagName, Vector2 mousePosition)
    {
        var getFlagNode = new GetFlagNode
        {
            title = "GetFlagNode",
            FlagName = flagName,
            GUID = Guid.NewGuid().ToString()
        };

        var inputPort = GeneratePort(getFlagNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        getFlagNode.inputContainer.Add(inputPort);

        var generatedPort = GeneratePort(getFlagNode, Direction.Output);
        generatedPort.portName = "Other";
        generatedPort.name = "Other";
        getFlagNode.outputContainer.Add(generatedPort);

        var button = new Button(clickEvent: () =>
        {
            AddChoicePort(getFlagNode);
        });
        button.text = "New Choice";
        getFlagNode.titleContainer.Add(button);

        getFlagNode.styleSheets.Add(Resources.Load<StyleSheet>("FlagNode"));

        var textFieldTarget = new TextField
        {
            name = "FlagName",
            value = flagName,
            label = "FlagName\n"
        };
        textFieldTarget.RegisterValueChangedCallback(evt =>
        {
            getFlagNode.FlagName = evt.newValue;
        });
        getFlagNode.mainContainer.Add(textFieldTarget);

        getFlagNode.RefreshExpandedState();
        getFlagNode.RefreshPorts();
        getFlagNode.SetPosition(new Rect(mousePosition, defaultNodeSize));

        return getFlagNode;
    }

    public BooleanSetFlagNode CreateBoolSetFlagNode(bool flagBool, string flagName, Vector2 mousePosition)
    {
        var setFlagNode = new BooleanSetFlagNode
        {
            title = "SetFlagNode",
            FlagName = flagName,
            FlagBool = flagBool,
            GUID = Guid.NewGuid().ToString()
        };

        var inputPort = GeneratePort(setFlagNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        setFlagNode.inputContainer.Add(inputPort);

        var generatedPort = GeneratePort(setFlagNode, Direction.Output);
        generatedPort.portName = "Next";
        generatedPort.name = "Next";
        setFlagNode.outputContainer.Add(generatedPort);

        setFlagNode.styleSheets.Add(Resources.Load<StyleSheet>("FlagNode"));

        var textFieldTarget = new TextField
        {
            name = "FlagName",
            value = flagName,
            label = "FlagName\n"
        };
        textFieldTarget.RegisterValueChangedCallback(evt =>
        {
            setFlagNode.FlagName = evt.newValue;
        });
        setFlagNode.mainContainer.Add(textFieldTarget);

        var boolFieldTarget = new UnityEngine.UIElements.Toggle
        {
            name = "FlagSet",
            value = flagBool,
            label = "FlagSet\n"
        };
        boolFieldTarget.RegisterValueChangedCallback(evt =>
        {
            setFlagNode.FlagBool = evt.newValue;
        });
        setFlagNode.mainContainer.Add(boolFieldTarget);

        setFlagNode.RefreshExpandedState();
        setFlagNode.RefreshPorts();
        setFlagNode.SetPosition(new Rect(mousePosition, defaultNodeSize));

        return setFlagNode;
    }

    public BooleanGetFlagNode CreateBoolGetFlagNode(string flagName, Vector2 mousePosition)
    {
        var getFlagNode = new BooleanGetFlagNode
        {
            title = "GetFlagNode",
            FlagName = flagName,
            GUID = Guid.NewGuid().ToString()
        };

        var inputPort = GeneratePort(getFlagNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        getFlagNode.inputContainer.Add(inputPort);

        var generatedPort = GeneratePort(getFlagNode, Direction.Output);
        generatedPort.portName = "Other";
        generatedPort.name = "Other";
        getFlagNode.outputContainer.Add(generatedPort);

        var truePort = GeneratePort(getFlagNode, Direction.Output);
        truePort.portName = "True";
        truePort.name = "True";
        getFlagNode.outputContainer.Add(truePort);

        var falsePort = GeneratePort(getFlagNode, Direction.Output);
        falsePort.portName = "False";
        falsePort.name = "False";
        getFlagNode.outputContainer.Add(falsePort);

        getFlagNode.styleSheets.Add(Resources.Load<StyleSheet>("FlagNode"));

        var textFieldTarget = new TextField
        {
            name = "FlagName",
            value = flagName,
            label = "FlagName\n"
        };
        textFieldTarget.RegisterValueChangedCallback(evt =>
        {
            getFlagNode.FlagName = evt.newValue;
        });
        getFlagNode.mainContainer.Add(textFieldTarget);

        getFlagNode.RefreshExpandedState();
        getFlagNode.RefreshPorts();
        getFlagNode.SetPosition(new Rect(mousePosition, defaultNodeSize));

        return getFlagNode;
    }

    public MoveToPosNode CreateMoveToPosNode(string targetObject, string referenceObject, Vector3 posOffset, bool wait, Vector2 mousePosition)
    {
        var moveToPosNode = new MoveToPosNode
        {
            title = "MoveToPosNode",
            TargetObject = targetObject,
            ReferenceObject = referenceObject,
            PosOffset = posOffset,
            Wait = wait,
            GUID = Guid.NewGuid().ToString()
        };

        var inputPort = GeneratePort(moveToPosNode, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        moveToPosNode.inputContainer.Add(inputPort);

        var generatedPort = GeneratePort(moveToPosNode, Direction.Output);
        generatedPort.portName = "Next";
        generatedPort.name = "Next";
        moveToPosNode.outputContainer.Add(generatedPort);

        moveToPosNode.styleSheets.Add(Resources.Load<StyleSheet>("FlagNode"));

        var textFieldTarget = new TextField
        {
            name = "TargetObject",
            value = targetObject,
            label = "TargetObject\n"
        };
        textFieldTarget.RegisterValueChangedCallback(evt =>
        {
            moveToPosNode.TargetObject = evt.newValue;
        });
        moveToPosNode.mainContainer.Add(textFieldTarget);

        textFieldTarget = new TextField
        {
            name = "ReferenceObject",
            value = referenceObject,
            label = "ReferenceObject\n"
        };
        textFieldTarget.RegisterValueChangedCallback(evt =>
        {
            moveToPosNode.ReferenceObject = evt.newValue;
        });
        moveToPosNode.mainContainer.Add(textFieldTarget);

        var vector3FieldTarget = new Vector3Field
        {
            name = "PositionOffset",
            value = posOffset,
            label = "PositionOffset\n"

        };
        vector3FieldTarget.RegisterValueChangedCallback(evt =>
        {
            moveToPosNode.PosOffset = evt.newValue;
        });
        moveToPosNode.mainContainer.Add(vector3FieldTarget);

        var boolFieldTarget = new UnityEngine.UIElements.Toggle
        {
            name = "Wait",
            value = wait,
            label = "Wait\n"
        };
        boolFieldTarget.RegisterValueChangedCallback(evt =>
        {
            moveToPosNode.Wait = evt.newValue;
        });
        moveToPosNode.mainContainer.Add(boolFieldTarget);

        moveToPosNode.RefreshExpandedState();
        moveToPosNode.RefreshPorts();
        moveToPosNode.SetPosition(new Rect(mousePosition, defaultNodeSize));

        return moveToPosNode;
    }

    public void AddDialogueNode(string nodeName, Vector2 mousePosition)
    {
        AddElement(CreateDialogueNode(string.Empty, string.Empty, mousePosition));
    }

    public void AddAnimationTriggerNode(string nodeName, Vector2 mousePosition)
    {
        AddElement(CreateAnimationTriggerNode(string.Empty, string.Empty, mousePosition));
    }

    public void AddSetFlagNode(string nodeName, Vector2 mousePosition)
    {
        AddElement(CreateSetFlagNode(string.Empty, string.Empty, mousePosition));
    }

    public void AddGetFlagNode(string nodeName, Vector2 mousePosition)
    {
        AddElement(CreateGetFlagNode(string.Empty, mousePosition));
    }

    public void AddBooleanSetFlagNode(string nodeName, Vector2 mousePosition)
    {
        AddElement(CreateBoolSetFlagNode(false, string.Empty, mousePosition));
    }

    public void AddBooleanGetFlagNode(string nodeName, Vector2 mousePosition)
    {
        AddElement(CreateBoolGetFlagNode(string.Empty, mousePosition));
    }

    public void AddMoveToPosNode(string nodeName, Vector2 mousePosition)
    {
        AddElement(CreateMoveToPosNode(string.Empty, string.Empty, Vector3.zero, false, mousePosition));
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        var compatiblePorts = new List<Port>();

        ports.ForEach(funcCall: (port) =>
        {
            if(startPort!=port && startPort.node != port.node)
            {
                compatiblePorts.Add(port);
            }
        });

        return compatiblePorts;
    }

    public void AddChoicePort(Node node, string overriddenPortName = "")
    {
        var generatedPort = GeneratePort(node, Direction.Output);

        var oldLabel = generatedPort.contentContainer.Q<Label>("type");
        generatedPort.contentContainer.Remove(oldLabel);

        var outputPortCount = node.outputContainer.Query(name: "connector").ToList().Count;

        var choicePortName = string.IsNullOrEmpty(overriddenPortName)
            ? $"Choice {outputPortCount}"
            : overriddenPortName;

        var textField = new TextField
        {
            name = string.Empty,
            value = choicePortName
        };
        textField.RegisterValueChangedCallback(evt => generatedPort.portName = evt.newValue);
        generatedPort.contentContainer.Add(new Label("  "));
        generatedPort.contentContainer.Add(textField);

        var deleteButton = new Button(() => RemovePort(node, generatedPort))
        {
            text = "X"
        };
        generatedPort.contentContainer.Add(deleteButton);

        generatedPort.portName = choicePortName;
        generatedPort.name = choicePortName;
        node.outputContainer.Add(generatedPort);
        node.RefreshExpandedState();
        node.RefreshPorts();
    }

    private void RemovePort(Node node, Port generatedPort)
    {
        var targetEdge = edges.ToList().Where(x =>
        x.output.portName == generatedPort.portName && x.output.node == generatedPort.node);

        if (targetEdge.Any())
        {
            var edge = targetEdge.First();
            edge.input.Disconnect(edge);
            RemoveElement(targetEdge.First());
        }

        node.outputContainer.Remove(generatedPort);
        node.RefreshPorts();
        node.RefreshExpandedState();
    }

    public void AddPropertyToBlackBoard(ExposedProperty exposedProperty)
    {
        var localPropertyName = exposedProperty.PropertyName;
        var localPropertyValue = exposedProperty.PropertyValue;
        while (ExposedProperties.Any(x => x.PropertyName == localPropertyName))
        {
            localPropertyName = $"{localPropertyName}(1)";
        }

        var property = new ExposedProperty();
        property.PropertyName = localPropertyName;
        property.PropertyValue = localPropertyValue;
        ExposedProperties.Add(property);

        var container = new VisualElement();
        var blackboardField = new BlackboardField { text = property.PropertyName, typeText = "string property" };
        container.Add(blackboardField);

        var propertyValueTextField = new TextField("Value:")
        {
            value = localPropertyValue
        };
        propertyValueTextField.RegisterValueChangedCallback(evt =>
        {
            var changingPropertyIndex = ExposedProperties.FindIndex(x => x.PropertyName == property.PropertyName);
            ExposedProperties[changingPropertyIndex].PropertyValue = evt.newValue;
        });
        var blackBoardValueRow = new BlackboardRow(blackboardField, propertyValueTextField);
        container.Add(blackBoardValueRow);

        Blackboard.Add(container);
    }

    public void ClearBlackBoardAndExposedProperties()
    {
        ExposedProperties.Clear();
        Blackboard.Clear();
    }
}
