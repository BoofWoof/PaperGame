using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : FighterClass
{
    //ActionList
    public List<GameObject> actionsAvailable;
    
    void Awake()
    {
        //SETS BASIC VARIABLE--
        base.Awake();
        friendly = false;
        //----------------------
    }
    
    void Update()
    {

    }
    public void makeItTurn()
    {
        //IT IS MY TURN YAY--
        myTurn = true;
        //-------------------
    }
}
