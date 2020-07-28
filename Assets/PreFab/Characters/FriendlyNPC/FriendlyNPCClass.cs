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

    //Rotation Info
    private float goal = 0;
    private float rotated = 0;
    private float rotateDisplay = 0;
    private float rotSpeed;
    public float rotSpeedMagnitude = 360;


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
        //Handle Dialogue
        distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);
        if (readyForDialogue && ((OverworldController.gameMode == OverworldController.gameModeOptions.Mobile) || (OverworldController.gameMode == OverworldController.gameModeOptions.DialogueReady)))
        {
            if (Bubble == null)
            {
                Bubble = Instantiate(dialogueBubble, transform.position + new Vector3(0, 0.5f + height/2, 0), Quaternion.identity);
                Bubble.transform.SetParent(transform);
                Bubble.transform.rotation = Quaternion.identity;
            } else
            {
                Bubble.transform.rotation = Quaternion.identity;
            }
            if (Input.GetButtonDown("Fire1"))
            {
                Activated();
                if ((Player.transform.position.x > this.transform.position.x + 0.2f))
                    {
                        Player.GetComponent<CharacterMovementOverworld>().goal = 0;
                    }
                    if ((Player.transform.position.x < this.transform.position.x - 0.2f))
                    {
                        Player.GetComponent<CharacterMovementOverworld>().goal = 180;
                    }
            }
        } else
        {
            if (Bubble != null)
            {
                Destroy(Bubble);
                Bubble = null;
            }
        }

        //Face Player If Near
        if ((distanceToPlayer < 1) && ((OverworldController.gameMode == OverworldController.gameModeOptions.Mobile) || (OverworldController.gameMode == OverworldController.gameModeOptions.DialogueReady)))
        {
            //SetRotationGoals=================================================================================
            if ((Player.transform.position.x > this.transform.position.x + 0.2f))
            {
                goal = 180;
            }
            if ((Player.transform.position.x < this.transform.position.x - 0.2f))
            {
                goal = 0;
            }
        }

        //SPRITE ROTATION START-------------------------------------------------------------------------
        if ((goal > rotated))
        {
            rotSpeed = rotSpeedMagnitude * Time.deltaTime;
            rotated = rotated + rotSpeed;
        }
        if ((goal < rotated))
        {
            rotSpeed = -rotSpeedMagnitude * Time.deltaTime;
            rotated = rotated + rotSpeed;
        }
        rotateDisplay = rotated;
        if ((rotated > 90))
        {
            rotateDisplay += 180;
        }
        transform.rotation = Quaternion.Euler(0, rotateDisplay, 0);
        Vector3 currentScale = transform.localScale;
        if (rotated < 90)
        {
            this.transform.localScale = new Vector3(Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
        if (rotated > 90)
        {
            this.transform.localScale = new Vector3(-Mathf.Abs(currentScale.x), currentScale.y, currentScale.z);
        }
        //SPRITE ROTATION END------------------------------------------------------------------

        //transform.rotation = Quaternion.identity;
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
