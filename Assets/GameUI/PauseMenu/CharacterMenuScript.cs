﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterMenuScript : MonoBehaviour
{
    public GameObject MainHealth;
    public GameObject AllyHealth;

    private void OnEnable()
    {
        MainHealth.GetComponent<TextMeshProUGUI>().text = "Health: " + GameDataTracker.playerData.health.ToString();
        AllyHealth.GetComponent<TextMeshProUGUI>().text = "health: " + GameDataTracker.playerData.WerewolfHealth.ToString();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
