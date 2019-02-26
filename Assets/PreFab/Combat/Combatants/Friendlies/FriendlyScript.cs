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
        myID = CombatController.friendList.Count;
        CombatController.addFigherToList(gameObject, name, myID, friendly);
        //---------------------
    }

    public override void makeItTurn()
    {
        //CREATE A MENU OF YOUR MOVES---------------------------------------------------------------------------------------------------------------------------------
        CombatMenu = Instantiate<GameObject>(MenuObject, new Vector3(transform.position.x+0.25f, transform.position.y + 2.5f, transform.position.z), Quaternion.identity);
        CombatMenu.GetComponent<BattleMenu>().MenuTiles = moveContainer.GetComponent<movesetContainer>().moveList;
        CombatMenu.GetComponent<BattleMenu>().friendlySource = friendly;
        CombatMenu.GetComponent<BattleMenu>().sourceID = myID;
        CombatMenu.transform.parent = transform;
        //---------------------------------------------------------------------------------------------------------------------------------------------------------------
    }
}
