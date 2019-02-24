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
            sceneLists.friendList.Remove(transform.parent.gameObject);
        }
        else
        {
            sceneLists.enemyList.Remove(transform.parent.gameObject);
        }
        sceneLists.gameControllerAccess.GetComponent<CombatController>().updateIDs();
        Destroy(transform.parent.gameObject);
        cutsceneDone();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
