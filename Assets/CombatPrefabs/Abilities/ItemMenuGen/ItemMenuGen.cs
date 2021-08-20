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

        Sprite[] itemSprite = new Sprite[inventory.Count];
        GameObject[] moveArray = new GameObject[inventory.Count];
        for (int inv_idx = 0; inv_idx < inventory.Count; inv_idx++)
        {
            GameObject item = ItemMapping.itemMap[inventory[inv_idx]];
            itemSprite[inv_idx] = item.GetComponent<ItemTemplate>().itemImage;
            moveArray[inv_idx] = item;
        }

        FighterClass stats = character.GetComponent<FighterClass>();
        BattleMenu menu = ScriptableObject.CreateInstance<BattleMenu>();
        menu.characterTarget = character;
        menu.characterHeight = stats.CharacterHeight;
        menu.characterWidth = stats.CharacterWidth;
        menu.movesList = moveArray;
        menu.spriteList = itemSprite;

        GameDataTracker.combatExecutor.AddMenu(menu);
    }
}
