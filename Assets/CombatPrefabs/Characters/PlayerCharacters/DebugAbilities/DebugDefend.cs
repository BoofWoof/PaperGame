using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDefend : moveTemplate
{
    public override void Activate(List<GameObject> targets)
    {
        base.Activate(targets);
        foreach (GameObject target in targets)
        {
            int intensity = 1;
            FighterClass.statusInfo defenseStatus = new FighterClass.statusInfo();

            defenseStatus.status = FighterClass.statusEffects.Defending;
            defenseStatus.intensity = intensity;
            defenseStatus.trigger = FighterClass.statusTrigger.TurnStart;
            defenseStatus.timeRemaining = 1;

            FighterClass stats = target.GetComponent<FighterClass>();

            stats.characterStatus.Add(defenseStatus);
            stats.Defense += intensity;
        }
    }
}
