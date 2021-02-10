using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTile : CombatTileClass
{
    public override void endOfTurn()
    {
        DealDamage d = new DealDamage();
        d.amount = 1;
        d.effects = FighterClass.statusEffects.None;
        d.location = FighterClass.attackLocation.Ground;
        d.source = gameObject;
        d.type = FighterClass.attackType.Fire;
        CutsceneController.addCutsceneEvent(d, onTopOfTile, true, OverworldController.gameModeOptions.Cutscene);
    }
}
