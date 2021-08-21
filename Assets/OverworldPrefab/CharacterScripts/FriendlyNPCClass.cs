using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BoxCollider))]
public class FriendlyNPCClass : MonoBehaviour
{
    GameControls controls;

    //Character Information
    [Header("Character Info")]
    public string CharacterName = "NameMeYaDingus";
    public bool OverrideTurning = false;
    public bool OverrideTalkRange = false;
    private float height;

    //Dialogue Info
    [Header("Dialogue Settings")]
    private GameObject Bubble;
    public float heightOverSpeaker = 0.8f;
    public DialogueContainer dialogue;
    public AudioClip[] letter_noises;
    public int[] letters_per_noise_list;

    public string InputString = "You didn't give me any text ya dingus.";
    public GameObject dialogueBubble;

    //Cutscene Events Info
    public float distanceToPlayer;
    public bool readyForDialogue;

    //Rotation Info
    private float goal = 0;
    private float rotated = 0;
    private float rotateDisplay = 0;
    private float rotSpeed;
    public float rotSpeedMagnitude = 360;

    private Character thisNPCCharacter;

    private void Awake()
    {
        controls = new GameControls();
    }

    private void OnEnable()
    {
        controls.OverworldControls.Enable();
    }

    private void OnDisable()
    {
        controls.OverworldControls.Disable();
    }

    void Start()
    {
        height = transform.GetComponent<BoxCollider>().size.y;

        if (CharacterName == "NameMeYaDingus")
        {
            print($"Name me! My ID is {this.GetInstanceID()}");
        }
        thisNPCCharacter = new Character();
        thisNPCCharacter.CharacterObject = gameObject;
        thisNPCCharacter.CharacterName = CharacterName;
        thisNPCCharacter.dialogueHeight = height + heightOverSpeaker;
        thisNPCCharacter.uniqueSceneID = GetInstanceID();
        GameDataTracker.CharacterList.Add(thisNPCCharacter);

        NavMeshAgent agent = GetComponent<NavMeshAgent>();
        agent.autoTraverseOffMeshLink = false;
        agent.updateRotation = false;
    }

    public void DestroySelf()
    {
        GameDataTracker.CharacterList.Remove(thisNPCCharacter);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!OverrideTalkRange)
        {
            GameObject Player = OverworldController.Player;
            distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);
            DialogueCheck(distanceToPlayer, Player);
            FacePlayer(distanceToPlayer, Player);
        } else
        {
            distanceToPlayer = 1000;
        }
    }

    private void FacePlayer(float distanceToPlayer, GameObject Player)
    {
        if (!OverrideTurning)
        {
            //Face Player If Near
            if ((distanceToPlayer < 1) && ((GameDataTracker.gameMode == GameDataTracker.gameModeOptions.Mobile) || (GameDataTracker.gameMode == GameDataTracker.gameModeOptions.DialogueReady)))
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
        }
    }

    private void DialogueCheck(float distanceToPlayer, GameObject Player)
    {
        if (!OverrideTalkRange)
        {
            if (readyForDialogue && ((GameDataTracker.gameMode == GameDataTracker.gameModeOptions.Mobile) || (GameDataTracker.gameMode == GameDataTracker.gameModeOptions.DialogueReady)))
            {
                if (Bubble == null)
                {
                    Bubble = Instantiate(dialogueBubble, transform.position + new Vector3(0, 0.5f + height / 2, 0), Quaternion.identity);
                    Bubble.transform.SetParent(transform);
                    Bubble.transform.rotation = Quaternion.identity;
                }
                else
                {
                    Bubble.transform.rotation = Quaternion.identity;
                }
                if (controls.OverworldControls.MainAction.triggered)
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
            }
            else
            {
                if (Bubble != null)
                {
                    Destroy(Bubble);
                    Bubble = null;
                }
            }
        }
    }

    protected virtual void Activated()
    {
        //GameObject Dialogue = Instantiate<GameObject>(dialogueCutscene, Vector3.zero, Quaternion.identity);
        if (dialogue != null)
        {
            CutsceneDeconstruct complexCutscene = ScriptableObject.CreateInstance<CutsceneDeconstruct>();
            complexCutscene.Deconstruct(dialogue, CharacterName, gameObject);
        }
        else
        {
            SayDialogue dialogueCutscene = ScriptableObject.CreateInstance<SayDialogue>();
            dialogueCutscene.heightOverSpeaker = heightOverSpeaker;
            dialogueCutscene.speakerName = CharacterName;
            dialogueCutscene.inputText = new TextAsset(InputString);
            dialogueCutscene.letter_noises = letter_noises;
            dialogueCutscene.letters_per_noise_list = letters_per_noise_list;
            CutsceneController.addCutsceneEvent(dialogueCutscene, gameObject, true, GameDataTracker.gameModeOptions.Cutscene);
        }
    }
}
