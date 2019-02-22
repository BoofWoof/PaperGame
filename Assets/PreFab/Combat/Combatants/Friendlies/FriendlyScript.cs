using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyScript : FighterClass
{
    public GameObject MenuObject;
    private GameObject CombatMenu = null;
    
    void Awake()
    {
        //SETS BASIC VARIABLE--
        base.Awake();
        friendly = true;
        //---------------------
    }

    public void makeItTurn()
    {
        //IT IS MY TURN YAY--------
        myTurn = true;
        //----------------------------

        //CREATE A MENU OF YOUR MOVES---------------------------------------------------------------------------------------------------------------------------------
        CombatMenu = Instantiate<GameObject>(MenuObject, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);
        CombatMenu.GetComponent<BattleMenu>().MenuTiles = moveContainer.GetComponent<movesetContainer>().moveList;
        CombatMenu.GetComponent<BattleMenu>().friendlySource = friendly;
        CombatMenu.GetComponent<BattleMenu>().sourceID = myID;
        CombatMenu.transform.parent = transform;
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
