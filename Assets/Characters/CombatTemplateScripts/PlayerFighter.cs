using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFighter : FighterClass
{
    [Header("Selection UI")]
    public GameObject SelectionWheel;
    public Sprite characterSelector;
    public Sprite characterSelectorFloor;
    public Material characterSelectorMaterial;

    public override void Update()
    {
        base.Update();
        if(CombatExecutor.objectGrid[pos.x, pos.y] != null)
        {
            CombatExecutor.objectGrid[pos.x, pos.y].GetComponent<ObjectTemplate>().Collect(this);
        }
    }

}
