using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeAttackMove : MoveClass
{
    public override void effect()
    {
        GameObject Attack = new GameObject();
        SwipeAttackCutscene A = Attack.AddComponent<SwipeAttackCutscene>();
        A.amount = power;
        A.damageTarget = enemyList[targetID].CharacterObject;
        A.effects = FighterClass.statusEffects.None;
        A.location = FighterClass.attackLocation.All;
        A.source = friendlyList[sourceID].CharacterObject;
        A.type = FighterClass.attackType.Normal;
        CombatController.addCutseenEvent(Attack, friendlyList[sourceID].CharacterObject, true);
    }
}
