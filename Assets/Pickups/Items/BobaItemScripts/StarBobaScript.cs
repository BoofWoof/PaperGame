using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBobaScript : ItemTemplate
{
    public override void OverWorldUse(int itemIndex)
    {
        base.OverWorldUse(itemIndex);
        GameDataTracker.ChangeHealth(100);
    }

    public override void Activate(List<GameObject> targets)
    {
        base.Activate(targets);
        foreach (GameObject target in targets)
        {
            target.GetComponent<FighterClass>().HP -= 5;
        }
    }
}
