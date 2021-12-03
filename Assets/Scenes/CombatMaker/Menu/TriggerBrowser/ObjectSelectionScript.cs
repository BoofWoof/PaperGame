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

    public GameObject CharacterTriggerMenu;
    public GameObject CharacterViewer;
    public GameObject ObjectTriggerMenu;
    public GameObject ObjectViewer;
    public GameObject TileTriggerMenu;
    public GameObject TileViewer;
    public GameObject SceneTriggerMenu;
    public GameObject SceneViewer;
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

    public void OpenTriggerBrowser(GameObject Menu, GameObject[,] GridSource)
    {
        GameObject levelEditorMenu = Instantiate(Menu);
        levelEditorMenu.GetComponent<TriggerBrowserBaseScript>().SourceMenu = gameObject;
        levelEditorMenu.GetComponent<TriggerBrowserBaseScript>().TargetCharacters = new List<GridObject> {
            GridSource[targetBlock.x, targetBlock.y].GetComponent<GridObject>()};
        levelEditorMenu.transform.SetParent(transform.parent);
        gameObject.SetActive(false);

    }

    public void OpenCharacterTriggerBrowser()
    {
        OpenTriggerBrowser(CharacterTriggerMenu, GridCrafter.characterGrid);
    }

    public void OpenObjectTriggerBrowser()
    {
        OpenTriggerBrowser(ObjectTriggerMenu, GridCrafter.objectGrid);
    }

    public void OpenTileTriggerBrowser()
    {
        OpenTriggerBrowser(TileTriggerMenu, GridCrafter.blockGrid);
    }

    public void OpenSceneTriggerBrowser()
    {
        OpenTriggerBrowser(SceneTriggerMenu, GridCrafter.blockGrid);
    }

    public void OpenTriggerViewer(GameObject Menu, GameObject[,] GridSource)
    {
        GameObject viewMenu = Instantiate(Menu);
        viewMenu.GetComponent<ViewMenuBaseScript>().SourceMenu = gameObject;
        viewMenu.GetComponent<ViewMenuBaseScript>().SelectedCharacter = GridSource[targetBlock.x, targetBlock.y].GetComponent<GridObject>();
        gameObject.SetActive(false);
    }

    public void ViewCharacter()
    {
        OpenTriggerViewer(CharacterViewer, GridCrafter.characterGrid);
    }

    public void ViewObject()
    {
        OpenTriggerViewer(ObjectViewer, GridCrafter.objectGrid);
    }

    public void ViewTile()
    {
        OpenTriggerViewer(TileViewer, GridCrafter.blockGrid);
    }

    public void ViewScene()
    {
        OpenTriggerViewer(SceneViewer, GridCrafter.blockGrid);
    }
}
