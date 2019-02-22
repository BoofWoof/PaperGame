using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;




public class CombatController : MonoBehaviour
{
    //SceneConstructers
    public GameObject PlayerInput;
    public GameObject PartnerInput;
    public GameObject Enemy_Select;
    public GameObject Scene_Select;

    //Instantiated Scene Objects
    private List<GameObject> playerPositions;
    private List<GameObject> enemyPositions;
    private List<GameObject> enemyObjectList;

    //GUI Elements
    private GameObject combatCanvas;
    private List<GameObject> HealthText= new List<GameObject>();

    //Instantiated Scene Objects
    private GameObject scene;
    private List<GameObject> enemyList = new List<GameObject>();
    private List<GameObject> friendList = new List<GameObject>();
    

    //ID Update Needed
    private bool IDUpdateNeeded = false;

    //Whose Turn?
    private bool friendlyTurn = true;
    private int IDTurn = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Create Scene
        scene = (GameObject)Instantiate(Scene_Select, transform.position, Quaternion.identity);

        //Load Character Positions
        playerPositions = scene.GetComponent<BattleSceneContainer>().PlayerPositions;
        enemyPositions = scene.GetComponent<BattleSceneContainer>().EnemyPositions;

        //Load Enemies
        enemyObjectList = (List<GameObject>)Enemy_Select.GetComponent<EnemyContainer>().EnemyList;

        //Create Enemeies
        for (int i = 0; i < enemyObjectList.Count; i++)
        {
            enemyList.Add(Instantiate<GameObject>(enemyObjectList[i], enemyPositions[i].transform.position, Quaternion.identity));
            enemyList[i].transform.parent = transform;
        }

        //Create Allies
        friendList.Add(Instantiate<GameObject>(PlayerInput, playerPositions[0].transform.position, Quaternion.identity));
        friendList[0].transform.parent = transform;
        friendList.Add(Instantiate<GameObject>(PartnerInput, playerPositions[1].transform.position, Quaternion.identity));
        friendList[1].transform.parent = transform;

        //UPDATE IDS
        updateIDs();
        //SET PLAYER ONE AS HAVING THE FIRST TURN
        friendList[0].GetComponent<FriendlyScript>().makeItTurn();

        //CREATE GUI
        combatCanvas = new GameObject();
        Canvas c = combatCanvas.AddComponent<Canvas>();
        c.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler s = combatCanvas.AddComponent<CanvasScaler>();
        s.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        combatCanvas.AddComponent<GraphicRaycaster>();
        for(int i = 0; i < friendList.Count+enemyList.Count; i++)
        {
            GameObject guiText = new GameObject();
            guiText.transform.SetParent(combatCanvas.transform);
            guiText.AddComponent<Text>();
            guiText.GetComponent<Text>().text = "Boof";
            guiText.GetComponent<Text>().font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            guiText.GetComponent<Text>().color = new Color(0, 0, 0);
            guiText.GetComponent<RectTransform>().localPosition = new Vector3(-300, 150-i*20, 0);
            HealthText.Add(guiText);
        }
        //tempScoreGuiElement = GUI.transform.GetChild(0).gameObject;

    }

    // Update is called once per frame
    void Update()
    {
        //MAKE THIS BETTER LATER
        for(int i = 0; i < friendList.Count; i++)
        {
            HealthText[i].GetComponent<Text>().text = friendList[i].GetComponent<FighterClass>().HP.ToString();
        }
        for (int j = 0; j < enemyList.Count; j++)
        {
            HealthText[j + friendList.Count].GetComponent<Text>().text = enemyList[j].GetComponent<FighterClass>().HP.ToString();
        }
    }

    void updateIDs()
    {
        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyList[i].GetComponent<EnemyScript>().friendlyList = friendList;
            enemyList[i].GetComponent<EnemyScript>().enemyList = enemyList;
            enemyList[i].GetComponent<EnemyScript>().myID = i;
        }
        for (int j = 0; j < friendList.Count; j++)
        {
            friendList[j].GetComponent<FriendlyScript>().friendlyList = friendList;
            friendList[j].GetComponent<FriendlyScript>().enemyList = enemyList;
            friendList[j].GetComponent<FriendlyScript>().myID = j;
        }
    }

    public void nextTurn()
    {
        IDTurn++;
        if (friendlyTurn)
        {
            if(IDTurn >= friendList.Count)
            {
                IDTurn = 0;
                //friendlyTurn = false;
            }
        } else
        {
            /*
            IDTurn++;
            if (IDTurn >= enemyList.Count)
            {
                IDTurn = 0;
                friendlyTurn = true;
            }
            */
        }
        if (friendlyTurn)
        {
            friendList[IDTurn].GetComponent<FriendlyScript>().makeItTurn();
        } else
        {
            //enemyList[IDTurn].GetComponent<EnemyScript>().makeItTurn();
        }
    }
}
