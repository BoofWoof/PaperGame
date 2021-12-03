using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileViewMenuScript : ViewMenuBaseScript
{
    public override void Start()
    {
        base.Start();
        Name.text = SelectedCharacter.ObjectInfo.ObjectName;
        Notes.SetText("Note: " + SelectedCharacter.ObjectInfo.ObjectNote);
        SourceGrid = GridCrafter.blockGrid;

        UpdateCutsceneList();
    }
}
