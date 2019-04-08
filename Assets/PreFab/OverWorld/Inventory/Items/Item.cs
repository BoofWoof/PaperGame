using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Item : MonoBehaviour
{
    public string itemName;
    public int id;
    public virtual void OverworldUse()
    {

    }

    public virtual void BattleUse()
    {

    }
}
