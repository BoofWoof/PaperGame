using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAttack : MoveClass
{
    public override void effect()
    {
        GameObject attack = new GameObject();
        GameObject attackTarget = sceneLists.friendList[(int)Random.Range(0, sceneLists.friendList.Count)];
        attack.AddComponent<PointAttackCutscene>();
        attack.GetComponent<PointAttackCutscene>().source = enemyList[sourceID];
        attack.GetComponent<PointAttackCutscene>().amount = power;
        attack.GetComponent<PointAttackCutscene>().type = FighterClass.attackType.Normal;
        attack.GetComponent<PointAttackCutscene>().effects = FighterClass.statusEffects.None;
        sceneLists.addCutseenEvent(attack, attackTarget, true);
        actionDone();
    }
}
