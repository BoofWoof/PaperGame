using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS ONE DOES A LIFE STEAL ATTACK
public class MoveTest3Script : MoveClass
{
    void Start()
    {
        targetMode = targetModeTypes.Enemies;
    }
    public override void effect()
    {
        if (friendlySource)
        {
            enemyList[targetID].GetComponent<FighterClass>().attackEffect(power, FighterClass.attackType.LifeSteal, FighterClass.statusEffects.None, friendlyList[sourceID]);
        }
        else
        {
            enemyList[targetID].GetComponent<FighterClass>().attackEffect(power, FighterClass.attackType.LifeSteal, FighterClass.statusEffects.None, enemyList[sourceID]);
        }
        print("You stole that Health!");
    }
}
