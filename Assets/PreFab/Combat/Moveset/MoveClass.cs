using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoveClass : MonoBehaviour
{
    public int power = 1;

    public enum selectModeTypes { off, select_setup, selecting}
    public enum targetModeTypes { Self, Friends, Enemies, Others, NoTarget, Random}

    public GameObject cursorInput;
    public bool friendly;
    public targetModeTypes targetMode { get; set; } = targetModeTypes.Enemies;
    public int targetID = 0;
    public GameObject cursor = null;
    public bool released = true;
    public selectModeTypes selectMode { get; set; } = selectModeTypes.off;
    [HideInInspector] public int myID;
    [HideInInspector] public List<GameObject> friendlyList;
    [HideInInspector] public List<GameObject> enemyList;
    private bool canPress = false;



    public void Update()
    {
        if (selectMode == selectModeTypes.select_setup)
        {
            cursor = Instantiate<GameObject>(cursorInput, transform.position, Quaternion.identity);
            selectMode = selectModeTypes.selecting;
        }
        if (selectMode == selectModeTypes.selecting)
        {
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
            //ONLY INCLUDING ATTACK ENEMY TYPE FOR NOW
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
                cursor.GetComponent<CharacterSelectHover>().startPosition = new Vector3(enemyList[targetID].transform.position.x, enemyList[targetID].transform.position.y+1.0f, enemyList[targetID].transform.position.z);
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
                cursor.GetComponent<CharacterSelectHover>().startPosition = new Vector3(friendlyList[targetID].transform.position.x, friendlyList[targetID].transform.position.y + 1.0f, friendlyList[targetID].transform.position.z);
            }
            if ((Input.GetButtonDown("Fire1")) && (canPress))
            {
                selectMode = selectModeTypes.off;
                Destroy(cursor);
                effect();
                actionDone();
            }
            if ((Input.GetButtonDown("Fire2")) && (canPress))
            {
                selectMode = selectModeTypes.off;
                Destroy(cursor);
                transform.parent.GetComponent<BattleMenu>().enableMenu();
            }
            canPress = true;
        }
    }

    public void select()
    {
        transform.parent.GetComponent<BattleMenu>().disableMenu();
        selectMode = selectModeTypes.select_setup;
        canPress = false;
    }

    public virtual void effect()
    {

    }

    public void actionDone()
    {
        if (friendly)
        {
            friendlyList[myID].GetComponent<FighterClass>().nextTurn();
        } else
        {
            enemyList[myID].GetComponent<FighterClass>().nextTurn();
        }
        Destroy(transform.parent.gameObject);
    }
}
