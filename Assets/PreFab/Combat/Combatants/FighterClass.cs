using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterClass : MonoBehaviour
{
    public enum attackType { Normal, Heal };
    public enum statusEffects { None };

    public int HPMax = 20;
    public int HP = 20;
    //ActionList
    public GameObject moveContainer = null;
    [HideInInspector] public bool myTurn = false;
    [HideInInspector] public int myID;
    [HideInInspector] public List<GameObject> friendlyList;
    [HideInInspector] public List<GameObject> enemyList;
    [HideInInspector] public bool friendly = false;

    public void nextTurn()
    {
        myTurn = false;
        transform.parent.GetComponent<CombatController>().nextTurn();
    }

    
    public void attackEffect(int amount, attackType type, statusEffects effects, GameObject source)
    {
        if(type == attackType.Normal)
        {
            HP -= amount;
        }
        if(type == attackType.Heal)
        {
            HP += amount;
            if(HP > HPMax)
            {
                HP = HPMax;
            }
        }
    }
    
}
