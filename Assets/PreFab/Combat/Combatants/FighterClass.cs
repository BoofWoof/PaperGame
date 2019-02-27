using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS IS THE CLASS ANY COMBAT CHARACTER USES
public class FighterClass : MonoBehaviour
{
    public string CharacterName = "NameMeYouDingus";

    //DAMAGE AND EFFECT TYPES-----------------------
    public enum attackType { Normal, Fire, Heal, LifeSteal, GuaranteedDamage };
    public enum statusEffects { None };
    public enum attackLocation {All, Ground, Air, Water}
    public enum passiveTileEffects {None, Burn, Zap}
    //----------------------------------------------

    //IDENTIFICATION OF THIS CHARACTER----------------
    [HideInInspector] public int myID;
    [HideInInspector] public bool friendly = false;
    [HideInInspector] public GameObject tileOn;
    //-----------------------------------------------

    //HEALTH VARIABLES AND DEFENSE----------------
    public int HPMax = 25;
    public int HP = 20;
    public int Power = 2;
    public int Defense = 0;
    public passiveTileEffects passTileEffect = passiveTileEffects.None;
    //---------------------------------

    //INPUT OF AVAIALBE ATTACKS--------------------------
    public GameObject moveContainer = null;
    //---------------------------------------------------

    //Where Did I Start?------------
    public Vector3 HomePosition;
    //------------------------------

    public GameObject damageGraphicInput;
    //-------------------------------------------------------------AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA-------------------------------------------bOOF

    public void Awake()
    {
        //GRAB THE IMPORTANT LISTS-----------------
        HomePosition = transform.position;
        //----------------------------------------
    }

    public virtual void makeItTurn()
    {
        HomePosition = transform.position;
    }

    public virtual void characterEndTurn()
    {
        tileOn.GetComponent<CombatTileClass>().endOfTurn();
    }
    
    public void attackEffect(int amount, attackType type, statusEffects effects, attackLocation location, GameObject source)
    {
        tileOn.GetComponent<CombatTileClass>().damageBuffer(amount, type, effects, location, source);
    }

    public void postBufferAttackEffect(int amount, attackType type, statusEffects effects, attackLocation location, GameObject source)
    {
        //CALLS THE METHODS FOR EACH ATTACK TYPE AND STATUS EFFECT.  
        //IMMUNITY AND SPECIAL EFFECTS TO DIFFERENT TYPES CAN BE MADE BY OVERRIDING THE VIRTUAL METHODS.
        if (type == attackType.Normal)
        {
            NormalDamage(amount, source);
        }
        if (type == attackType.Fire)
        {
            FireDamage(amount, source);
        }
        if (type == attackType.Heal)
        {
            Heal(amount, source);
        }
        if (type == attackType.LifeSteal)
        {
            LifeStealDamage(amount, source);
        }
        //----------------------------------------------------------------------------------------------

        //CHECK IF DEAD---------------------------
        if (HP <= 0)
        {
            death();
        }
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
        GameObject damageGraphic = Instantiate(damageGraphicInput, transform.position + new Vector3(0.25f, 1.25f, 0), Quaternion.identity);
        damageGraphic.GetComponent<DamageIndicator>().damageAmount = damage;
    }
    //-----------------------------------------------------------------------

    //This will be replaced with more specific damage types.  --------------
    public virtual void FireDamage(int amount, GameObject source)
    {
        int damage = amount - Defense;
        if (damage < 0)
        {
            damage = 0;
        }
        HP -= damage;
        GameObject damageGraphic = Instantiate(damageGraphicInput, transform.position + new Vector3(0.25f, 1.25f, 0), Quaternion.identity);
        damageGraphic.GetComponent<DamageIndicator>().damageAmount = damage;
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

    public virtual void death()
    {
        GameObject deathEvent = new GameObject();
        deathEvent.AddComponent<DeathEvent>();
        CombatController.addCutseenEventFRONT(deathEvent, gameObject, true);
    }
}
