using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class NodeSearchWindow : ScriptableObject, ISearchWindowProvider
{
    private DialogueGraphView _graphView;
    private EditorWindow _window;
    private Texture2D _indentationIcon;

    public void Init(EditorWindow window, DialogueGraphView graphView)
    {
        _graphView = graphView;
        _window = window;

        _indentationIcon = new Texture2D(1, 1);
        _indentationIcon.SetPixel(0, 0, new Color(0, 0, 0, 0));
        _indentationIcon.Apply();
    }

    public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
    {
        var tree = new List<SearchTreeEntry>
        {
            new SearchTreeGroupEntry(new GUIContent("Create Elements"), 0),
            new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),
            new SearchTreeEntry(new GUIContent("Dialogue Node", _indentationIcon))
            {
                userData = new DialogueNode(), level = 2
            },
            new SearchTreeGroupEntry(new GUIContent("Flag Node"), 1),
            new SearchTreeEntry(new GUIContent("Set Flag Node", _indentationIcon))
            {
                userData = new SetFlagNode(), level = 2
            },
            new SearchTreeEntry(new GUIContent("Check Flag Node", _indentationIcon))
            {
                userData = new GetFlagNode(), level = 2
            },
            new SearchTreeEntry(new GUIContent("Set Boolean Flag Node", _indentationIcon))
            {
                userData = new BooleanSetFlagNode(), level = 2
            },
            new SearchTreeEntry(new GUIContent("Check Boolean Flag Node", _indentationIcon))
            {
                userData = new BooleanGetFlagNode(), level = 2
            },
            new SearchTreeGroupEntry(new GUIContent("Animation Node"), 1),
            new SearchTreeEntry(new GUIContent("Animation Trigger", _indentationIcon))
            {
                userData = new AnimationTriggerNode(), level = 2
            },
            new SearchTreeGroupEntry(new GUIContent("Movement Node"), 1),
            new SearchTreeEntry(new GUIContent("Move To Position", _indentationIcon))
            {
                userData = new MoveToPosNode(), level = 2
            }
        };
        return tree;
    }

    public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
    {
        var worldMousePosition = _window.rootVisualElement.ChangeCoordinatesTo(_window.rootVisualElement.parent, 
            context.screenMousePosition-_window.position.position);
        var localMousePosition = _graphView.contentViewContainer.WorldToLocal(worldMousePosition);
        switch (SearchTreeEntry.userData)
        {
            case DialogueNode dialogueNode:
                _graphView.AddDialogueNode("Dialogue Node", localMousePosition);
                return true;
            case SetFlagNode setFlagNode:
                _graphView.AddSetFlagNode("Set Flag Node", localMousePosition);
                return true;
            case GetFlagNode getFlagNode:
                _graphView.AddGetFlagNode("Get Flag Node", localMousePosition);
                return true;
            case BooleanSetFlagNode booleanSetFlagNode:
                _graphView.AddBooleanSetFlagNode("Set Boolean Flag Node", localMousePosition);
                return true;
            case BooleanGetFlagNode booleanGetFlagNode:
                _graphView.AddBooleanGetFlagNode("Get Boolean Flag Node", localMousePosition);
                return true;
            case AnimationTriggerNode animationTriggerNode:
                _graphView.AddAnimationTriggerNode("Animation Trigger Node", localMousePosition);
                return true;
            case MoveToPosNode moveToPosNode:
                _graphView.AddMoveToPosNode("Move To Pos Node", localMousePosition);
                return true;
            default:
                return false;
        }
    }
}
