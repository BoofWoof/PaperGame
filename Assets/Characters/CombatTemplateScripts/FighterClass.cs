using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(movesetContainer))]
[RequireComponent(typeof(SpriteFlipper))]
//THIS IS THE CLASS ANY COMBAT CHARACTER USES
public class FighterClass : CombatObject
{
    //CutsceneContainers
    [HideInInspector] public List<LowHealthTriggerInfo> LowHealthTriggers = new List<LowHealthTriggerInfo>();

    //DAMAGE AND EFFECT TYPES-----------------------
    public enum attackType { Normal, Fire, Heal, LifeSteal, GuaranteedDamage };
    public enum statusEffects { None, Defending, Stunned, Pushable};
    public enum statusTrigger { TurnStart, TurnEnd}
    public enum attackLocation { All, Ground, Air, Water }
    public enum CharacterPosition { Ground, Air, Water }
    //----------------------------------------------

    public struct statusInfo
    {
        public statusEffects status;
        public int intensity;
        public statusTrigger trigger;
        public int timeRemaining;
    }

    public CharacterPosition characterPosition;

    [Header("Character Stats")]
    public bool friendly = false;
    
    public int HPMax = 25;
    public int HP = 20;
    public int Power = 2;
    public int Defense = 0;
    public int PushStrength = 1;
    public float WalkSpeed = 5.0f;
    public float JumpSpeed = 2.0f;
    public float SwimSpeed = 1.0f;

    public float CharacterHeight = 1.0f;
    public float CharacterWidth = 1.0f;

    public bool MustBeat = true;

    [Header("Status Effects")]
    public bool Paralyzed = false;
    public bool Dead = false;

    [Header("Display Options")]
    public bool displayHealth;
    private TextMeshPro healthText;

    [Header("Status Effects")]
    public List<statusInfo> characterStatus = new List<statusInfo>();

    [Header("Inspection Cutscene")]
    public Animator animator;
    public DialogueContainer inspectionInfo;

    //INPUT OF AVAIALBE ATTACKS--------------------------
    [HideInInspector]public movesetContainer moveContainer;
    [HideInInspector]public moveTemplate attack;
    //---------------------------------------------------

    public virtual void Awake()
    {
        Character thisNPCCharacter = new Character();
        thisNPCCharacter.CharacterObject = gameObject;
        thisNPCCharacter.CharacterName = name;
        thisNPCCharacter.dialogueHeight = CharacterHeight;
        thisNPCCharacter.uniqueSceneID = GetInstanceID();
        GameDataTracker.CharacterList.Add(thisNPCCharacter);
        moveContainer = GetComponent<movesetContainer>();
    }

    public override void Start()
    {
        base.Start();
        GameObject healthTextObject = new GameObject("Health Text");
        healthText = healthTextObject.AddComponent<TextMeshPro>();
        healthTextObject.transform.position = transform.position + new Vector3(0, CharacterHeight - 0.5f, 0);
        healthTextObject.transform.SetParent(transform);
        healthText.text = HP.ToString() + "/" + HPMax.ToString();
        healthText.fontSize = 2f;
        healthText.horizontalAlignment = HorizontalAlignmentOptions.Center;
        healthText.verticalAlignment = VerticalAlignmentOptions.Middle;
    }

    public override void Update()
    {
        base.Update();
        healthText.text = HP.ToString() + "/" + HPMax.ToString();
    }

    public virtual void TurnStart()
    {
        for (int statusIdx = characterStatus.Count - 1; statusIdx >= 0; statusIdx--)
        {
            statusInfo status = characterStatus[statusIdx];
            if(status.trigger == statusTrigger.TurnStart)
            {
                status.timeRemaining = status.timeRemaining - 1;
                if(status.timeRemaining <= 0)
                {
                    if(status.status == statusEffects.Defending)
                    {
                        Defense -= status.intensity;
                    }
                    characterStatus.RemoveAt(statusIdx);
                }
            }
        }
    }

    public virtual void TurnEnd()
    {
        for (int statusIdx = characterStatus.Count - 1; statusIdx >= 0; statusIdx--)
        {
            statusInfo status = characterStatus[statusIdx];
            if (status.trigger == statusTrigger.TurnEnd)
            {
                status.timeRemaining = status.timeRemaining - 1;
                if (status.timeRemaining <= 0)
                {
                    if (status.status == statusEffects.Stunned)
                    {
                        Paralyzed = false;
                        animator.SetTrigger("StunEnd");
                    }
                    if (status.status == statusEffects.Pushable)
                    {
                        Pushable = false;
                    }
                    characterStatus.RemoveAt(statusIdx);
                }
            }
        }
    }

    public void Defend(int intensity, int turns)
    {
        FighterClass.statusInfo defenseStatus = new FighterClass.statusInfo();
        defenseStatus.status = FighterClass.statusEffects.Defending;
        defenseStatus.intensity = intensity;
        defenseStatus.trigger = FighterClass.statusTrigger.TurnStart;
        defenseStatus.timeRemaining = turns;

        characterStatus.Add(defenseStatus);
        Defense += intensity;
    }

    public void Stun(int turns)
    {
        FighterClass.statusInfo stunStatus = new FighterClass.statusInfo();
        stunStatus.status = FighterClass.statusEffects.Stunned;
        stunStatus.trigger = FighterClass.statusTrigger.TurnEnd;
        stunStatus.timeRemaining = turns;

        characterStatus.Add(stunStatus);
        Paralyzed = true;
    }

