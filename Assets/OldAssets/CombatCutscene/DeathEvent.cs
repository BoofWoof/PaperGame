using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEvent : CutSceneClass
{
    // Start is called before the first frame update
    void Start()
    {
    }

    override public bool Activate()
    {
        if (parent.GetComponent<FighterClass>().friendly)
        {
            CombatController.friendList.Remove(CombatController.friendList[parent.GetComponent<FighterClass>().myID]);
        }
        else
        {
            CombatController.enemyList.Remove(CombatController.enemyList[parent.GetComponent<FighterClass>().myID]);
        }
        CombatController.updateIDs();
        Destroy(parent);
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
