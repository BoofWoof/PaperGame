using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BadgeMapping : MonoBehaviour
{
    public static GameObject[] badgeMap;
    public static Sprite defaultImage;

    public GameObject[] badgeMapInput;
    public Sprite inputDefaultImage;

    void Awake()
    {
        badgeMap = badgeMapInput;
        defaultImage = inputDefaultImage;
    }
}
