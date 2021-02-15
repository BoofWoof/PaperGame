using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMapping : MonoBehaviour
{
    public static GameObject[] itemMap;
    public static Sprite defaultImage;

    public GameObject[] itemMapInput;
    public Sprite inputDefaultImage;


    void Awake()
    {
        itemMap = itemMapInput;
        defaultImage = inputDefaultImage;
    }
}
