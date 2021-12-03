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
    public ObjectInfoScript ObjectInfo;
    private float height;

    //Dialogue Info
    [Header("Dialogue Settings")]
    private GameObject Bubble;
    public DialogueContainer dialogue;
    
    public GameObject dialogueBubble;

    //Cutscene Events Info
    public float distanceToPlayer;
    public bool readyForDialogue;

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

        if (ObjectInfo.ObjectName == "NameMeYaDingus")
        {
            print($"Name me! My ID is {this.GetInstanceID()}");
        }
        thisNPCCharacter = new Character();
        thisNPCCharacter.CharacterObject = gameObject;
        thisNPCCharacter.ObjectInfo = ObjectInfo;
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
        if(dialogue != null)
        {
            GameObject Player = OverworldController.Player;
            DialogueCheck(distanceToPlayer, Player);
            distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);
        } else
        {
            distanceToPlayer = 1000f;
        }
    }

    private void DialogueCheck(float distanceToPlayer, GameObject Player)
    {
        if (dialogue != null)
        {
            if (readyForDialogue && ((GameDataTracker.cutsceneMode == GameDataTracker.cutsceneModeOptions.Mobile) || GameDataTracker.dialogueReady))
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
                        Player.GetComponent<CharacterMovementOverworld>().GetComponent<SpriteFlipper>().setFacingLeft();
                    }
                    if ((Player.transform.position.x < this.transform.position.x - 0.2f))
                    {
                        Player.GetComponent<CharacterMovementOverworld>().GetComponent<SpriteFlipper>().setFacingRight();
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
            complexCutscene.Deconstruct(dialogue, ObjectInfo.ObjectName, gameObject);
        }
    }
}
