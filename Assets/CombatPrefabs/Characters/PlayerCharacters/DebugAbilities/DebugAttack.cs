using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAttack : moveTemplate
{
    public override void Activate(List<GameObject> targets)
    {
        base.Activate(targets);
        foreach (GameObject target in targets)
        {
            target.GetComponent<FighterClass>().postBufferAttackEffect(character.GetComponent<FighterClass>().Power, FighterClass.attackType.Normal, FighterClass.statusEffects.None, FighterClass.attackLocation.Ground, character);
        }
    }
}
