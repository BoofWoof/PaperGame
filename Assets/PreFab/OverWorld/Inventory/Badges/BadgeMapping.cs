using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadgeMapping : MonoBehaviour
{
    public static string[] nameMap;
    public static string[] descriptionMap;
    public static int[] badgeCost;
    public static Sprite[] imageMap;
    public static Sprite defaultImage;

    public string[] inputNameMap;
    public string[] inputDescriptionMap;
    public int[] inputBadgeCost;
    public Sprite[] inputImageMap;
    public Sprite inputDefaultImage;

    void Awake()
    {
        nameMap = inputNameMap;
        descriptionMap = inputDescriptionMap;
        badgeCost = inputBadgeCost;
        imageMap = inputImageMap;
        defaultImage = inputDefaultImage;
    }

    public static BadgeTemplate actionMap(int idx)
    {
        switch (idx)
        {
            case 0:
                return VitaBadgeScript.CreateInstance<VitaBadgeScript>();
            case 1:
                return VitaBadgeScript.CreateInstance<VitaBadgePartnerScript>();
            default:
                Debug.Log("No Matching Index");
                break;
        }
        return VitaBadgeScript.CreateInstance<VitaBadgeScript>();
    }
}
