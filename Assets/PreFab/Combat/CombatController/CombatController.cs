using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public class CutSceneEventCombat
{
    public GameObject CutsceneEvent;
    public GameObject CutsceneTarget;
    public bool Wait;
    public GameObject CameraFocus;
    public Vector3 CameraOffset;
}

public class CombatController : MonoBehaviour
{
    //CUTSCENE STUFF=============================================================================================
    //===========================================================================================================
    public static int cutScenesPlaying = 0;

    public static List<CutSceneEventCombat> cutsceneList = new List<CutSceneEventCombat>();

    public static Vector3 defaultOffset = new Vector3(0, 2.3f, -6f);
    public static GameObject defaultFocus;

    public static GameObject gameControllerAccess;
    //===========================================================================================================

    //LIST OF ENEMY AND FRIENDS
    public static List<Character> enemyList = new List<Character>();
    public static List<Character> friendList = new List<Character>();

    //TrackingCamera----------------
    public GameObject trackingCameraInput;
    [HideInInspector]
    public GameObject trackingCamera;
    //-------------------------------

    //SceneInputs--------------------------------
    public GameObject gameObjectSelect;
    //--------------------------------------------

    //SceneLocations------------------------------
    private GameObject[] playerPositions;
    private GameObject[] enemyPositions;
    //---------------------------------------------

    //GUI Elements-----------------------------------------------------
    private GameObject combatCanvas;
    private List<GameObject> HealthText= new List<GameObject>();
    //-----------------------------------------------------------------

    //Instantiated Scene-------------------------------------------
    private GameObject scene;
    //-------------------------------------------------------------

    //Whose Turn?----------------------------------------------------
    private bool friendlyTurn = true;
    private int IDTurn = 0;
    //--------------------------------------------------------------

    //Player/Partner
    GameObject Player;
    GameObject Partner;
    //

    void Start()
    {
        //RECORD THIS AS A PUBLIC VARIABLE FOR EASY ACCESS-----------
        gameControllerAccess = gameObject;
        //-----------------------------------------------------------

        //Empty Global Variable--------------------------
        enemyList = new List<Character>();
        friendList = new List<Character>();
        cutScenesPlaying = 0;
        //------------------------------------------------

        //Create Scene-----------------------------------------------------------------------------------
        scene = (GameObject)Instantiate(gameObjectSelect.GetComponent<combatObjectContainer>().Scene, transform.position, Quaternion.identity);
        defaultOffset = scene.GetComponent<BattleSceneContainer>().sceneDefaultOffset;
        //---------------------------------------------------------------------------------------------

        //Create Tracking Camera------------------------------------------------------------------------
        trackingCamera = Instantiate<GameObject>(trackingCameraInput, scene.transform.position + new Vector3(0, 2.5f, -6.5f), Quaternion.Euler(5,0,0));
        trackingCamera.GetComponent<Camera>().enabled = true;
        trackingCamera.GetComponent<CameraFollow>().OverworldCamera = false;
        trackingCamera.GetComponent<CameraFollow>().ObjectToTrack = scene;
        trackingCamera.GetComponent<CameraFollow>().offset = defaultOffset;
        trackingCamera.GetComponent<CameraFollow>().combat = true;
        defaultFocus = scene;
        //----------------------------------------------------------------------------------------------

        //Load Scene Positions------------------------------------------------------------------------
        playerPositions = scene.GetComponent<BattleSceneContainer>().PlayerPositions;
        enemyPositions = scene.GetComponent<BattleSceneContainer>().EnemyPositions;
        //-------------------------------------------------------------------------------------------
        
        //Load Enemies and Tiles From Container----------------------------------------------------------------
        GameObject[] enemyObjectList = (GameObject[])gameObjectSelect.GetComponent<combatObjectContainer>().EnemyList;
        GameObject[] enemyTileObjecTList = (GameObject[])gameObjectSelect.GetComponent<combatObjectContainer>().EnemyTileList;
        GameObject[] friendlyTileObjecTList = (GameObject[])gameObjectSelect.GetComponent<combatObjectContainer>().FriendlyTileList;
        //-------------------------------------------------------------------------------------------

        //Create Loaded Enemies And Tiles----------------------------------------------------------------------------------------------------
        for (int i = 0; i < enemyObjectList.Length; i++)
        {
            GameObject newTile = Instantiate<GameObject>(enemyTileObjecTList[i], enemyPositions[i].transform.position, Quaternion.identity);
            CombatTileClass newTileScript = newTile.GetComponent<CombatTileClass>();
            GameObject newEnemy = Instantiate<GameObject>(enemyObjectList[i], newTile.transform.position + new Vector3(0,newTileScript.halfTileHeight,0), Quaternion.identity);
            newEnemy.GetComponent<FighterClass>().tileOn = newTile;
            newTileScript.onTopOfTile = newEnemy;
        }
        //------------------------------------------------------------------------------------------------------------------------------------

        //Create Loaded Allies And Tiles-------------------------------------------------------------------------------------------------
        GameObject PlayerTile = Instantiate<GameObject>(friendlyTileObjecTList[0], playerPositions[0].transform.position, Quaternion.identity);
        CombatTileClass PlayerTileScript = PlayerTile.GetComponent<CombatTileClass>();
        Player = Instantiate<GameObject>(gameObjectSelect.GetComponent<combatObjectContainer>().Player, PlayerTile.transform.position + new Vector3(0,PlayerTileScript.halfTileHeight,0), Quaternion.identity);
        Player.GetComponent<FighterClass>().tileOn = PlayerTile;
        PlayerTileScript.onTopOfTile = Player;

        GameObject PartnerTile = Instantiate<GameObject>(friendlyTileObjecTList[1], playerPositions[1].transform.position, Quaternion.identity);
        CombatTileClass PartnerTileScript = PartnerTile.GetComponent<CombatTileClass>();
        Partner = Instantiate<GameObject>(gameObjectSelect.GetComponent<combatObjectContainer>().Partner, PartnerTile.transform.position + new Vector3(0, PartnerTileScript.halfTileHeight, 0), Quaternion.identity);
        Partner.GetComponent<FighterClass>().tileOn = PartnerTile;
        PartnerTileScript.onTopOfTile = Partner;
        //-------------------------------------------------------------------------------------------------------------------------------------

        //SET PLAYER ONE AS HAVING THE FIRST TURN-------------------------------------
        friendList[0].CharacterObject.GetComponent<FriendlyScript>().makeItTurn();
        //---------------------------------------------------------------------------

        //CREATE TEST GUI----------------------------------------------------------------------------------------------
        combatCanvas = new GameObject();
        Canvas c = combatCanvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler s = combatCanvas.AddComponent<CanvasScaler>();
        s.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        combatCanvas.AddComponent<GraphicRaycaster>();
        for(int i = 0; i < friendList.Count+ enemyList.Count; i++)
        {
            GameObject guiText = new GameObject();
            guiText.transform.SetParent(combatCanvas.transform);
            guiText.AddComponent<Text>();
            guiText.GetComponent<Text>().font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            guiText.GetComponent<Text>().color = new Color(0, 0, 0);
            guiText.GetComponent<RectTransform>().localPosition = new Vector3(-300, 150-i*20, 0);
            HealthText.Add(guiText);
        }
        //--------------------------------------------------------------------------------------------------------------

    }

