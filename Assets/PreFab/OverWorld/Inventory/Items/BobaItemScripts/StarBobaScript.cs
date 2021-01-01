using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBobaScript : ItemTemplate
{
    public override void OverWorldUse()
    {
        GameDataTracker.ChangeHealth(100);
    }
}
