using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveClass : MonoBehaviour
{
    //SELECTION MODE FOR MOVE AND TARGETING MODE FOR MOVE-------------------------------
    public enum selectModeTypes { off, select_setup, selecting }
    public selectModeTypes selectMode { get; set; } = selectModeTypes.off;
    public enum targetModeTypes { Self, Friends, Enemies, Others, NoTarget, Random }
    public targetModeTypes targetMode { get; set; } = targetModeTypes.Enemies;
    public int targetID = 0;
    //----------------------------------------------------------------------------------

    //HOW POWERFUL IS THIS MOVE--
    public int power = 1;
    //---------------------------

    //ID OF THE SOURCE-------------------------
    [HideInInspector] public int sourceID;
    public bool friendlySource;
    //-----------------------------------------

    //SELECTION CURSOR-----------------
    public GameObject cursorInput;
    public GameObject cursor = null;
    //---------------------------------

    //USER INPUT CONTROL-------------
    public bool released = true;
    private bool canPress = false;
    //------------------------------

    //LIST OF CHARACTERS------------------------------------
    [HideInInspector] public List<GameObject> friendlyList;
    [HideInInspector] public List<GameObject> enemyList;
    //------------------------------------------------------
    
    void Awake()
    {
        //LOAD CHARACTER LIST------------------
        friendlyList = sceneLists.friendList;
        enemyList = sceneLists.enemyList;
        //-------------------------------------
    }

    public void Update()
    {
        //SETS UP SELECTION AND SPAWNS A CURSOR------------------------------------------------------
        if (selectMode == selectModeTypes.select_setup)
        {
            cursor = Instantiate<GameObject>(cursorInput, transform.position, Quaternion.identity);
            selectMode = selectModeTypes.selecting;
        }
        //-------------------------------------------------------------------------------------------

        //SELECT THE TARGET-----------------------------------------------------------------------------------------------------------------------------------------------
        if (selectMode == selectModeTypes.selecting)
        {
            //CHECK IF THE TARGET SHOULD CHANGE---------------------
            float verticalInput = Input.GetAxis("Vertical");
            if (released) {
                if (verticalInput > 0)
                {
                    targetID++;
                    released = false;
                }
                if (verticalInput < 0)
                {
                    targetID--;
                    released = false;
                }
            }
            if (verticalInput == 0)
            {
                released = true;
            }
            //-----------------------------------------------------
            
            //DETERMINES WHO THE NEW TARGET WILL BE BASED ON TARGETING TYPE---------------------------------------------------------------------------------------------------------
            if (targetMode == targetModeTypes.Enemies)
            {
                if((targetID >= enemyList.Count))
                {
                    targetID = 0;
                }
                if((targetID < 0))
                {
                    targetID = enemyList.Count - 1;
                }
                cursor.GetComponent<CharacterSelectHover>().startPosition = new Vector3(enemyList[targetID].transform.position.x, enemyList[targetID].transform.position.y+2.0f, enemyList[targetID].transform.position.z);
            }
            if (targetMode == targetModeTypes.Friends)
            {
                if ((targetID >= friendlyList.Count))
                {
                    targetID = 0;
                }
                if ((targetID < 0))
                {
                    targetID = friendlyList.Count - 1;
                }
                cursor.GetComponent<CharacterSelectHover>().startPosition = new Vector3(friendlyList[targetID].transform.position.x, friendlyList[targetID].transform.position.y + 2.0f, friendlyList[targetID].transform.position.z);
            }
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------

            //INITIATES MOVE ON SELECTED TARGET------------------
            if ((Input.GetButtonDown("Fire1")) && (canPress))
            {
                selectMode = selectModeTypes.off;
                Destroy(cursor);
                effect();
                actionDone();
            }
            //---------------------------------------------------

            //GOES BACK TO MENU---------------------------------------------
            if ((Input.GetButtonDown("Fire2")) && (canPress))
            {
                selectMode = selectModeTypes.off;
                Destroy(cursor);
                transform.parent.GetComponent<BattleMenu>().enableMenu();
            }
            //--------------------------------------------------------------
            canPress = true; //this just makes sure that the click to select the menu item doesn't also select the target
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------
    }

    //WHAT HAS THE MOVE TAKE ITS ACTION------------------------------
    public void select()
    {
        transform.parent.GetComponent<BattleMenu>().disableMenu();
        selectMode = selectModeTypes.select_setup;
        canPress = false;
    }
    //-----------------------------------------------------------------

    //THIS IS FILLED IN WITH THE ACTION TO TAKE--
    public virtual void effect()
    {
    }
    //---------------------------------------------

    //WHAT TO DO WHEN THE ACTIONS ARE DONE-----------------------------------------
    public void actionDone()
    {
        GameObject TurnEnder = new GameObject();
        TurnEnder.AddComponent<NextTurn>();
        sceneLists.addCutseenEvent(TurnEnder, sceneLists.gameControllerAccess, true);
        if (friendlySource)
        {
            Destroy(transform.parent.gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }
    //-----------------------------------------------------------------------------
}
