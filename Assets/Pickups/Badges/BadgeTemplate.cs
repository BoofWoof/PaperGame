using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class BadgeTemplate : MonoBehaviour
{
    public string name;
    public string itemDescription;
    public int badgeCost;
    public Sprite sprite;

    public virtual void OnEquip()
    {

    }
    public virtual void OnUnequip()
    {

    }
    
    public virtual void EnterRoom()
    {

    }

    public virtual void WorldTick()
    {

    }
    
    public virtual void BattleStart()
    {

    }

    public virtual void BattleEnd()
    {

    }

    public virtual void TurnStart()
    {

    }

    public virtual void EnemyTurnEnd()
    {

    }

    public virtual void DamageBuffer()//TODO: add paramters for damage taken, make return the new damage
    {

    }

    public virtual void DamageDealt()//TODO: add paramters for damage done, make return the new damage
    {

    }


}
