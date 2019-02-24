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
        GameObject textbox = Instantiate<GameObject>(dialogueEventInput, Vector3.zero, Quaternion.identity);
        textbox.SetActive(false);
        textbox.GetComponent<SayDialogue>().heightOverSpeaker = 2;
        textbox.GetComponent<SayDialogue>().inputText = new TextAsset("Ughh\n" +
            "Why do I have to lose...\n" +
            "To such a big nerd.");
        sceneLists.addCutseenEventFRONT(textbox, gameObject, true);
    }
}
