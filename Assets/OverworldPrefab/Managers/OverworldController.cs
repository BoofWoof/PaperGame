using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OverworldController : MonoBehaviour
{
    GameControls controls;

    //List of all nonplayer characters as well as a player specific lookup.
    public static GameObject Player;
    public static GameObject Partner;
    private static GameObject PauseMenu;
    //-----------------------------------------------------

    //Spawned Objects
    public GameObject playerInput;  //Player Object
    public GameObject pauseMenu; //Pause Menu Object
    public GameObject spawnPoint;  //Where to spawn if not transitioning between areas.
    public GameObject sourceCamera;
    public GameObject[] SceneTransfers;  //Triggers that will cause a scene transfer.
    
    public static DialogueContainer AreaInfo;
    public DialogueContainer AreaInfoInput;
    public static List<MorganTorchScript> AllMorganTorches;

    private void OnEnable()
    {
        controls.OverworldControls.Enable();
    }

    private void OnDisable()
    {
        controls.OverworldControls.Disable();
    }

    public void Awake()
    {
        AllMorganTorches = new List<MorganTorchScript>();
        MorganMaterialReset();
        AreaInfo = AreaInfoInput;
        controls = new GameControls();
        GameDataTracker.clearCharacterList();
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

                        GameDataTracker.cutsceneMode = GameDataTracker.cutsceneModeOptions.Cutscene;
                        PlayerTravelDirection pm = ScriptableObject.CreateInstance<PlayerTravelDirection>();
                        SceneMover.exitDirectionOptions entranceDirection = sceneTransfer.GetComponent<SceneMover>().exitDirection;
                        if (entranceDirection == SceneMover.exitDirectionOptions.up)
                        {
                            pm.endPosition = Player.transform.position + new Vector3(0, 0, -2);
                            pm.travelDirection = SceneMover.exitDirectionOptions.down;
                        }
                        else if (entranceDirection == SceneMover.exitDirectionOptions.left)
                        {
                            pm.endPosition = Player.transform.position + new Vector3(2, 0, 0);
                            pm.travelDirection = SceneMover.exitDirectionOptions.right;
                        }
                        else if (entranceDirection == SceneMover.exitDirectionOptions.right)
                        {
                            pm.endPosition = Player.transform.position + new Vector3(-2, 0, 0);
                            pm.travelDirection = SceneMover.exitDirectionOptions.left;
                        }
                        else if (entranceDirection == SceneMover.exitDirectionOptions.down)
                        {
                            pm.endPosition = Player.transform.position + new Vector3(0, 0, 2);
                            pm.travelDirection = SceneMover.exitDirectionOptions.up;
                        }
                        CutsceneController.addCutsceneEvent(pm, Player, true, GameDataTracker.cutsceneModeOptions.Cutscene);
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
    }

    public void Start()
    {
        CameraManager.CreateNewCamera(sourceCamera, Player, CameraManager.DefaultCameraOffset, CameraManager.DefaultRotation, true);

        GameDataTracker.spawnLastTransitionObject();
        //SpawnsPartner
        if (GameDataTracker.playerData.CurrentCompanion > 0)
        {
            SpawnPartner(GameDataTracker.playerData.CurrentCompanion);
        }
    }

    private void SpawnPartner(int PartnerID)
    {
        Partner = Instantiate(PartnerMapper.partnerMap[PartnerID], Player.transform.position, Quaternion.identity);
    }

    public static void SwapPartner(int PartnerID)
    {
        Vector3 partnerPosition = Partner.transform.position;
        Partner.GetComponent<FriendlyNPCClass>().DestroySelf();
        Partner = Instantiate(PartnerMapper.partnerMap[PartnerID], partnerPosition, Quaternion.identity);
    }

    public static void ChangePauseState()
    {
        if (GameDataTracker.pauseActive)
        {
            GameDataTracker.pauseActive = false;
            PauseMenu.SetActive(false);
        }
        else
        {
            GameDataTracker.pauseActive = true;
            PauseMenu.SetActive(true);
        }
    }

    //-------------------------------------------------------------------------------------------------------------------------------------------
    public void Update()
    {
        CameraManager.UpdateHeading();
        if (controls.OverworldControls.CycleLeft.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            CameraManager.CameraHeading -= 80f * Time.deltaTime;
        }
        if (controls.OverworldControls.CycleRight.phase == UnityEngine.InputSystem.InputActionPhase.Started)
        {
            CameraManager.CameraHeading += 80f * Time.deltaTime;
        }

        //Pause and unpause game. ================
        if (controls.OverworldControls.Inventory.triggered && (GameDataTracker.cutsceneMode == GameDataTracker.cutsceneModeOptions.Mobile))
        {
            ChangePauseState();
        }
        if (GameDataTracker.pauseActive)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }
        //=========================================
        //Check if any dialogue is available.
        if (GameDataTracker.cutsceneMode == GameDataTracker.cutsceneModeOptions.Mobile)
        {
            float closestCharacterDistance = 100;
            GameObject closestCharacter = null;
            foreach (Character CharacterItem in GameDataTracker.CharacterList)
            {
                FriendlyNPCClass npcClass = CharacterItem.CharacterObject.GetComponent<FriendlyNPCClass>();
                if (npcClass != null)
                {
                    float distanceToPlayer = npcClass.distanceToPlayer;
                    if (distanceToPlayer < closestCharacterDistance)
                    {
                        closestCharacterDistance = distanceToPlayer;
                        closestCharacter = CharacterItem.CharacterObject;
                    }
                }
            }
            foreach (Character CharacterItem in GameDataTracker.CharacterList)
            {
                FriendlyNPCClass npcClass = CharacterItem.CharacterObject.GetComponent<FriendlyNPCClass>();
                if (npcClass != null)
                {
                    npcClass.readyForDialogue = false;
                }
            }
            if (closestCharacterDistance < 1)
            {
                closestCharacter.GetComponent<FriendlyNPCClass>().readyForDialogue = true;
                GameDataTracker.dialogueReady = true;
            } else
            {
                GameDataTracker.dialogueReady = false;
            }
        } else
        {
            GameDataTracker.dialogueReady = false;
        }
    }

    public void MorganMaterialReset()
    {
        Shader.SetGlobalFloat("_DisappearDistance", 0);
        Shader.SetGlobalFloat("_ExtraTorch1DisappearDistance", 0);
        Shader.SetGlobalFloat("_ExtraTorch2DisappearDistance", 0);
        Shader.SetGlobalFloat("_ExtraTorch3DisappearDistance", 0);
    }
}


