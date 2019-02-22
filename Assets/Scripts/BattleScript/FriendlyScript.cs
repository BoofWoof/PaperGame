using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyScript : FighterClass
{
    public GameObject MenuObject;
    private GameObject CombatMenu = null;

    // Start is called before the first frame update
    void Start()
    {
        friendly = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void makeItTurn()
    {
        //Create Rotating Menu
        CombatMenu = Instantiate<GameObject>(MenuObject, new Vector3(transform.position.x, transform.position.y + 2, transform.position.z), Quaternion.identity);
        CombatMenu.GetComponent<BattleMenu>().MenuTiles = moveContainer.GetComponent<movesetContainer>().moveList;
        CombatMenu.GetComponent<BattleMenu>().comControl = gameObject;
        CombatMenu.transform.parent = transform;
        myTurn = true;
        //UpdateMenu
        CombatMenu.GetComponent<BattleMenu>().friendlyList = friendlyList;
        CombatMenu.GetComponent<BattleMenu>().enemyList = enemyList;
        CombatMenu.GetComponent<BattleMenu>().myID = myID;
        CombatMenu.GetComponent<BattleMenu>().friendly = friendly;
    }
}
