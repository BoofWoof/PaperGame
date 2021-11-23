using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSelectionScript: MenuBaseScript
{
    public Button Character, CharacterView;
    public Button Object, ObjectView;
    public Button Tile, TileView;

    public Vector2Int targetBlock = new Vector2Int(0, 0);

    public GameObject CharacterCutscene;
    public GameObject CharacterViewer;
    public GameObject ObjectCutscene;
    public GameObject ObjectViewer;
    public GameObject TileCutscene;
    public GameObject TileViewer;
    // Start is called before the first frame update

    public override void Start()
    {
        base.Start();
        GridCrafter.MenuOpen = true;
        if (GridCrafter.characterGrid[targetBlock.x, targetBlock.y] == null)
        {
            Character.interactable = false;
            CharacterView.interactable = false;
        }
        else
        {
            Character.interactable = true;
            CharacterView.interactable = true;
        }
        if (GridCrafter.objectGrid[targetBlock.x, targetBlock.y] == null)
        {
            Object.interactable = false;
            ObjectView.interactable = false;
        }
        else
        {
            Object.interactable = true;
            ObjectView.interactable = true;
        }
        if (GridCrafter.blockGrid[targetBlock.x, targetBlock.y] == null)
        {
            Tile.interactable = false;
            TileView.interactable = false;
        }
        else
        {
            Tile.interactable = true;
            TileView.interactable = true;
        }
    }

    public override void Close()
    {
        GridCrafter.MenuOpen = false;
        base.Close();
    }

    public void OpenCharacterCutscene()
    {
        GameObject levelEditorMenu = Instantiate(CharacterCutscene);
        levelEditorMenu.GetComponent<CharacterCutscenesScript>().SourceMenu = gameObject;
        levelEditorMenu.GetComponent<CharacterCutscenesScript>().TargetCharacters = new List<GridObject> {
            GridCrafter.characterGrid[targetBlock.x, targetBlock.y].GetComponent<GridObject>()};
        levelEditorMenu.transform.SetParent(transform.parent);
        gameObject.SetActive(false);
    }

    public void ViewCharacter()
    {
        GameObject characterViewMenu = Instantiate(CharacterViewer);
        characterViewMenu.GetComponent<CharacterViewMenuScript>().SourceMenu = gameObject;
        characterViewMenu.GetComponent<CharacterViewMenuScript>().SelectedCharacter = GridCrafter.characterGrid[targetBlock.x, targetBlock.y].GetComponent<FighterClass>();
        gameObject.SetActive(false);
    }
}
