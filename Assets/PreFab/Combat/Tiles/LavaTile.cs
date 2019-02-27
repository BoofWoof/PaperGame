using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTile : CombatTileClass
{
    public override void endOfTurn()
    {
        GameObject fireDamage = new GameObject();
        DealDamage d = fireDamage.AddComponent<DealDamage>();
        d.amount = 1;
        d.effects = FighterClass.statusEffects.None;
        d.location = FighterClass.attackLocation.Ground;
        d.source = gameObject;
        d.type = FighterClass.attackType.Fire;
        CombatController.addCutseenEvent(fireDamage, onTopOfTile, true);
    }
}
