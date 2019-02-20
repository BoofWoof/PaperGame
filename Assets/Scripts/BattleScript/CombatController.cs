using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatController : MonoBehaviour
{
    //SceneConstructers
    public GameObject PlayerInput;
    public GameObject PartnerInput;
    public GameObject Enemy_Select;
    public GameObject Scene_Select;
    public GameObject GUI_Select;
    //Constant Inputs
    public GameObject MenuObject;
    //Instantiated Scene Objects
    private List<GameObject> playerPositions;
    private List<GameObject> enemyPositions;
    private List<GameObject> enemyObjectList;
    private GameObject GUI;

    //Instantiated Scene Objects
    private GameObject scene;
    private List<GameObject> enemyList;
    private GameObject Player;
    private GameObject Partner;

    //GuiTest
    private GameObject tempScoreGuiElement;
    private int tempScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameObject CombatMenu = Instantiate<GameObject>(MenuObject, new Vector3(transform.position.x-4, transform.position.y-1, transform.position.z), Quaternion.identity);
        CombatMenu.GetComponent<BattleMenu>().comControl = gameObject;
        //Create Scene
        scene = (GameObject)Instantiate(Scene_Select, transform.position, Quaternion.identity);
        playerPositions = scene.GetComponent<BattleSceneContainer>().PlayerPositions;
        enemyPositions = scene.GetComponent<BattleSceneContainer>().EnemyPositions;
        //Load Enemies
        enemyObjectList = (List<GameObject>)Enemy_Select.GetComponent<EnemyContainer>().EnemyList;
        for (int i = 0; i < enemyObjectList.Count; i++)
        {
            Instantiate<GameObject>(enemyObjectList[i], enemyPositions[i].transform.position, Quaternion.identity);
        }
        Player = Instantiate<GameObject>(PlayerInput, playerPositions[0].transform.position, Quaternion.identity);
        Partner = Instantiate<GameObject>(PartnerInput, playerPositions[1].transform.position, Quaternion.identity);
        GUI = Instantiate<GameObject>(GUI_Select, Vector3.zero, Quaternion.identity);
        tempScoreGuiElement = GUI.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            tempScore++;
        }
        tempScoreGuiElement.GetComponent<UnityEngine.UI.Text>().text = "Score: " + tempScore;
    }
}
