using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeAttackMove : MoveClass
{
    public override void effect()
    {
        SwipeAttackCutscene A = new SwipeAttackCutscene();
        A.amount = power;
        A.damageTarget = enemyList[targetID].CharacterObject;
        A.effects = FighterClass.statusEffects.None;
        A.location = FighterClass.attackLocation.All;
        A.source = friendlyList[sourceID].CharacterObject;
        A.type = FighterClass.attackType.Normal;
        CutsceneController.addCutsceneEvent(A, friendlyList[sourceID].CharacterObject, true, OverworldController.gameModeOptions.Cutscene);
    }
}