    public void LightWeight(int turns)
    {
        FighterClass.statusInfo pushStatus = new FighterClass.statusInfo();
        pushStatus.status = FighterClass.statusEffects.Pushable;
        pushStatus.trigger = FighterClass.statusTrigger.TurnEnd;
        pushStatus.timeRemaining = turns;

        characterStatus.Add(pushStatus);
        Pushable = true;
    }

    public virtual moveTemplate GetMove()
    {
        if (attack is null)
        {
            int moveIndex = Random.Range(0, moveContainer.moves.Length);
            attack = moveContainer.moves[moveIndex].GetComponent<moveTemplate>();
            return attack;
        } else
        {
            return attack;
        }
    }

    public virtual void GetRoute(CombatExecutor combatExecutor)
    {
        AStar router = ScriptableObject.CreateInstance<AStar>();
        //Get Move
        moveTemplate move = GetMove();
        if(move.targetType != moveTemplate.TargetType.None)
        {
            //Get Goals
            List<GameObject> possibleTargets = combatExecutor.getTargets(move.targetType);
            if (combatExecutor.Clip.GetComponent<FighterClass>().Dead)
            {
                possibleTargets.Remove(combatExecutor.Clip);
            }
            if (combatExecutor.Partner != null)
            {
                if (combatExecutor.Partner.GetComponent<FighterClass>().Dead)
                {
                    possibleTargets.Remove(combatExecutor.Partner);
                }
            }
            List<Vector2Int> goalPos = new List<Vector2Int>();
            List<GameObject> goalObjects = new List<GameObject>();
            move.character = gameObject;
            (goalPos, goalObjects) = move.findGoals(possibleTargets);
            (Vector2Int newPos, FighterClass.CharacterPosition moveType, bool atGoal) = router.GetNextTile(
                this,
                pos,
                goalPos
                );
            if (atGoal)
            {
                List<GameObject> selectedTargets = new List<GameObject>();
                selectedTargets.Add(goalObjects[router.finalGoalIdx]);
                move.Activate(selectedTargets);
            }
            else
            {
                MoveCharacter((newPos.x - pos.x), (newPos.y - pos.y));
            }
        } else
        {
            move.character = gameObject;
            move.Activate(new List<GameObject>());
        }
    }

    public virtual void MoveCharacter(int HorChange, int VerChange)
    {
        Vector2Int EndPos = new Vector2Int(pos.x + HorChange, pos.y + VerChange);
        List<Vector2Int> potentialGridOccupations = PotentialGridOccupation(EndPos);

        if (HorChange > 0)
        {
            gameObject.GetComponent<SpriteFlipper>().setFacingRight();
        }
        if (HorChange < 0)
        {
            gameObject.GetComponent<SpriteFlipper>().setFacingLeft();
        }

        foreach (Vector2Int potentialGridOccupation in potentialGridOccupations)
        {
            if (!BattleMapProcesses.isThisOnTheGrid(EndPos)) return;
            if (!BattleMapProcesses.CanIMoveToTile(EndPos, this)) return;
        }
        if (BattleMapProcesses.isTileEmpty(potentialGridOccupations, gameObject))
        {
            MoveCharacterExecute(EndPos, WalkSpeed, JumpSpeed, CombatExecutor.characterGrid);
            return;
        }
        if (AttemptPush(potentialGridOccupations, HorChange, VerChange, WalkSpeed, PushStrength))
        {
            MoveCharacterExecute(EndPos, WalkSpeed, JumpSpeed, CombatExecutor.characterGrid);
            return;
        }
    }

    public override void MoveCharacterExecute(Vector2Int EndPos, float walkSpeed, float jumpSpeed, GameObject[,] grid)
    {
        base.MoveCharacterExecute(EndPos, walkSpeed, jumpSpeed, grid);
        animator.SetTrigger("Go");
        animator.speed = 2.0f;
    }

    public void postBufferAttackEffect(int amount, attackType type, statusEffects effects, attackLocation location, GameObject source)
    {
        int healthBeforeDamage = HP;
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
            return;
        }

        foreach(LowHealthTriggerInfo lowHealthTrigger in LowHealthTriggers)
        {
            if(HP <= lowHealthTrigger.TriggerValue && lowHealthTrigger.TriggerValue < healthBeforeDamage)
            {
                if (CombatExecutor.CutsceneDataManager.TriggerATrigger(lowHealthTrigger.Label))
                {
                    GameDataTracker.combatExecutor.cutsceneDeconstruct = ScriptableObject.CreateInstance<CutsceneDeconstruct>();
                    GameDataTracker.combatExecutor.cutsceneDeconstruct.Deconstruct(Resources.Load<DialogueContainer>(lowHealthTrigger.CutscenePath), name, gameObject);
                }
            }
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
        
        GameObject damageText = Instantiate<GameObject>(Resources.Load<GameObject>("DamageTextDebug"));
        damageText.transform.position = transform.position + new Vector3(CharacterWidth * 0.75f, CharacterHeight * 0.75f, 0);
        damageText.GetComponent<TextMeshPro>().text = damage.ToString();
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
        characterStatus = new List<statusInfo>();
        GameDataTracker.combatExecutor.IDiedBye(gameObject);
    }
}
