using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBobaScript : ItemTemplate
{
    public override void OverWorldUse(int itemIndex)
    {
        base.OverWorldUse(itemIndex);
        GameDataTracker.ChangeHealth(-2);
    }

    public override void Activate(List<GameObject> targets)
    {
        base.Activate(targets);
        foreach (GameObject target in targets)
        {
            target.GetComponent<FighterClass>().postBufferAttackEffect(2, FighterClass.attackType.Normal, FighterClass.statusEffects.None, FighterClass.attackLocation.Ground, character);
        }
    }
}
