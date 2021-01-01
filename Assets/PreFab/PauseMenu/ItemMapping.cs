using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemMapping : MonoBehaviour
{
    public static string[] nameMap;
    public static string[] descriptionMap;
    public static Sprite[] imageMap;
    public static Sprite defaultImage;

    public string[] inputNameMap;
    public string[] inputDescriptionMap;
    public Sprite[] inputImageMap;
    public Sprite inputDefaultImage;

    void Awake()
    {
        nameMap = inputNameMap;
        descriptionMap = inputDescriptionMap;
        imageMap = inputImageMap;
        defaultImage = inputDefaultImage;
    }
}
