using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FancyManScript : EnemyScript
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public override void death()
    {
        base.death();
        SayDialogue textbox = new SayDialogue();
        textbox.heightOverSpeaker = 2;
        textbox.inputText = new TextAsset("Ughh\n" +
            "Why do I have to lose...\n" +
            "To such a big nerd.");
        CutsceneController.addCutsceneEventFront(textbox, gameObject, true, OverworldController.gameModeOptions.Cutscene);
    }
}
