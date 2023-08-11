using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitaBadgeScript : BadgeTemplate
{
    public override void OnEquip()
    {
        GameDataTracker.playerData.maxHealth += 5;
        GameDataTracker.playerData.health += 5;
    }
    public override void OnUnequip()
    {
        GameDataTracker.playerData.maxHealth -= 5;
        GameDataTracker.playerData.health -= 5;
        if (GameDataTracker.playerData.health < 1) {
            GameDataTracker.playerData.health = 1;
        }
    }
}
