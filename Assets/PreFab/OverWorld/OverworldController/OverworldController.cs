﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character
{
    public GameObject CharacterObject;
    public string CharacterName;
    public int uniqueSceneID;
}
[System.Serializable]
public class CutSceneEvent
{
    public GameObject CutsceneEvent;
    public GameObject CutsceneTarget;
    public bool Wait;
    public GameObject CameraFocus;
    public Vector3 CameraOffset;
    public OverworldController.gameModeOptions GameMode;
}

public class OverworldController : MonoBehaviour
{
    //List of all nonplayer characters as well as a player specific lookup.
    public static List<Character> CharacterList;
    public static List<Character> EnemyList;
    public static GameObject Player;
    //-----------------------------------------------------

    //Spawned Objects
    public GameObject playerInput;  //Player Object
    public GameObject spawnPoint;  //Where to spawn if not transitioning between areas.
    public GameObject trackingCameraInput;  //What track camera to use.
    public static GameObject trackingCamera;  //Publically accessible camera.
    public GameObject[] SceneTransfers;  //Triggers that will cause a scene transfer.

    //CutScene-------------------------------------------------
    private static List<CutSceneEvent> CutsceneQueue = new List<CutSceneEvent>();
    public static int CutscenesPlaying = 0;
    //-----------------------------------------------------------

    //GameplayMode---------------------------------------------------
    public enum gameModeOptions {Mobile, Cutscene, MobileCutscene, DialogueReady, Paused};
    public static gameModeOptions gameMode = gameModeOptions.Mobile;
    //-----------------------------------------------------------------

    public void Awake()
    {
        CharacterList = new List<Character>();
        EnemyList = new List<Character>();
        CutscenesPlaying = 0;
        if (GameDataTracker.lastAreaWasCombat == false)
        {
            if (GameDataTracker.previousArea != null)
            {
                foreach (GameObject sceneTransfer in SceneTransfers)
                {
                    //SPAWNS PLAYER AT THE DESIGNATED AREA ENTRANCE
                    if (sceneTransfer.GetComponent<SceneMover>().sceneName == GameDataTracker.previousArea)
                    {
                        Player = Instantiate(playerInput, sceneTransfer.transform.position, Quaternion.identity);

                        GameObject moveEvent = new GameObject();
                        PlayerTravelDirection pm = moveEvent.AddComponent<PlayerTravelDirection>();
                        SceneMover.exitDirectionOptions entranceDirection = sceneTransfer.GetComponent<SceneMover>().exitDirection;
                        if (entranceDirection == SceneMover.exitDirectionOptions.up)
                        {
                            pm.endPosition = Player.transform.position + new Vector3(0, 0, -2);
                            pm.travelDirection = SceneMover.exitDirectionOptions.down;
                        }
                        else if (entranceDirection == SceneMover.exitDirectionOptions.left)
                        {
                            pm.endPosition = Player.transform.position + new Vector3(-2, 0, 0);
                            pm.travelDirection = SceneMover.exitDirectionOptions.right;
                        }
                        else if (entranceDirection == SceneMover.exitDirectionOptions.right)
                        {
                            pm.endPosition = Player.transform.position + new Vector3(2, 0, 0);
                            pm.travelDirection = SceneMover.exitDirectionOptions.left;
                        }
                        else if (entranceDirection == SceneMover.exitDirectionOptions.down)
                        {
                            pm.endPosition = Player.transform.position + new Vector3(0, 0, 2);
                            pm.travelDirection = SceneMover.exitDirectionOptions.up;
                        }
                        addCutsceneEvent(moveEvent, Player, true, gameModeOptions.Cutscene);
                    }
                }
            }
            else
            {
                //SPAWNS PLAYERS AT A SPAWNPOINT
                Player = Instantiate(playerInput, spawnPoint.transform.position, Quaternion.identity);
            }
        }
        else
        {
            //SPAWNS PLAYER WHERE THEY STARTED COMBAT
            GameDataTracker.Save();
            Player = Instantiate(playerInput, GameDataTracker.combatStartPosition, Quaternion.identity);
            GameDataTracker.lastAreaWasCombat = false;
        }
        trackingCamera = Instantiate<GameObject>(trackingCameraInput, Player.transform.position + new Vector3(0,1,-2), Quaternion.Euler(25,0,0));
        trackingCamera.GetComponent<CameraFollow>().ObjectToTrack = Player;
        trackingCamera.GetComponent<CameraFollow>().combat = false;
        updateTrackingCameraY(Player.transform.position.y);
    }

    //WAYS TO ADD CUTSCENE EVENTS------------------------------------------------------------------------------------------------------------------------------------------------
    public static void addCutsceneEvent(GameObject CutsceneEventInput, GameObject CutsceneTargetInput, bool WaitInput, gameModeOptions GameModeInput, GameObject CameraFocusInput, Vector3 CameraOffsetInput)
    {
        CutsceneEventInput.SetActive(false);
        CutSceneEvent newEvent = new CutSceneEvent();
        newEvent.CutsceneEvent = CutsceneEventInput;
        newEvent.CutsceneTarget = CutsceneTargetInput;
        newEvent.Wait = WaitInput;
        newEvent.GameMode = GameModeInput;
        newEvent.CameraFocus = CameraFocusInput;
        newEvent.CameraOffset = CameraOffsetInput;
        CutsceneQueue.Add(newEvent);
    }

