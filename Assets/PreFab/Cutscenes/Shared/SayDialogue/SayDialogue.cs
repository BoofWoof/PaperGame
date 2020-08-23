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

    private DialogueNodeData currentNode;
    private List<NodeLinkData> currentLinks;
    public NodeLinkData selectedLink;
    
    private GameObject spawnedTextBox;

    // Start is called before the first frame update
    void Start()
    {
    }

    override public bool Activate()
    {
        if (inputDialogue == null)
        {
            GameObject textbox = Resources.Load<GameObject>("TextBox");
            spawnedTextBox = Instantiate<GameObject>(textbox, new Vector3(parent.transform.position.x, parent.transform.position.y + heightOverSpeaker, parent.transform.position.z), Quaternion.identity);
            spawnedTextBox.GetComponent<TextBoxController>().textfile = inputText;
            OverworldController.setTrackingMultiplyer(0.7f);
        } else
        {
            currentNode = inputDialogue.DialogueNodeData.First(x => x.Guid == inputDialogue.StartingGUID);
            NodeRead();
        }
        return true;
    }

    // Update is called once per frame
    override public bool Update()
    {
        if (spawnedTextBox == null)
        {
            if (inputDialogue == null)
            {
                OverworldController.setTrackingMultiplyer(1.0f);
                return true;
            } else
            {
                currentLinks = inputDialogue.NodeLinks.Where(x => x.BaseNodeGuid == currentNode.Guid).ToList();
                if (currentLinks.Count == 0)
                {
                    OverworldController.setTrackingMultiplyer(1.0f);
                    return true;
                }
                else
                {
                    currentNode = inputDialogue.DialogueNodeData.First(x => x.Guid == selectedLink.TargetNodeGuid);
                    NodeRead();
                }
            }
        }
        return false;
    }

    private void NodeRead()
    {
        currentLinks = inputDialogue.NodeLinks.Where(x => x.BaseNodeGuid == currentNode.Guid).ToList();
        inputText = new TextAsset(currentNode.DialogueText);
        GameObject textbox = Resources.Load<GameObject>("TextBox");
        Transform target;
        float dialogueHeight = heightOverSpeaker;
        if (currentNode.TargetPlayer == string.Empty) {
            target = parent.transform;
        } else
        {
            Character findCharacter = OverworldController.findCharacterByName(currentNode.TargetPlayer, OverworldController.CharacterList);
            if (findCharacter.CharacterObject)
            {
                target = findCharacter.CharacterObject.transform;
                dialogueHeight = findCharacter.dialogueHeight;
            } else
            {
                target = parent.transform;
                inputText = new TextAsset("Error: " + currentNode.TargetPlayer + " is not found.  Maybe you made a typo?");
            }
        }
        spawnedTextBox = Instantiate<GameObject>(textbox, new Vector3(target.position.x, target.position.y + dialogueHeight, target.position.z), Quaternion.identity);
        spawnedTextBox.GetComponent<TextBoxController>().textfile = inputText;
        spawnedTextBox.GetComponent<TextBoxController>().choices = currentLinks;
        if (currentNode.TargetPlayer == string.Empty)
        {
            spawnedTextBox.GetComponent<TextBoxController>().speakerName = speakerName;
        }
        else
        {
            spawnedTextBox.GetComponent<TextBoxController>().speakerName = currentNode.TargetPlayer;
        }
        spawnedTextBox.GetComponent<TextBoxController>().scriptSource = this;
        OverworldController.setTrackingMultiplyer(0.7f);
    }
}
