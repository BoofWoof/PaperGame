using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{


    public int size;
    public int maxSize;
    private PlayerinventorySlot head;

    //gets the PlayerinvertorySlot ovbject at given index
    public PlayerinventorySlot GetIndex(int index)
    {
        if (index > size)
        {
            return null;
        }
        PlayerinventorySlot temp = head;
        while (index > 0)
        {
            temp = temp.next;
            if (temp == null)
            {
                return null;
            }
        }
        return temp;
    }

    //gets the first instance of a PlayerinvertorySlot of a specific name.
    public PlayerinventorySlot GetFirstNameInstance(string name)
    {
        int index = 0;
        PlayerinventorySlot temp = head;
        while (index < size)
        {
            if (temp.itemName == name)
            {
                return temp;
            }
            temp = temp.next;
            if (temp == null)
            {
                return null;
            }
        }
        return null;
    }

    //deletes the object at the given index
    public void DeleteIndex(int index)
    {
        if (index > size - 1 && size > 0)
        {
            return;
        }

        PlayerinventorySlot temp = head;
        PlayerinventorySlot prev = head;
        while (index != 0)
        {
            prev = temp;
            temp = temp.next;
            index--;
        }
        prev.next = temp.next;
        size--;
    }

    //Adds this item to the head of the list
    public void AddItem(Item newItem)
    {
        PlayerinventorySlot newSlot = new PlayerinventorySlot();
        newSlot.item = newItem;
        newSlot.itemName = newItem.itemName;
        newSlot.next = head;
        head = newSlot;
    }

    //Adds this item to the head of the list
    public void AddItemTail(Item newItem)
    {
        PlayerinventorySlot temp = head;
        while(temp.next != null)
        {
            temp = temp.next;
        }

        PlayerinventorySlot newSlot = new PlayerinventorySlot();
        newSlot.item = newItem;
        newSlot.itemName = newItem.itemName;
        temp.next = newSlot;
    }

    //Uses the item at the given index in the context of overworld
    public void UseItemOverworld(int index)
    {
        PlayerinventorySlot item = GetIndex(index);
        if(item != null)
        {
            item.UseItemOverworld();
        }
    }

    //Uses the item at the given index in the context of battle. Will delete 
    public void UseItemBattle(int index)
    {
        PlayerinventorySlot item = GetIndex(index);
        if (item != null)
        {
            item.UseItemBattle();
        }
    }

}