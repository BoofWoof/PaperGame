using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyNPCClass : MonoBehaviour
{
    //CharactersName
    private GameObject Player;
    private GameObject Bubble;
    private float height;

    public string CharacterName = "NameMeYaDingus";
    public int UniqueSceneID = -1;
    public OverworldController.gameModeOptions dialogueMode = OverworldController.gameModeOptions.MobileCutscene;
    public TextAsset InputText;
    public string InputString = "You didn't give me any text ya dingus.";

    public GameObject dialogueCutscene;
    public GameObject dialogueBubble;
    //Cutscene Events
    public float distanceToPlayer;
    public bool readyForDialogue;

    void Start()
    {
        if(UniqueSceneID == -1)
        {
            print("Give " + CharacterName + " an ID ya nerd.");
        }
        Character thisNPCCharacter = new Character();
        thisNPCCharacter.CharacterObject = gameObject;
        thisNPCCharacter.CharacterName = CharacterName;
        thisNPCCharacter.uniqueSceneID = UniqueSceneID;
        OverworldController.CharacterList.Add(thisNPCCharacter);
        height = transform.GetComponent<BoxCollider>().size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
        {
            Player = OverworldController.Player;
        }
        distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);
        if (readyForDialogue && ((OverworldController.gameMode == OverworldController.gameModeOptions.Mobile) || (OverworldController.gameMode == OverworldController.gameModeOptions.DialogueReady)))
        {
            if (Bubble == null)
            {
                Bubble = Instantiate(dialogueBubble, transform.position + new Vector3(0, 0.5f + height/2, 0), Quaternion.identity);
                Bubble.transform.SetParent(transform);
            }
            if (Input.GetButtonDown("Fire1"))
            {
                Activated();
            }
        } else
        {
            if (Bubble != null)
            {
                Destroy(Bubble);
                Bubble = null;
            }
        }
    }

    protected virtual void Activated()
    {
        GameObject Dialogue = Instantiate<GameObject>(dialogueCutscene, Vector3.zero, Quaternion.identity);
        if (InputText != null)
        {
            Dialogue.GetComponent<SayDialogue>().inputText = InputText;
        }
        else
        {
            Dialogue.GetComponent<SayDialogue>().inputText = new TextAsset(InputString);
        }
        OverworldController.addCutsceneEvent(Dialogue,gameObject,true, dialogueMode);
    }
}
