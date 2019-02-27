using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathEvent : CutSceneClass
{
    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent.gameObject.GetComponent<FighterClass>().friendly)
        {
            CombatController.friendList.Remove(CombatController.friendList[transform.parent.gameObject.GetComponent<FighterClass>().myID]);
        }
        else
        {
            CombatController.enemyList.Remove(CombatController.enemyList[transform.parent.gameObject.GetComponent<FighterClass>().myID]);
        }
        CombatController.updateIDs();
        Destroy(transform.parent.gameObject);
        cutsceneDone();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
