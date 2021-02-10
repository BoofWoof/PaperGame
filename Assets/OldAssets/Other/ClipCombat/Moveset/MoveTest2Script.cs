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
            friendlyList[targetID].CharacterObject.GetComponent<FighterClass>().attackEffect(power, FighterClass.attackType.Heal, FighterClass.statusEffects.None, FighterClass.attackLocation.All, friendlyList[sourceID].CharacterObject);
        } else
        {
            friendlyList[targetID].CharacterObject.GetComponent<FighterClass>().attackEffect(power, FighterClass.attackType.Heal, FighterClass.statusEffects.None, FighterClass.attackLocation.All, enemyList[sourceID].CharacterObject);
        }
        print("You healed um!");
    }
}
