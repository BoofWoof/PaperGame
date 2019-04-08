﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Badge : MonoBehaviour
{
    public string badgeName;
    public int slotReq;
    public int id;

    
    //==================================================OVERWORLD TRIGGERS===============================================
    public virtual void EnterRoom()
    {

    }

    public virtual void WorldTick()//On realtime tick
    {

    }


    //==================================================BATTLE TRIGGERS==================================================
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
