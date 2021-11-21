using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSelectionScript: MonoBehaviour
{
    public Button Character, CharacterView;
    public Button Object, ObjectView;
    public Button Tile, TileView;

    public Vector2Int targetBlock = new Vector2Int(0, 0);

    public GameObject CharacterCutscene;
    public GameObject ObjectCutscene;
    public GameObject TileCutscene;
    // Start is called before the first frame update

    private void Start()
    {
        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
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

    public void Close()
    {
        GridCrafter.MenuOpen = false;
        Destroy(gameObject);
    }

    public void OpenCharacterCutscene()
    {
        GameObject levelEditorMenu = Instantiate(CharacterCutscene);
        levelEditorMenu.GetComponent<CharacterCutscenesScript>().SourceObject = gameObject;
        levelEditorMenu.transform.SetParent(transform.parent);
        gameObject.SetActive(false);
    }
}
