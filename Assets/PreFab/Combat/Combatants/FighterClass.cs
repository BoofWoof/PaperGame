﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS IS THE CLASS ANY COMBAT CHARACTER USES
public class FighterClass : MonoBehaviour
{
    //DAMAGE AND EFFECT TYPES-----------------------
    public enum attackType { Normal, Heal, LifeSteal, GuaranteedDamage };
    public enum statusEffects { None };
    //----------------------------------------------

    //IMPORTANT LIST OF ALL OTHER CHARACTERS IN SCENE--------
    [HideInInspector] public List<GameObject> friendlyList;
    [HideInInspector] public List<GameObject> enemyList;
    //--------------------------------------------------------

    //IDENTIFICATION OF THIS CHARACTER----------------
    [HideInInspector] public int myID;
    [HideInInspector] public bool friendly = false;
    //-----------------------------------------------

    //HEALTH VARIABLES AND DEFENSE----------------
    public int HPMax = 25;
    public int HP = 20;
    public int Defense = 0;
    //---------------------------------

    //INPUT OF AVAIALBE ATTACKS--------------------------
    public GameObject moveContainer = null;
    //---------------------------------------------------

    //IS IT MY TURN!!!! :o -----------------------
    [HideInInspector] public bool myTurn = false;
    //-------------------------------------------------------

    //LIST OF CUSTSCENE TASKS------------
    [HideInInspector] public List<GameObject> cutsceneTasks = new List<GameObject>();
    //-----------------------------------

    //SHOULD I END MY TURN?--------
    [HideInInspector] public bool endTurn = false;
    //-----------------------------

    public void Awake()
    {
        //GRAB THE IMPORTANT LISTS-----------------
        friendlyList = sceneLists.friendList;
        enemyList = sceneLists.enemyList;
        //----------------------------------------
    }

    private void Update()
    {
        //DO I NEED TO PLAY ANY COOL CUTSCENES-----------------------
        if ((cutsceneTasks.Count > 0)&&(sceneLists.cutScenePlaying==false))
        {
            sceneLists.cutScenePlaying = true;
            GameObject c = Instantiate<GameObject>(cutsceneTasks[0], Vector3.zero, Quaternion.identity);
            c.transform.SetParent(transform);
        }

        //DO I NEED TO END THE TURN--------------------------------------
        if((cutsceneTasks.Count == 0)&&(sceneLists.cutScenePlaying == false)&&(endTurn == true))
        {
            nextTurn();
            endTurn = false;
        }
        //-----------------------------------------------------------------
    }

    public void cutSceneDone()
    {
        //Gets rid of the finished cutscene task.----
        GameObject c = cutsceneTasks[0];
        cutsceneTasks.Remove(c);
        sceneLists.cutScenePlaying = false;
        //------------------------------------------
    }

    public void nextTurn()
    {
        //GIVE UP YOUR TURN TO SOMEONE ELSE YA CHUMP-----------------------
        myTurn = false;
        transform.parent.GetComponent<CombatController>().nextTurn();
        //-----------------------------------------------------------------
    }

    
    public void attackEffect(int amount, attackType type, statusEffects effects, GameObject source)
    {
        //CALLS THE METHODS FOR EACH ATTACK TYPE AND STATUS EFFECT.  
        //IMMUNITY AND SPECIAL EFFECTS TO DIFFERENT TYPES CAN BE MADE BY OVERRIDING THE VIRTUAL METHODS.
        if(type == attackType.Normal)
        {
            NormalDamage(amount, source);
        }
        if(type == attackType.Heal)
        {
            Heal(amount, source);
        }
        if (type == attackType.LifeSteal)
        {
            LifeStealDamage(amount, source);
        }
        //----------------------------------------------------------------------------------------------

        //CHECK IF DEAD---------------------------
        if(HP <= 0)
        {
            death();
        }
        //----------------------------------------
    }

    public virtual void death()
    {
        if (friendly)
        {
            friendlyList.Remove(gameObject);
        } else
        {
            enemyList.Remove(gameObject);
        }
        transform.parent.GetComponent<CombatController>().updateIDs();
        Destroy(gameObject);
    }

    //This will be replaced with more specific damage types.  --------------
    public virtual void NormalDamage(int amount, GameObject source)
    {
        int damage = amount - Defense;
        if(damage < 0)
        {
            damage = 0;
        }
        HP -= damage;
    }
    //-----------------------------------------------------------------------

    //Heals your character--------------------------------------------------
    public virtual void Heal(int amount, GameObject source)
    {
        HP += amount;
        if (HP > HPMax)
        {
            HP = HPMax;
        }
    }
    //--------------------------------------------------------------------

    //Stels your health------------------------------------------------------------------------------
    public virtual void LifeStealDamage(int amount, GameObject source)
    {
        int damage = amount - Defense;
        if (damage < 0)
        {
            damage = 0;
        }
        HP -= damage;
        source.GetComponent<FighterClass>().HP += damage;
        if(source.GetComponent<FighterClass>().HP > source.GetComponent<FighterClass>().HPMax)
        {
            source.GetComponent<FighterClass>().HP = source.GetComponent<FighterClass>().HPMax;
        }
    }
    //----------------------------------------------------------------------------------------------

    //Call this if you want it guaranteed the damage goes through.
    public void GuaranteedDamage(int amount)
    {
        HP -= amount;
    }
    //---------------------------------------------------------------------------------------------
}
