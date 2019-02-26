using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextTurn : CutSceneClass
{
    // Start is called before the first frame update
    void Start()
    {
        CombatController.gameControllerAccess.GetComponent<CombatController>().nextTurn();
        cutsceneDone();
    }
}
