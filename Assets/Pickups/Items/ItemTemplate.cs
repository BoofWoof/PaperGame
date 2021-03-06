﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTemplate : moveTemplate
{
    public string itemDescription;
    public Sprite itemImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OverWorldUse(int itemIdx)
    {
        RemoveItem(itemIdx);
    }

    public override void Activate(List<GameObject> targets)
    {
        RemoveItem(moveIndex);
    }

    public void RemoveItem(int itemIdx)
    {
        GameDataTracker.playerData.Inventory.RemoveAt(itemIdx);
    }

    public virtual void CombatUse()
    {
        Debug.Log("CombatUseNotImplemented");
    }

    public virtual void CombatThrow()
    {
        Debug.Log("CombatThrowNotImplemented");
    }
}
