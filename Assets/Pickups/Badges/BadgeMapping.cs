using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadgeMapping : MonoBehaviour
{
    public static Sprite defaultImage;
    public Sprite inputDefaultImage;

    private static string[] badge_map = new string[128];

    void Awake()
    {
        defaultImage = inputDefaultImage;
        badge_map[0] = "VitaBadge";
        badge_map[1] = "VitaBadgePartner";
        badge_map[2] = "AttackBadgeTest";
    }

    public static GameObject getBadge(int badge_index)
    {
        GameObject badge = Resources.Load<GameObject>("Badges/" + badge_map[badge_index]);
        Debug.Log(badge);
        return badge;
    }


}