    public static void addCutsceneEvent(GameObject CutsceneEventInput, GameObject CutsceneTargetInput, bool WaitInput, gameModeOptions GameModeInput)
    {
        CutsceneEventInput.SetActive(false);
        CutSceneEvent newEvent = new CutSceneEvent();
        newEvent.CutsceneEvent = CutsceneEventInput;
        newEvent.CutsceneTarget = CutsceneTargetInput;
        newEvent.Wait = WaitInput;
        newEvent.GameMode = GameModeInput;
        newEvent.CameraFocus = null;
        newEvent.CameraOffset = Vector3.zero;
        CutsceneQueue.Add(newEvent);
    }
    public static void addCutsceneEventFRONT(GameObject CutsceneEventInput, GameObject CutsceneTargetInput, bool WaitInput, gameModeOptions GameModeInput, GameObject CameraFocusInput, Vector3 CameraOffsetInput)
    {
        CutsceneEventInput.SetActive(false);
        CutSceneEvent newEvent = new CutSceneEvent();
        newEvent.CutsceneEvent = CutsceneEventInput;
        newEvent.CutsceneTarget = CutsceneTargetInput;
        newEvent.Wait = WaitInput;
        newEvent.GameMode = GameModeInput;
        newEvent.CameraFocus = CameraFocusInput;
        newEvent.CameraOffset = CameraOffsetInput;
        CutsceneQueue.Insert(0, newEvent);
    }

    public static void addCutsceneEventFRONT(GameObject CutsceneEventInput, GameObject CutsceneTargetInput, bool WaitInput, gameModeOptions GameModeInput)
    {
        CutsceneEventInput.SetActive(false);
        CutSceneEvent newEvent = new CutSceneEvent();
        newEvent.CutsceneEvent = CutsceneEventInput;
        newEvent.CutsceneTarget = CutsceneTargetInput;
        newEvent.Wait = WaitInput;
        newEvent.GameMode = GameModeInput;
        newEvent.CameraFocus = null;
        newEvent.CameraOffset = Vector3.zero;
        CutsceneQueue.Insert(0, newEvent);
    }
    //-------------------------------------------------------------------------------------------------------------------------------------------

    public void Update()
    {
        //Check if any dialogue is available.
        if ((gameMode == gameModeOptions.Mobile)|| (gameMode == gameModeOptions.DialogueReady))
        {
            gameMode = gameModeOptions.Mobile;
            float closestCharacterDistance = 100;
            GameObject closestCharacter = null;
            foreach (Character CharacterItem in CharacterList)
            {
                float distanceToPlayer = CharacterItem.CharacterObject.GetComponent<FriendlyNPCClass>().distanceToPlayer;
                if (distanceToPlayer < closestCharacterDistance)
                {
                    closestCharacterDistance = distanceToPlayer;
                    closestCharacter = CharacterItem.CharacterObject;
                }
            }
            foreach (Character CharacterItem in CharacterList)
            {
                CharacterItem.CharacterObject.GetComponent<FriendlyNPCClass>().readyForDialogue = false;
            }
            if (closestCharacterDistance < 1)
            {
                closestCharacter.GetComponent<FriendlyNPCClass>().readyForDialogue = true;
                gameMode = gameModeOptions.DialogueReady;
            }
        }

        if (CutscenesPlaying == 0 && CutsceneQueue.Count > 0)
        {
            bool keepPlaying = true;
            while (keepPlaying)
            {
                CutscenesPlaying++;
                CutSceneEvent eventInitiation = CutsceneQueue[0];
                eventInitiation.CutsceneEvent.SetActive(true);
                eventInitiation.CutsceneEvent.transform.SetParent(eventInitiation.CutsceneTarget.transform);
                if(eventInitiation.Wait == true)
                {
                    keepPlaying = false;
                }
                gameMode = eventInitiation.GameMode;
                CutsceneQueue.Remove(eventInitiation);
            }
        }
        if (CutscenesPlaying == 0 && CutsceneQueue.Count == 0)
        {
            gameMode = gameModeOptions.Mobile;
        }
    }

    public static Character findCharacterByName(string Name, List<Character> charList)
    {
        Character foundCharacter = new Character();
        foundCharacter.CharacterName = "NoNamesLikeThat";
        foundCharacter.CharacterObject = null;
        foreach(Character charItem in charList){
            if(charItem.CharacterName == Name)
            {
                foundCharacter = charItem;
                return (foundCharacter);
            }
        }
        return (foundCharacter);
    }
    public static Character findCharacterUniqueSceneID(int ID, List<Character> charList)
    {
        Character foundCharacter = new Character();
        foundCharacter.CharacterName = "NoNamesLikeThat";
        foundCharacter.CharacterObject = null;
        foundCharacter.uniqueSceneID = -1;
        foreach (Character charItem in charList)
        {
            if (charItem.uniqueSceneID == ID)
            {
                foundCharacter = charItem;
                return (foundCharacter);
            }
        }
        return (foundCharacter);
    }

    public static void updateTrackingCameraY(float newY)
    {
        trackingCamera.GetComponent<CameraFollow>().trackingcameraY = newY;
    }
}


