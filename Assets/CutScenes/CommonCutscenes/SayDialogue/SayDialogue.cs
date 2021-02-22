using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SayDialogue : CutSceneClass
{
    public DialogueContainer inputDialogue;
    public TextAsset inputText;
    public float heightOverSpeaker;
    public string speakerName;

    public NodeLinkData selectedLink;
    
    private GameObject spawnedTextBox;

    public CutsceneDeconstruct deconstructerSource;
    public List<NodeLinkData> currentLinks;

    // Start is called before the first frame update
    void Start()
    {
    }

    override public bool Activate()
    {
        if (deconstructerSource == null)
        {
            GameObject textbox = Resources.Load<GameObject>("TextBox");
            spawnedTextBox = Instantiate<GameObject>(textbox, new Vector3(parent.transform.position.x, parent.transform.position.y + heightOverSpeaker, parent.transform.position.z), Quaternion.identity);
            spawnedTextBox.GetComponent<TextBoxController>().textfile = inputText;
        } else
        {
            DialogueNodeRead();
        }
        return true;
    }

    // Update is called once per frame
    override public bool Update()
    {
        if (spawnedTextBox == null)
        {
            if (deconstructerSource == null)
            {
                return true;
            } else
            {
                return true;
            }
        }
        return false;
    }

    private void DialogueNodeRead()
    {
        GameObject textbox = Resources.Load<GameObject>("TextBox");

        Debug.Log(speakerName);
        
        Character findCharacter = OverworldController.findCharacterByName(speakerName, OverworldController.CharacterList);
        Transform target;
        float dialogueHeight;

        target = findCharacter.CharacterObject.transform;
        dialogueHeight = findCharacter.dialogueHeight;

        spawnedTextBox = Instantiate<GameObject>(textbox, new Vector3(target.position.x, target.position.y + dialogueHeight, target.position.z), Quaternion.identity);
        spawnedTextBox.GetComponent<TextBoxController>().textfile = inputText;
        spawnedTextBox.GetComponent<TextBoxController>().choices = currentLinks;
        spawnedTextBox.GetComponent<TextBoxController>().speakerName = speakerName;
        spawnedTextBox.GetComponent<TextBoxController>().scriptSource = deconstructerSource;
    }

}
