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

    public AudioClip[] letter_noises;
    public int[] letters_per_noise_list;

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
            tb_controller.letter_noises = letter_noises;
            tb_controller.letters_per_noise_list = letters_per_noise_list;
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
        
        Character findCharacter = GameDataTracker.findCharacterByName(speakerName, GameDataTracker.CharacterList);
        FriendlyNPCClass friendlyNPC = findCharacter.CharacterObject.GetComponent<FriendlyNPCClass>();
        if (friendlyNPC != null) {
            letter_noises = friendlyNPC.ObjectInfo.LetterNoises;
            letters_per_noise_list = friendlyNPC.ObjectInfo.LetterNoisesPerList;
        }
        else {
            letter_noises = null;
            letters_per_noise_list = null;
        }
        Transform target;
        float dialogueHeight;

        target = findCharacter.CharacterObject.transform;
        dialogueHeight = findCharacter.ObjectInfo.GetDialogueHeightOverSpeaker();

        spawnedTextBox = Instantiate<GameObject>(textbox, new Vector3(target.position.x, target.position.y + dialogueHeight, target.position.z + 0.2f), Quaternion.identity);
        TextBoxController tbController = spawnedTextBox.GetComponent<TextBoxController>();
        tbController.textfile = inputText;
        tbController.choices = currentLinks;
        tbController.speakerName = speakerName;
        tbController.scriptSource = deconstructerSource;
        tbController.letter_noises = letter_noises;
        tbController.letters_per_noise_list = letters_per_noise_list;
    }

}
