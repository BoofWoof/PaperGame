using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBobaScript : ItemTemplate
{
    public override void OverWorldUse()
    {
        GameDataTracker.ChangeHealth(-2);
    }
}
