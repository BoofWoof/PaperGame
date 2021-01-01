using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyBobaScript : ItemTemplate
{
    public override void OverWorldUse()
    {
        GameDataTracker.ChangeHealth(3);
    }
}
