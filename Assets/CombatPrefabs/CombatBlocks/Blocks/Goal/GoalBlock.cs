using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalBlock : BlockTemplate
{
    public bool active = false;

    public override void TileEntered(FighterClass enteredCharacter)
    {
        if (enteredCharacter.objectID <= 10)
        {
            active = true;
        }
    }

    public override void ObjectTileEntered(ObjectTemplate enteredObject)
    {
    }

    public void SlideObject(CombatObject targetObject)
    {
    }
}
