using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAttackBadge : BadgeTemplate
{
    public override void BattleStart()
    {
        GameDataTracker.playerData.maxHealth += 5;
        GameDataTracker.playerData.health += 5;
    }
}
