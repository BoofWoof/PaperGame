using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTileClass : MonoBehaviour
{
    public GameObject onTopOfTile;
    public float halfTileHeight = 0.5f;
    
    public virtual void damageBuffer(int amount, FighterClass.attackType type, FighterClass.statusEffects effects, FighterClass.attackLocation location, GameObject source)
    {
        onTopOfTile.GetComponent<FighterClass>().postBufferAttackEffect(amount, type, effects, location, source);
    }
    public virtual void endOfTurn()
    {
    }
}
