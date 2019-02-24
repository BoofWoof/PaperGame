using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTurn : CutSceneClass
{
    // Start is called before the first frame update
    void Start()
    {
        sceneLists.gameControllerAccess.GetComponent<CombatController>().nextTurn();
        cutsceneDone();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
