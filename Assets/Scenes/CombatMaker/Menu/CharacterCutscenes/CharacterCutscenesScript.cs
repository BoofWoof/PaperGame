using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterCutscenesScript : MonoBehaviour
{
    public GameObject SourceMenu;

    public GameObject AddMoreTargetsMenu;
    public GameObject AddLowHealthTriggerMenu;

    public List<GridObject> TargetCharacters;
    public Slider TriggerLimitSlider;
    public TextMeshProUGUI TriggerLimitText;

    private void Start()
    {
        transform.SetParent(SourceMenu.transform.parent);
        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    private void Update()
    {
        if(TriggerLimitSlider.value == 0)
        {
            TriggerLimitText.SetText("Trigger Limit: Inf");
        } else
        {
            TriggerLimitText.SetText($"Trigger Limit: {TriggerLimitSlider.value}");
        }
    }

    public void Close()
    {
        SourceMenu.SetActive(true);
        Destroy(gameObject);
    }

    public void CloseChain()
    {
        SourceMenu.SetActive(true);
        SourceMenu.GetComponent<ObjectSelectionScript>().Close();
        Close();
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
        AddCharactersMenu.GetComponent<LowHealthTriggerScript>().TriggerLimit = (int)TriggerLimitSlider.value;

        gameObject.SetActive(false);
    }
}
