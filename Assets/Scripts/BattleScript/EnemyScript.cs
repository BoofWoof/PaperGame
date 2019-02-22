using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : FighterClass
{



    //ActionList
    public List<GameObject> actionsAvailable;

    // Start is called before the first frame update
    void Start()
    {
        friendly = false;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void makeItTurn()
    {
        myTurn = true;
    }
}
