﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Character
{
    public GameObject CharacterObject;
    public string CharacterName;
    public float dialogueHeight;
    public float uniqueSceneID;
}

public class OverworldController : MonoBehaviour
{
    //List of all nonplayer characters as well as a player specific lookup.
    public static List<Character> CharacterList;
    public static List<Character> EnemyList;
    public static GameObject Player;
    private static GameObject PauseMenu;
    //-----------------------------------------------------

    //Spawned Objects
    public GameObject playerInput;  //Player Object
    public GameObject pauseMenu; //Pause Menu Object
    public GameObject spawnPoint;  //Where to spawn if not transitioning between areas.
    public GameObject trackingCameraInput;  //What track camera to use.
    public static GameObject trackingCamera;  //Publically accessible camera.
    public GameObject[] SceneTransfers;  //Triggers that will cause a scene transfer.

    //GameplayMode---------------------------------------------------
    //-----------------------------------------------------------------


    public void Awake()
    {
        CharacterList = new List<Character>();
        EnemyList = new List<Character>();
        PauseMenu = Instantiate(pauseMenu, Vector3.zero, Quaternion.identity);
        PauseMenu.SetActive(false);
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

                        GameDataTracker.gameMode = GameDataTracker.gameModeOptions.Cutscene;
                        PlayerTravelDirection pm = ScriptableObject.CreateInstance<PlayerTravelDirection>();
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
                        CutsceneController.addCutsceneEvent(pm, Player, true, GameDataTracker.gameModeOptions.Cutscene);
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
            Player = Instantiate(playerInput, GameDataTracker.combatStartPosition, Quaternion.identity);
            GameDataTracker.lastAreaWasCombat = false;
        }
        trackingCamera = Instantiate<GameObject>(trackingCameraInput, Player.transform.position + new Vector3(0,1,-2), Quaternion.Euler(25,0,0));
        trackingCamera.GetComponent<CameraFollow>().ObjectToTrack = Player;
        trackingCamera.GetComponent<CameraFollow>().combat = false;
        updateTrackingCameraY(Player.transform.position.y);
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------
    public void Update()
    {
        //Pause and unpause game. ================
        if (Input.GetKeyDown(KeyCode.I) && (GameDataTracker.gameMode == GameDataTracker.gameModeOptions.Mobile || GameDataTracker.gameMode == GameDataTracker.gameModeOptions.Paused))
        {
            if (GameDataTracker.gameMode == GameDataTracker.gameModeOptions.Paused)
            {
                GameDataTracker.gameMode = GameDataTracker.gameModePre;
                PauseMenu.SetActive(false);
            }
            else
            {
                GameDataTracker.gameModePre = GameDataTracker.gameMode;
                GameDataTracker.gameMode = GameDataTracker.gameModeOptions.Paused;
                PauseMenu.SetActive(true);
            }
        }
        if (GameDataTracker.gameMode == GameDataTracker.gameModeOptions.Paused)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
        //=========================================
        //Check if any dialogue is available.
        if ((GameDataTracker.gameMode == GameDataTracker.gameModeOptions.Mobile) || (GameDataTracker.gameMode == GameDataTracker.gameModeOptions.DialogueReady))
        {
            GameDataTracker.gameMode = GameDataTracker.gameModeOptions.Mobile;
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
                GameDataTracker.gameMode = GameDataTracker.gameModeOptions.DialogueReady;
            }
        }
    }

    public static Character findCharacterByName(string Name, List<Character> charList)
    {
        Character foundCharacter = new Character();
        foundCharacter.CharacterName = "NoNamesLikeThat";
        foundCharacter.CharacterObject = null;
        foundCharacter.dialogueHeight = -1;
        foundCharacter.uniqueSceneID = -1;
        foreach (Character charItem in charList){
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
        foundCharacter.dialogueHeight = -1;
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
        if (trackingCamera != null)
        {
            trackingCamera.GetComponent<CameraFollow>().trackingcameraY = newY;
        }
    }
}


