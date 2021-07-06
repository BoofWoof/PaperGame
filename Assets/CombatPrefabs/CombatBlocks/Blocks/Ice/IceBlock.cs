using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : BlockTemplate
{
    public override void TileEntered(FighterClass enteredCharacter)
    {
        Vector2 prevPos = enteredCharacter.prevPos;
        Vector2 posDif = enteredCharacter.pos - prevPos;
        enteredCharacter.PushObject((int)posDif.y, (int)posDif.x, 5f, true, GameDataTracker.combatExecutor);
    }

    public override void ObjectTileEntered(ObjectTemplate enteredObject)
    {
        Vector2 prevPos = enteredObject.prevPos;
        Vector2 posDif = enteredObject.pos - prevPos;
        enteredObject.PushObject((int)posDif.y, (int)posDif.x, 6f, false, GameDataTracker.combatExecutor);
    }
}
