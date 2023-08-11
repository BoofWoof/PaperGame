using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMapping : MonoBehaviour
{
    public static Sprite defaultImage;
    public Sprite inputDefaultImage;

    private static string[] item_map = new string[128];


    void Awake()
    {
        defaultImage = inputDefaultImage;
        item_map[0] = "FishBoba";
        item_map[1] = "HoneyBoba";
        item_map[2] = "StarBoba";
        item_map[3] = "UFOBoba";
    }

    public static GameObject getItem(int item_index)
    {
        GameObject item = Resources.Load<GameObject>("Items/" + item_map[item_index]);
        Debug.Log(item);
        return item;
    }
}
