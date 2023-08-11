using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMenuGen : moveTemplate
{
    public override void Activate(List<GameObject> targets)
    {
        List<int> inventory = GameDataTracker.playerData.Inventory;

        //These adds are only here for debugging.
        if(inventory.Count == 0)
        {
            inventory.Add(0);
            inventory.Add(1);
            inventory.Add(2);
            inventory.Add(0);
            inventory.Add(3);
            inventory.Add(1);
        }

        List<Sprite> itemSprite = new List<Sprite>();
        List<GameObject> moveList = new List<GameObject>();
        for (int inv_idx = 0; inv_idx < inventory.Count; inv_idx++)
        {
            GameObject item = ItemMapping.getItem(inventory[inv_idx]);
            itemSprite.Add(item.GetComponent<ItemTemplate>().itemImage);
            moveList.Add(item);
        }

        FighterClass stats = character.GetComponent<FighterClass>();
        BattleMenu menu = ScriptableObject.CreateInstance<BattleMenu>();
        menu.characterTarget = character;
        menu.characterHeight = stats.CharacterHeight;
        menu.characterWidth = stats.CharacterWidth;
        menu.movesList = moveList;
        menu.spriteList = itemSprite;

        GameDataTracker.combatExecutor.AddMenu(menu);
    }
}
