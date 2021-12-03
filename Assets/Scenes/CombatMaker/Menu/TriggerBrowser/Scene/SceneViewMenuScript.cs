using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneViewMenuScript : ViewMenuBaseScript
{
    public override void Start()
    {
        base.Start();
        Name.text = "Scene";
        Notes.SetText("Note: ");
        SourceGrid = null;

        UpdateCutsceneList();
    }
}
