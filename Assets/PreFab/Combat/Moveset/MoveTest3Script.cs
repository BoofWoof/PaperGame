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
            enemyList[targetID].CharacterObject.GetComponent<FighterClass>().attackEffect(power, FighterClass.attackType.LifeSteal, FighterClass.statusEffects.None, FighterClass.attackLocation.All, friendlyList[sourceID].CharacterObject);
        }
        else
        {
            enemyList[targetID].CharacterObject.GetComponent<FighterClass>().attackEffect(power, FighterClass.attackType.LifeSteal, FighterClass.statusEffects.None, FighterClass.attackLocation.All, enemyList[sourceID].CharacterObject);
        }
        print("You stole that Health!");
    }
}
