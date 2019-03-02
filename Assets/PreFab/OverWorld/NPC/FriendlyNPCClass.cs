using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyNPCClass : MonoBehaviour
{
    //CharactersName
    private GameObject Player;
    public string CharacterName = "NameMeYaDingus";
    public int UniqueSceneID = -1;
    public OverworldController.gameModeOptions dialogueMode = OverworldController.gameModeOptions.MobileCutscene;
    public TextAsset InputText;
    public string InputString = "You didn't give me any text ya dingus.";

    public GameObject dialogueCutscene;
    //Cutscene Events

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
        {
            Player = OverworldController.Player;
        }
        if (Input.GetButtonDown("Fire1") && (Vector3.Distance(Player.transform.position, transform.position) < 1) && (OverworldController.gameMode == OverworldController.gameModeOptions.Mobile))
        {
            Activated();
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
