using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartnerMapper : MonoBehaviour
{
    public static GameObject[] partnerMap;

    public GameObject[] partnerInput;

    void Awake()
    {
        partnerMap = partnerInput;
    }
}
