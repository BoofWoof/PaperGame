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

    public static ItemTemplate actionMap(int idx)
    {
        switch (idx)
        {
            case 0:
                return HoneyBobaScript.CreateInstance<HoneyBobaScript>();
            case 1:
                return StarBobaScript.CreateInstance<StarBobaScript>();
            case 2:
                return UFOBobaScript.CreateInstance<UFOBobaScript>();
            case 3:
                return FishBobaScript.CreateInstance<FishBobaScript>();
            default:
                Debug.Log("No Matching Index");
                break;
        }
        return HoneyBobaScript.CreateInstance<ItemTemplate>();
    }
}
