﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScriptCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        sprite.transform.Rotate(0,3,0);
    }
}
