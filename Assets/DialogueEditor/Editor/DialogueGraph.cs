using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using UnityEditor.Experimental.GraphView;
using System.Linq;

public class DialogueGraph : EditorWindow
{
    private DialogueGraphView _graphView;
    private string _filename = "New Narrative";

    [MenuItem("Graph/Dialogue Graph")]
    public static void OpenDialogueGraphWindow()
    {
        var window = GetWindow<DialogueGraph>();
        window.titleContent = new GUIContent("Dialogue Graph");
    }

    private void ConstructGraphView()
    {
        _graphView = new DialogueGraphView(this)
        {
            name = "Dialogue Graph"
        };
        _graphView.StretchToParentSize();
        rootVisualElement.Add(_graphView);
    }

    private void GenerateToolbar()
    {
        var toolbar = new Toolbar();

        var fileNameTextField = new TextField(label: "FileName");
        fileNameTextField.SetValueWithoutNotify(_filename);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt => _filename = evt.newValue);
        toolbar.Add(fileNameTextField);

        toolbar.Add(new Button(clickEvent: () => RequestDataOperation(true)) { text = "Save Data" });
        toolbar.Add(new Button(clickEvent: () => RequestDataOperation(false)) { text = "Load Data" });

        //var nodeCreateButton = new Button(clickEvent: () =>
        //    {
        //        _graphView.CreateNode("Dialogue Node");
        //    }
        //);
        //nodeCreateButton.text = "Create Node";
        //toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);
    }

    private void RequestDataOperation(bool save)
    {
        if (string.IsNullOrEmpty(_filename))
        {
            EditorUtility.DisplayDialog("Invalid file name!", "Please enter a valid file name.", "OK");
        }

        var saveUtility = GraphSaveUtility.GetInstance(_graphView);
        if (save)
        {
            saveUtility.SaveGraph(_filename);
        } else
        {
            saveUtility.LoadGraph(_filename);
        }
    }

    private void GenerateMiniMap()
    {
        var miniMap = new MiniMap { anchored = true };
        var cords = _graphView.contentViewContainer.WorldToLocal(new Vector2(this.maxSize.x-10, 30));
        miniMap.SetPosition(new Rect(cords.x, cords.y, 200, 140));
        _graphView.Add(miniMap);
    }

    private void OnEnable()
    {
        ConstructGraphView();
        GenerateToolbar();
        GenerateMiniMap();
        //GenerateBlackBoard();
    }

    private void GenerateBlackBoard()
    {
        var blackBoard = new Blackboard(_graphView);
        blackBoard.Add(new BlackboardSection { title = "Exposed Properties" });
        blackBoard.addItemRequested = _blackboard =>
        {
            _graphView.AddPropertyToBlackBoard(new ExposedProperty());
        };
        blackBoard.editTextRequested = (blackboard1, element, newValue) =>
        {
            var oldPropertyName = ((BlackboardField)element).text;
            if(_graphView.ExposedProperties.Any(x => x.PropertyName == newValue))
            {
                EditorUtility.DisplayDialog("Error", "This property name already exists, please chose another one!", "OK");
                return;
            }

            var propertyIndex = _graphView.ExposedProperties.FindIndex(x => x.PropertyName == oldPropertyName);
            _graphView.ExposedProperties[propertyIndex].PropertyName = newValue;
            ((BlackboardField)element).text = newValue;
        };
        blackBoard.SetPosition(new Rect(10, 30, 200, 300));
        _graphView.Add(blackBoard);
        _graphView.Blackboard = blackBoard;
    }

    private void OnDisable()
    {
        rootVisualElement.Remove(_graphView);
    }
}
