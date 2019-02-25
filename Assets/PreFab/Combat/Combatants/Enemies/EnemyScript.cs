﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : FighterClass
{
    public GameObject dialogueEventInput;
    //ActionList
    public List<GameObject> actionsAvailable;

    void Awake()
    {
        //SETS BASIC VARIABLE--
        base.Awake();
        friendly = false;
        Power = 2;
        actionsAvailable = moveContainer.GetComponent<movesetContainer>().moveList;
        //----------------------
    }

    void Update()
    {

    }


    public void makeItTurn()
    {
        GameObject attackSelect = actionsAvailable[(int)Random.Range(0, actionsAvailable.Count - 1)];
        GameObject attackEntity = Instantiate<GameObject>(attackSelect, Vector3.zero, Quaternion.identity);
        attackEntity.GetComponent<MoveClass>().friendlySource = friendly;
        attackEntity.GetComponent<MoveClass>().power = Power;
        attackEntity.GetComponent<MoveClass>().sourceID = myID;
        attackEntity.GetComponent<MoveClass>().effect();
    }
}