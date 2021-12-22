using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelEditorScript : MonoBehaviour
{
    public GridCrafter SourceScript;
    public GameObject SaveTextField;
    private GameControls control;

    private void Start()
    {
        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        GridCrafter.MenuOpen = true;
        control = new GameControls();
        control.MapCraftControls.Enable();
    }

    private void OnDisable()
    {
        control.MapCraftControls.Disable();
    }

    public void Update()
    {
        if (control.MapCraftControls.EditMenu.triggered) Close();
    }

    public void Close()
    {
        Destroy(gameObject);
        GridCrafter.MenuOpen = false;
    }

    public void EditHeight()
    {
        GridCrafter.editMode = "Height";
        GridCrafter.selectionRange = new Vector2Int(1, 1);
    }

    public void SetTileType(int tileTypeInput)
    {
        GridCrafter.editMode = "Tile";
        GridCrafter.tileType = tileTypeInput;
        GridCrafter.selectionRange = new Vector2Int(1, 1);
    }

    public void SetCharacterIndex(int characterTypeInput)
    {
        GridCrafter.editMode = "Character";
        GridCrafter.selectedCharacter = characterTypeInput;
        GridCrafter.selectionRange = CombatMapper.characterMap[GridCrafter.selectedCharacter].GetComponent<FighterClass>().TileSize;
    }

    public void SetObjectIndex(int objectTypeInput)
    {
        GridCrafter.editMode = "Object";
        GridCrafter.selectedObject = objectTypeInput;
        GridCrafter.selectionRange = new Vector2Int(1, 1);
    }

    public void SubLeft()
    {
        SourceScript.SubtractMap(1, 0, 0, 0);
    }

    public void SubRight()
    {
        SourceScript.SubtractMap(0, 1, 0, 0);
    }

    public void SubBottom()
    {
        SourceScript.SubtractMap(0, 0, 1, 0);
    }

    public void SubTop()
    {
        SourceScript.SubtractMap(0, 0, 0, 1);
    }

    public void AddLeft()
    {
        SourceScript.AddMap(1, 0, 0, 0);
    }

    public void AddRight()
    {
        SourceScript.AddMap(0, 1, 0, 0);
    }

    public void AddBottom()
    {
        SourceScript.AddMap(0, 0, 1, 0);
    }

    public void AddTop()
    {
        SourceScript.AddMap(0, 0, 0, 1);
    }

#if UNITY_EDITOR
    public void Save()
    {
        string filename = SaveTextField.GetComponent<InputField>().text;
        SourceScript.Save(filename);
    }
#endif
    
    public void LoadFromName()
    {
        string filename = SaveTextField.GetComponent<InputField>().text;
        SourceScript.LoadFromName(filename);
    }

    public void TurnTieToggle()
    {
        GridManager.turnTie = !GridManager.turnTie;
        Debug.Log(GridManager.turnTie);
    }

    public void PuzzleModeToggle()
    {
        GridManager.puzzleMode = !GridManager.puzzleMode;
        Debug.Log(GridManager.puzzleMode);
    }

    public void DoublePuzzleModeToggle()
    {
        GridManager.doublePuzzleMode = !GridManager.doublePuzzleMode;
        Debug.Log(GridManager.doublePuzzleMode);
    }
}
