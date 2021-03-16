using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMapper : MonoBehaviour
{
    public static GameObject[] objectMap;
    public static GameObject[] characterMap;
    public static GameObject[] blockMap;

    public GameObject[] inputObjectMap;
    public GameObject[] inputCharacterMap;
    public GameObject[] inputBlockMap;

    void Awake()
    {
        objectMap = inputObjectMap;
        characterMap = inputCharacterMap;
        blockMap = inputBlockMap;
        Debug.Log(characterMap);
    }
}
