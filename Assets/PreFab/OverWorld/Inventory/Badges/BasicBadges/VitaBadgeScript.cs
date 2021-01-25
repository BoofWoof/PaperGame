using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VitaBadgePartnerScript : BadgeTemplate
{
    public override void OnEquip()
    {
        GameDataTracker.playerData.CompanionMaxHealth += 5;
        GameDataTracker.playerData.WerewolfHealth += 5;
    }
    public override void OnUnequip()
    {
        GameDataTracker.playerData.CompanionMaxHealth -= 5;
        GameDataTracker.playerData.WerewolfHealth -= 5;
        if (GameDataTracker.playerData.WerewolfHealth < 1) {
            GameDataTracker.playerData.WerewolfHealth = 1;
        }
    }
}
