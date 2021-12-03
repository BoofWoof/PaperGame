using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterViewMenuScript : ViewMenuBaseScript
{
    //Text
    public TextMeshProUGUI Health;
    public TextMeshProUGUI Power;
    public TextMeshProUGUI Defense;

    public override void Start()
    {
        base.Start();
        Name.text = SelectedCharacter.ObjectInfo.ObjectName;
        Health.SetText($"Health: {((FighterClass)SelectedCharacter).HPMax}");
        Power.SetText($"Power: {((FighterClass)SelectedCharacter).Power}");
        Defense.SetText($"Defense: {((FighterClass)SelectedCharacter).Defense}");
        Notes.SetText("Note: " + SelectedCharacter.ObjectInfo.ObjectNote);

        SourceGrid = GridCrafter.characterGrid;

        UpdateCutsceneList();
    }
}