    void Update()
    {
        //UPDATES THE TEST GUI HEALTH TEXT---------------------------------------------------------------------------------------------------------------------
        for (int i = 0; i < friendList.Count; i++)
        {
            HealthText[i].GetComponent<Text>().text = friendList[i].CharacterObject.GetComponent<FighterClass>().HP.ToString();
        }
        for (int j = 0; j < enemyList.Count; j++)
        {
            HealthText[j + friendList.Count].GetComponent<Text>().text = enemyList[j].CharacterObject.GetComponent<FighterClass>().HP.ToString();
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------

        //ADD ANY CUTSCENE EVENTS------------------------------------------------------
        bool keepLooping = true;
        if ((cutScenesPlaying==0) && (cutsceneList.Count > 0))
        {
            while ((keepLooping)&& (cutsceneList.Count > 0))
            {
                cutScenesPlaying++;
                CutSceneEventCombat initCutScene = cutsceneList[0];
                initCutScene.CutsceneEvent.SetActive(true);
                initCutScene.CutsceneEvent.transform.SetParent(initCutScene.CutsceneTarget.transform);
                if (initCutScene.Wait == true)
                {
                    keepLooping = false;
                }
                //CAMERA ADJUSTMENT------------------------------------------------
                if (initCutScene.CameraOffset != Vector3.zero)
                {
                    trackingCamera.GetComponent<CameraFollow>().offset = initCutScene.CameraOffset;
                    trackingCamera.GetComponent<CameraFollow>().ObjectToTrack = initCutScene.CameraFocus;
                }
                //-----------------------------------------------------------------
                cutsceneList.Remove(initCutScene);
            }
        }
        //-----------------------------------------------------------------------------
    }

    public static void updateIDs()
    {
        //UPDATE THE IDS FOR WHEN A CHARACTER IS ADDED OR REMOVED------------------
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].CharacterObject.GetComponent<EnemyScript>().myID = i;
        }
        for (int j = 0; j < friendList.Count; j++)
        {
            friendList[j].CharacterObject.GetComponent<FriendlyScript>().myID = j;
        }
        //--------------------------------------------------------------------------
    }

    public void nextTurn()
    {
        //Return Camera To Its Home-------------------
        trackingCamera.GetComponent<CameraFollow>().ObjectToTrack = scene;
        trackingCamera.GetComponent<CameraFollow>().offset = defaultOffset;
        //--------------------------------------------

        //END COMBAT------------------------------- this needs a lot more functionality, but it serves its purpose for now.
        if (friendList.Count == 0)
        {
            Application.Quit(0);
        }
        else if (enemyList.Count == 0)
        {
            Player.GetComponent<ClipCombat>().updateGameStats();
            SceneManager.LoadScene(GameDataTracker.previousArea, LoadSceneMode.Single);
        }
        //--------------------------------------------------
        else
        {

            //CHANGE IDTURN TO NEXT CHARACTER-----------------------------------------------
            IDTurn++;
            if (friendlyTurn)
            {
                if (IDTurn >= friendList.Count)
                {
                    IDTurn = 0;
                    friendlyTurn = false;
                }
            }
            else
            {
                if (IDTurn >= enemyList.Count)
                {
                    IDTurn = 0;
                    friendlyTurn = true;
                }
            }
            //---------------------------------------------------------------------------------

            //Tell The Character It Is Their Turn------------------------------------------------
            if (friendlyTurn)
            {
                friendList[IDTurn].CharacterObject.GetComponent<FriendlyScript>().makeItTurn();
            }
            else
            {
                enemyList[IDTurn].CharacterObject.GetComponent<EnemyScript>().makeItTurn();
            }
            //------------------------------------------------------------------------------------
        }
    }

    //FIGHTERLISTCONTROL==============================================================================================
    public static void addFigherToList(GameObject fighter, string fighterName, int fighterID, bool friendly)
    {
        Character newFighter = new Character();
        newFighter.CharacterObject = fighter;
        newFighter.CharacterName = fighterName;
        if (friendly)
        {
            friendList.Add(newFighter);
        }
        else
        {
            enemyList.Add(newFighter);
        }
    }
    //=================================================================================================================


    //CUTSCENE STUFF
    //ADD CUTSCENE EVENT----------------------------------
    public static void addCutseenEvent(GameObject cutsceneEvent, GameObject target, bool wait, GameObject cameraFocus, Vector3 cameraOffset)
    {
        CutSceneEventCombat newEvent = new CutSceneEventCombat();
        newEvent.CutsceneEvent = cutsceneEvent;
        newEvent.CutsceneTarget = target;
        newEvent.Wait = wait;
        if (cameraOffset == Vector3.zero)
        {
            newEvent.CameraFocus = defaultFocus;
            newEvent.CameraOffset = defaultOffset;
        }
        else
        {
            newEvent.CameraFocus = cameraFocus;
            newEvent.CameraOffset = cameraOffset;
        }
        cutsceneList.Add(newEvent);
    }
    //----------------------------------------------------
    //ADD CUTSCENE EVENT IN FRONT----------------------------------
    public static void addCutseenEventFRONT(GameObject cutsceneEvent, GameObject target, bool wait, GameObject cameraFocus, Vector3 cameraOffset)
    {
        CutSceneEventCombat newEvent = new CutSceneEventCombat();
        newEvent.CutsceneEvent = cutsceneEvent;
        newEvent.CutsceneTarget = target;
        newEvent.Wait = wait;
        if (cameraOffset == Vector3.zero)
        {
            newEvent.CameraFocus = defaultFocus;
            newEvent.CameraOffset = defaultOffset;
        }
        else
        {
            newEvent.CameraFocus = cameraFocus;
            newEvent.CameraOffset = cameraOffset;
        }
        cutsceneList.Insert(0, newEvent);
    }
    public static void addCutseenEvent(GameObject cutsceneEvent, GameObject target, bool wait)
    {
        CutSceneEventCombat newEvent = new CutSceneEventCombat();
        newEvent.CutsceneEvent = cutsceneEvent;
        newEvent.CutsceneTarget = target;
        newEvent.Wait = wait;
        newEvent.CameraFocus = null;
        newEvent.CameraOffset = Vector3.zero;
        cutsceneList.Add(newEvent);
    }
    //----------------------------------------------------
    //ADD CUTSCENE EVENT IN FRONT----------------------------------
    public static void addCutseenEventFRONT(GameObject cutsceneEvent, GameObject target, bool wait)
    {
        CutSceneEventCombat newEvent = new CutSceneEventCombat();
        newEvent.CutsceneEvent = cutsceneEvent;
        newEvent.CutsceneTarget = target;
        newEvent.Wait = wait;
        newEvent.CameraFocus = null;
        newEvent.CameraOffset = Vector3.zero;
        cutsceneList.Insert(0, newEvent);
    }
    //===========================================================================================================
}
