using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCutscenesScript : MonoBehaviour
{
    public GameObject SourceObject;

    public GameObject AddMoreTargetsMenu;
    public GameObject AddLowHealthTriggerMenu;

    public List<GridObject> TargetCharacters;

    private void Start()
    {
        transform.SetParent(SourceObject.transform.parent);
        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    public void Close()
    {
        SourceObject.SetActive(true);
        Destroy(gameObject);
    }

    public void AddMoreCharacters()
    {
        GameObject AddCharactersMenu = Instantiate(AddMoreTargetsMenu);
        AddCharactersMenu.GetComponent<AddMoreTargetsScript>().SourceMenu = gameObject;
        AddCharactersMenu.GetComponent<AddMoreTargetsScript>().Targets = TargetCharacters;
        AddCharactersMenu.GetComponent<AddMoreTargetsScript>().SourceGrid = GridCrafter.characterGrid;

        gameObject.SetActive(false);
    }

    public void AddLowHealthTrigger()
    {
        GameObject AddCharactersMenu = Instantiate(AddLowHealthTriggerMenu);
        AddCharactersMenu.GetComponent<LowHealthTriggerScript>().SourceMenu = gameObject;
        AddCharactersMenu.GetComponent<LowHealthTriggerScript>().TargetCharacters = TargetCharacters;

        gameObject.SetActive(false);
    }
}
