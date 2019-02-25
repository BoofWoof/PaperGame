using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CombatController : MonoBehaviour
{
    //TrackingCamera----------------
    public GameObject trackingCameraInput;
    public GameObject trackingCamera;
    //-------------------------------

    //SceneInputsandLoadedObjects--------------------------------
    public GameObject PlayerInput;
    public GameObject PartnerInput;
    public GameObject Enemy_Select;
    public GameObject Scene_Select;
    private List<GameObject> enemyObjectList;
    //--------------------------------------------

    //SceneLocations------------------------------
    private List<GameObject> playerPositions;
    private List<GameObject> enemyPositions;
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
    
    void Start()
    {
        //RECORD THIS AS A PUBLIC VARIABLE FOR EASY ACCESS-----------
        sceneLists.gameControllerAccess = gameObject;
        //-----------------------------------------------------------

        //Empty Global Variable--------------------------
        sceneLists.enemyList = new List<GameObject>();
        sceneLists.friendList = new List<GameObject>();
        //------------------------------------------------

        //Create Scene-----------------------------------------------------------------------------------
        scene = (GameObject)Instantiate(Scene_Select, transform.position, Quaternion.identity);
        //---------------------------------------------------------------------------------------------

        //Create Tracking Camera------------------------------------------------------------------------
        trackingCamera = Instantiate<GameObject>(trackingCameraInput, scene.transform.position + new Vector3(0, 2.5f, -6.5f), Quaternion.Euler(5,0,0));
        trackingCamera.GetComponent<Camera>().enabled = true;
        trackingCamera.GetComponent<CameraFollow>().OverworldCamera = false;
        sceneLists.cameraTrackTarget = scene;
        sceneLists.cameraOffset = sceneLists.defaultOffset;
        sceneLists.defaultFocus = scene;
        //----------------------------------------------------------------------------------------------

        //Load Scene Positions------------------------------------------------------------------------
        playerPositions = scene.GetComponent<BattleSceneContainer>().PlayerPositions;
        enemyPositions = scene.GetComponent<BattleSceneContainer>().EnemyPositions;
        //-------------------------------------------------------------------------------------------

        //Load Enemies From Container----------------------------------------------------------------
        enemyObjectList = (List<GameObject>)Enemy_Select.GetComponent<EnemyContainer>().EnemyList;
        //-------------------------------------------------------------------------------------------

        //Create Loaded Enemies---------------------------------------------------------------------------------------------------------------
        for (int i = 0; i < enemyObjectList.Count; i++)
        {
            GameObject newEnemy = Instantiate<GameObject>(enemyObjectList[i], enemyPositions[i].transform.position, Quaternion.identity);
            sceneLists.enemyList.Add(newEnemy);
            newEnemy.GetComponent<FighterClass>().myID = sceneLists.enemyList.Count - 1;
            sceneLists.enemyList[i].transform.parent = transform;
        }
        //------------------------------------------------------------------------------------------------------------------------------------

        //Create Loaded Allies----------------------------------------------------------------------------------------------------------------
        sceneLists.friendList.Add(Instantiate<GameObject>(PlayerInput, playerPositions[0].transform.position, Quaternion.identity));
        sceneLists.friendList[0].transform.parent = transform;
        sceneLists.friendList[0].GetComponent<FighterClass>().myID = 0;
        sceneLists.friendList.Add(Instantiate<GameObject>(PartnerInput, playerPositions[1].transform.position, Quaternion.identity));
        sceneLists.friendList[1].transform.parent = transform;
        sceneLists.friendList[1].GetComponent<FighterClass>().myID = 1;
        //-------------------------------------------------------------------------------------------------------------------------------------
        
        //SET PLAYER ONE AS HAVING THE FIRST TURN-------------------------------------
        sceneLists.friendList[0].GetComponent<FriendlyScript>().makeItTurn();
        //---------------------------------------------------------------------------

        //CREATE TEST GUI----------------------------------------------------------------------------------------------
        combatCanvas = new GameObject();
        Canvas c = combatCanvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler s = combatCanvas.AddComponent<CanvasScaler>();
        s.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        combatCanvas.AddComponent<GraphicRaycaster>();
        for(int i = 0; i < sceneLists.friendList.Count+ sceneLists.enemyList.Count; i++)
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
        //UpdateCamera Tracking-----------------------------
        trackingCamera.GetComponent<CameraFollow>().ObjectToTrack = sceneLists.cameraTrackTarget;
        trackingCamera.GetComponent<CameraFollow>().offset = sceneLists.cameraOffset;
        //--------------------------------------------------
        //UPDATES THE TEST GUI HEALTH TEXT---------------------------------------------------------------------------------------------------------------------
        for (int i = 0; i < sceneLists.friendList.Count; i++)
        {
            HealthText[i].GetComponent<Text>().text = sceneLists.friendList[i].GetComponent<FighterClass>().HP.ToString();
        }
        for (int j = 0; j < sceneLists.enemyList.Count; j++)
        {
            HealthText[j + sceneLists.friendList.Count].GetComponent<Text>().text = sceneLists.enemyList[j].GetComponent<FighterClass>().HP.ToString();
        }
        //----------------------------------------------------------------------------------------------------------------------------------------------------

        //ADD ANY CUTSCENE EVENTS------------------------------------------------------
        bool keepLooping = true;
        if ((sceneLists.cutScenesPlaying==0) && (sceneLists.cutsceneEventList.Count > 0))
        {
            sceneLists.newScene = false;
            while ((keepLooping)&& (sceneLists.cutsceneEventList.Count > 0))
            {
                sceneLists.cutScenesPlaying++;
                sceneLists.cutsceneEventList[0].SetActive(true);
                sceneLists.cutsceneEventList[0].transform.SetParent(sceneLists.cutsceneTarget[0].transform);
                sceneLists.cutsceneEventList.Remove(sceneLists.cutsceneEventList[0]);
                sceneLists.cutsceneTarget.Remove(sceneLists.cutsceneTarget[0]);
                if (sceneLists.waitforNext[0] == true)
                {
                    keepLooping = false;
                }
                sceneLists.waitforNext.Remove(sceneLists.waitforNext[0]);
                //CAMERA ADJUSTMENT------------------------------------------------
                if (sceneLists.offsetList[0] != Vector3.zero)
                {
                    sceneLists.cameraOffset = sceneLists.offsetList[0];
                    sceneLists.cameraTrackTarget = sceneLists.cameraFocusList[0];
                }
                sceneLists.offsetList.Remove(sceneLists.offsetList[0]);
                sceneLists.cameraFocusList.Remove(sceneLists.cameraFocusList[0]);
                //-----------------------------------------------------------------
            }
        }
        //-----------------------------------------------------------------------------
    }

    public void updateIDs()
    {
        //UPDATE THE IDS FOR WHEN A CHARACTER IS ADDED OR REMOVED------------------
        for (int i = 0; i < sceneLists.enemyList.Count; i++)
        {
            sceneLists.enemyList[i].GetComponent<EnemyScript>().myID = i;
        }
        for (int j = 0; j < sceneLists.friendList.Count; j++)
        {
            sceneLists.friendList[j].GetComponent<FriendlyScript>().myID = j;
        }
        //--------------------------------------------------------------------------
    }

    public void nextTurn()
    {
        //Return Camera To Its Home-------------------
        sceneLists.cameraTrackTarget = scene;
        sceneLists.cameraOffset = sceneLists.defaultOffset;
        //--------------------------------------------

        //END COMBAT------------------------------- this needs a lot more functionality, but it serves its purpose for now.
        if (sceneLists.friendList.Count == 0)
        {
            Application.Quit(0);
        }
        else if (sceneLists.enemyList.Count == 0)
        {
            SceneManager.LoadScene("TestScene1", LoadSceneMode.Single);
        }
        else
        {
            //--------------------------------------------------

            //CHANGE IDTURN TO NEXT CHARACTER-----------------------------------------------
            IDTurn++;
            if (friendlyTurn)
            {
                if (IDTurn >= sceneLists.friendList.Count)
                {
                    IDTurn = 0;
                    friendlyTurn = false;
                }
            }
            else
            {
                if (IDTurn >= sceneLists.enemyList.Count)
                {
                    IDTurn = 0;
                    friendlyTurn = true;
                }
            }
            //---------------------------------------------------------------------------------

            //Tell The Character It Is Their Turn------------------------------------------------
            if (friendlyTurn)
            {
                sceneLists.friendList[IDTurn].GetComponent<FriendlyScript>().makeItTurn();
            }
            else
            {
                sceneLists.enemyList[IDTurn].GetComponent<EnemyScript>().makeItTurn();
            }
            //------------------------------------------------------------------------------------
        }
    }
}
