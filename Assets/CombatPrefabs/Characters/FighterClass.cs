using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(movesetContainer))]
//THIS IS THE CLASS ANY COMBAT CHARACTER USES
public class FighterClass : MonoBehaviour
{
    public enum CharacterPosition{Ground, Air, Water}

    public string CharacterName = "NameMeYouDingus";

    public CharacterPosition characterPosition;

    [Header("Selection UI")]
    public GameObject SelectionWheel;
    public Sprite characterSelector;

    //DAMAGE AND EFFECT TYPES-----------------------
    public enum attackType { Normal, Fire, Heal, LifeSteal, GuaranteedDamage };
    public enum statusEffects { None };
    public enum attackLocation {All, Ground, Air, Water}
    public enum passiveTileEffects {None, Burn, Zap}
    //----------------------------------------------

    [Header("Character Stats")]
    //IDENTIFICATION OF THIS CHARACTER----------------
    public bool friendly = false;
    //-----------------------------------------------

    //HEALTH VARIABLES AND DEFENSE----------------
    public int HPMax = 25;
    public int HP = 20;
    public int Power = 2;
    public int Defense = 0;
    //---------------------------------

    //---------------------------------
    public float CharacterHeight = 1.0f;
    public float CharacterWidth = 1.0f;

    [Header("Display Options")]
    public bool displayHealth;
    private TextMeshPro healthText;

    //INPUT OF AVAIALBE ATTACKS--------------------------
    [HideInInspector]public movesetContainer moveContainer;
    //---------------------------------------------------

    public virtual void Awake()
    {
        moveContainer = GetComponent<movesetContainer>();
    }

    public virtual void Start()
    {
        GameObject healthTextObject = new GameObject("Health Text");
        healthText = healthTextObject.AddComponent<TextMeshPro>();
        healthTextObject.transform.position = transform.position + new Vector3(0, CharacterHeight, 0);
        healthTextObject.transform.parent = transform;
        healthText.text = HP.ToString() + "/" + HPMax.ToString();
        healthText.fontSize = 2f;
        healthText.horizontalAlignment = HorizontalAlignmentOptions.Center;
        healthText.verticalAlignment = VerticalAlignmentOptions.Middle;
    }

    public virtual void Update()
    {
        healthText.text = HP.ToString() + "/" + HPMax.ToString();
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
        //DeathEvent deathEvent = new DeathEvent();
        //CutsceneController.addCutsceneEventFront(deathEvent, gameObject, true, OverworldController.gameModeOptions.Cutscene);
    }
}
