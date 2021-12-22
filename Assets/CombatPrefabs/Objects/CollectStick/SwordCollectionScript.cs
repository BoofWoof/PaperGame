using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordCollectionScript : ObjectTemplate
{
    public override void Collect(PlayerFighter collector)
    {
        base.Collect(collector);
        TurnManager turnManager = GameDataTracker.combatExecutor.turnManager;
        turnManager.EmptyList();
        turnManager.GoodGuysFirst();
        RemoveObject();
    }
}
