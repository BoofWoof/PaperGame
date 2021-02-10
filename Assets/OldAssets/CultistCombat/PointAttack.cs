using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAttack : MoveClass
{
    public override void effect()
    {
        GameObject attackTarget = CombatController.friendList[(int)Random.Range(0, CombatController.friendList.Count)].CharacterObject;
        PointAttackCutscene attack = new PointAttackCutscene();
        attack.source = enemyList[sourceID].CharacterObject;
        attack.amount = power;
        attack.type = FighterClass.attackType.Normal;
        attack.effects = FighterClass.statusEffects.None;
        CutsceneController.addCutsceneEvent(attack, attackTarget, true, OverworldController.gameModeOptions.Cutscene);
        actionDone();
    }
}
