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
            target.GetComponent<FighterClass>().Defend(1, 1);
        }
    }
}
