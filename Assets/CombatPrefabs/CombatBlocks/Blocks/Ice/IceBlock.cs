using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : BlockTemplate
{
    public override void TileEntered(FighterClass enteredCharacter)
    {
        Vector2 prevPos = enteredCharacter.prevPos;
        Vector2 pos = enteredCharacter.pos;
        Vector2 posDif = pos - prevPos;
        GameDataTracker.combatExecutor.PushObject(pos, (int)posDif.y, (int)posDif.x, 5f, true);
    }

    public override void ObjectTileEntered(ObjectTemplate enteredObject)
    {
        Vector2 prevPos = enteredObject.prevPos;
        Vector2 pos = enteredObject.pos;
        Vector2 posDif = pos - prevPos;
        GameDataTracker.combatExecutor.PushObject(pos, (int)posDif.y, (int)posDif.x, 6f, false);
    }
}
