using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectTemplate : CombatObject
{
    public bool Passable = false;


    public void Update()
    {
        if(!(move is null))
        {
            if (move.Update())
            {
                Destroy(move);
                move = null;
                GameDataTracker.combatExecutor.blockGrid[(int)pos.x, (int)pos.y].GetComponent<BlockTemplate>().ObjectTileEntered(this);
            }
        }
    }
}
