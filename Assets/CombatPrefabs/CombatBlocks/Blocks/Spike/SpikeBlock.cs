using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBlock : BlockTemplate
{
    public override void EndTurnOn(FighterClass target)
    {
        target.postBufferAttackEffect(3, FighterClass.attackType.Normal, FighterClass.statusEffects.None, FighterClass.attackLocation.Ground, gameObject);
    }

    public override void TileEntered(FighterClass target)
    {
        base.TileEntered(target);
        target.postBufferAttackEffect(1, FighterClass.attackType.Normal, FighterClass.statusEffects.None, FighterClass.attackLocation.Ground, gameObject);
    }
}
