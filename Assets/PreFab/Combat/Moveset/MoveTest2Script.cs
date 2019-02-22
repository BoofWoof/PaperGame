using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTest2Script : MoveClass
{
    void Start()
    {
        targetMode = targetModeTypes.Friends;
    }
    public override void effect()
    {
        if (friendly) {
            friendlyList[targetID].GetComponent<FighterClass>().attackEffect(power, FighterClass.attackType.Heal, FighterClass.statusEffects.None, friendlyList[myID]);
        } else
        {
            friendlyList[targetID].GetComponent<FighterClass>().attackEffect(power, FighterClass.attackType.Heal, FighterClass.statusEffects.None, enemyList[myID]);
        }
        print("You healed um!");
    }
}
