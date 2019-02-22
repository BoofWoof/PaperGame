using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest1Script : MoveClass
{
    void Start()
    {
        targetMode = targetModeTypes.Enemies;
    }
    public override void effect()
    {
        if (friendly)
        {
            enemyList[targetID].GetComponent<FighterClass>().attackEffect(power, FighterClass.attackType.Normal, FighterClass.statusEffects.None, friendlyList[myID]);
        }
        else
        {
            enemyList[targetID].GetComponent<FighterClass>().attackEffect(power, FighterClass.attackType.Normal, FighterClass.statusEffects.None, enemyList[myID]);
        }
        print("You hit um!");
    }
}
