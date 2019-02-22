using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS ONE DOES A HEAL ATTACK
public class MoveTest2Script : MoveClass
{
    void Start()
    {
        targetMode = targetModeTypes.Friends;
    }
    public override void effect()
    {
        if (friendlySource) {
            friendlyList[targetID].GetComponent<FighterClass>().attackEffect(power, FighterClass.attackType.Heal, FighterClass.statusEffects.None, friendlyList[sourceID]);
        } else
        {
            friendlyList[targetID].GetComponent<FighterClass>().attackEffect(power, FighterClass.attackType.Heal, FighterClass.statusEffects.None, enemyList[sourceID]);
        }
        print("You healed um!");
    }
}
