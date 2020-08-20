using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SayDialogue : CutSceneClass
{
    public DialogueContainer inputDialogue;
    public TextAsset inputText;
    public float heightOverSpeaker;

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
            currentNode = inputDialogue.DialogueNodeData[0];
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
        spawnedTextBox = Instantiate<GameObject>(textbox, new Vector3(parent.transform.position.x, parent.transform.position.y + heightOverSpeaker, parent.transform.position.z), Quaternion.identity);
        spawnedTextBox.GetComponent<TextBoxController>().textfile = inputText;
        spawnedTextBox.GetComponent<TextBoxController>().choices = currentLinks;
        spawnedTextBox.GetComponent<TextBoxController>().scriptSource = this;
        OverworldController.setTrackingMultiplyer(0.7f);
    }
}
