using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerinventorySlot : MonoBehaviour
{
    public PlayerinventorySlot next;
    public string itemName;
    public Item item;
    

    public void UseItemOverworld()
    {
        item.OverworldUse();
    }

    public void UseItemBattle()
    {
        item.BattleUse();
    }
}
