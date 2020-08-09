using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SayDialogue : CutSceneClass
{
    public TextAsset inputText;
    public float heightOverSpeaker;
    
    private GameObject spawnedTextBox;

    // Start is called before the first frame update
    void Start()
    {
    }

    override public bool Activate()
    {
        GameObject textbox = Resources.Load<GameObject>("TextBox");
        spawnedTextBox = Instantiate<GameObject>(textbox, new Vector3(parent.transform.position.x, parent.transform.position.y + heightOverSpeaker, parent.transform.position.z), Quaternion.identity);
        spawnedTextBox.GetComponent<TextBoxController>().textfile = inputText;
        OverworldController.setTrackingMultiplyer(0.7f);
        return true;
    }

    // Update is called once per frame
    override public bool Update()
    {
        if (spawnedTextBox == null)
        {
            OverworldController.setTrackingMultiplyer(1.0f);
            return true;
        }
        return false;
    }
}
