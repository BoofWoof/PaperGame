using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBlock : BlockTemplate
{
    public override void TileEntered(FighterClass enteredCharacter)
    {
        base.TileEntered(enteredCharacter);
        SlideObject(enteredCharacter);
    }

    public override void ObjectTileEntered(ObjectTemplate enteredObject)
    {
        SlideObject(enteredObject);
    }

    public void SlideObject(CombatObject targetObject)
    {
        Vector2 prevPos = targetObject.prevPos;
        Vector2 posDif = targetObject.pos - prevPos;
        if (Mathf.Abs(posDif.y) > Mathf.Abs(posDif.x))
        {
            posDif.x = 0;
        }
        else if (Mathf.Abs(posDif.y) < Mathf.Abs(posDif.x))
        {
            posDif.y = 0;
        }
        if (posDif.y != 0)
        {
            posDif.y /= Mathf.Abs(posDif.y);
        }
        if (posDif.x != 0)
        {
            posDif.x /= Mathf.Abs(posDif.x);
        }
        Vector2Int EndPos = targetObject.pos + new Vector2Int((int)posDif.x, (int)posDif.y);
        List<Vector2Int> potentialGridOccupations = targetObject.PotentialGridOccupation(EndPos);
        if (targetObject.AttemptPush(potentialGridOccupations, (int)posDif.x, (int)posDif.y, targetObject.LastMoveSpeed, 0))
        {
            targetObject.MoveCharacterExecute(EndPos, targetObject.LastMoveSpeed, targetObject.LastMoveSpeed, CombatExecutor.characterGrid);
        }
    }
}
