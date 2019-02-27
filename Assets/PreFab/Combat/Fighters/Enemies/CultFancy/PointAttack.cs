using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAttack : MoveClass
{
    public override void effect()
    {
        GameObject attack = new GameObject();
        GameObject attackTarget = CombatController.friendList[(int)Random.Range(0, CombatController.friendList.Count)].CharacterObject;
        attack.AddComponent<PointAttackCutscene>();
        attack.GetComponent<PointAttackCutscene>().source = enemyList[sourceID].CharacterObject;
        attack.GetComponent<PointAttackCutscene>().amount = power;
        attack.GetComponent<PointAttackCutscene>().type = FighterClass.attackType.Normal;
        attack.GetComponent<PointAttackCutscene>().effects = FighterClass.statusEffects.None;
        CombatController.addCutseenEvent(attack, attackTarget, true);
        actionDone();
    }
}
