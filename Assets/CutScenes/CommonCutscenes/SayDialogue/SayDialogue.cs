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

    public AudioClip letter_noise;

    // Start is called before the first frame update
    void Start()
    {
    }

    override public bool Activate()
    {
        if (deconstructerSource == null)
        {
            GameObject textbox = Resources.Load<GameObject>("TextBox");
            spawnedTextBox = Instantiate<GameObject>(textbox, new Vector3(parent.transform.position.x, parent.transform.position.y + heightOverSpeaker, parent.transform.position.z + 0.2f), Quaternion.identity);
            TextBoxController tb_controller = spawnedTextBox.GetComponent<TextBoxController>();
            tb_controller.textfile = inputText;
            tb_controller.letter_noise = letter_noise;
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
        
        Character findCharacter = GameDataTracker.findCharacterByName(speakerName, GameDataTracker.CharacterList);
        FriendlyNPCClass friendlyNPC = findCharacter.CharacterObject.GetComponent<FriendlyNPCClass>();
        if (friendlyNPC != null) letter_noise = friendlyNPC.letter_noise;
        else letter_noise = null;
        Transform target;
        float dialogueHeight;

        target = findCharacter.CharacterObject.transform;
        dialogueHeight = findCharacter.dialogueHeight;

        spawnedTextBox = Instantiate<GameObject>(textbox, new Vector3(target.position.x, target.position.y + dialogueHeight, target.position.z + 0.2f), Quaternion.identity);
        spawnedTextBox.GetComponent<TextBoxController>().textfile = inputText;
        spawnedTextBox.GetComponent<TextBoxController>().choices = currentLinks;
        spawnedTextBox.GetComponent<TextBoxController>().speakerName = speakerName;
        spawnedTextBox.GetComponent<TextBoxController>().scriptSource = deconstructerSource;
        spawnedTextBox.GetComponent<TextBoxController>().letter_noise = letter_noise;
    }

}
