using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOBobaScript : ItemTemplate
{
    public override void OverWorldUse()
    {
        GameDataTracker.ChangeHealth(2);
    }
}
