using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTemplate : CombatObject
{
    public override void Update()
    {
        base.Update();
        if(!(move is null))
        {
            if (move.Update())
            {
                Destroy(move);
                move = null;
                CombatExecutor.blockGrid[(int)pos.x, (int)pos.y].GetComponent<BlockTemplate>().ObjectTileEntered(this);
            }
        }
    }
}
