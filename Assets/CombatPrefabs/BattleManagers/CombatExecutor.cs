using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatExecutor : GridManager
{
    [Header("Loading")]
    public CombatContainer _containerCache;
    public CutsceneTrigger cutsceneTrigger;
    public CutsceneDeconstruct cutsceneDeconstruct;

    //Menu Info
    private List<BattleMenu> currentMenu = new List<BattleMenu>();
    private Targeter targeter = null;
    private moveTemplate selectedMove = null;

    [Header("UI Inputs")]
    public GameObject ClipHealth;
    public GameObject ClipMaxHealth;
    public GameObject PartnerHealth;
    public GameObject PartnerMaxHealth;
    public GameObject AbilityDescription;
    public GameObject AbilityDescriptionText;
    public GameObject TimerContainer;
    public GameObject TimerBar;

    [Header("UI Materials")]
    public Material spriteMaterial;

    //TurnManager
    private TurnManager turnManager;
    private TurnManager.turnPhases currentTurn;
    private float turnLength = 10.0f;
    private float turnTimeLeft;
    private float startGameWait = 1.5f;

    private void Awake()
    {
        GameDataTracker.clearCharacterList();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!(GameDataTracker.combatScene is null))
        {
            _containerCache = GameDataTracker.combatScene;
        }
        if (!(GameDataTracker.cutsceneTrigger is null))
        {
            cutsceneTrigger = GameDataTracker.cutsceneTrigger;
        }

        AbilityDescription.SetActive(false);
        TimerContainer.SetActive(false);

        GameDataTracker.combatExecutor = this;

        Load(_containerCache);

        //Update Clip And Partner Stats
        UpdateHealth();

        //Set Turn
        turnManager = ScriptableObject.CreateInstance<TurnManager>();
        if(puzzleMode || doublePuzzleMode)
        {
            currentTurn = turnManager.Puzzle();
        } else
        {
            currentTurn = turnManager.GoodGuysFirst();
        }

        //Set Camera Position
        SetCameraToWorld();
        if (!(cutsceneTrigger is null))
        {
            cutsceneTrigger.onCombatStart();
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealth();
        if (startGameWait > 0)
        {
            startGameWait -= Time.deltaTime;
        } else
        {
            if (noCutscenes())
            {
                if (currentTurn == TurnManager.turnPhases.ClipTurnStart)
                {
                    PlayerTurnStart(Clip);
                    return;
                }
                if (currentTurn == TurnManager.turnPhases.ClipTurn)
                {
                    if (selectedMove is null)
                    {
                        PlayerTurn(Clip);
                    }
                    else
                    {
                        Targeting();
                    }
                    return;
                }
                if (currentTurn == TurnManager.turnPhases.ClipTurnEnd)
                {
                    PlayerTurnEnd(Clip);
                    return;
                }
                if (currentTurn == TurnManager.turnPhases.PartnerTurnStart)
                {
                    PlayerTurnStart(Partner);
                    return;
                }
                if (currentTurn == TurnManager.turnPhases.PartnerTurn)
                {
                    if (selectedMove is null)
                    {
                        PlayerTurn(Partner);
                    }
                    else
                    {
                        Targeting();
                    }
                    return;
                }
                if (currentTurn == TurnManager.turnPhases.PartnerTurnEnd)
                {
                    PlayerTurnEnd(Partner);
                    return;
                }
                if (currentTurn == TurnManager.turnPhases.EnemyTurnStart)
                {
                    EnemyTurnStart();
                    return;
                }
                if (currentTurn == TurnManager.turnPhases.EnemyTurn)
                {
                    EnemyTurn();
                    return;
                }
                if (currentTurn == TurnManager.turnPhases.EnemyTurnEnd)
                {
                    EnemyTurnEnd();
                    return;
                }

                if (currentTurn == TurnManager.turnPhases.PuzzleTurn)
                {
                    PuzzleTurn();
                }
            } else
            {

            }
        }
    }

    private void PuzzleTurn()
    {
        turnTimeLeft = 1;
        bool allEnemyMoveDone = true;
        for (int enemyIdx = 0; enemyIdx < EnemyList.Count; enemyIdx++)
        {
            FighterClass enemy = EnemyList[enemyIdx].GetComponent<FighterClass>();
            if (enemy.move is null)
            {
                if (turnTimeLeft > 0 && !enemy.Paralyzed)
                {
                    allEnemyMoveDone = false;
                    enemy.GetRoute(this);
                }
            }
            else
            {
                allEnemyMoveDone = false;
                updateMove(enemy);
            }
        }

        movePlayerCharacter(allEnemyMoveDone);

        if (puzzleMode)
        {
            foreach(GoalBlock goalBlock in goalBlockList)
            {
                if(goalBlock.active == true)
                {
                    currentTurn = turnManager.NextTurn();
                }
            }
        }
        if (doublePuzzleMode)
        {
            bool allActive = true;
            foreach (GoalBlock goalBlock in goalBlockList)
            {
                if (goalBlock.active == false)
                {
                    allActive = false;
                    break;
                }
            }
            if (allActive)
            {
                currentTurn = turnManager.NextTurn();
            }
        }
        /*if ((turnTimeLeft <= 0 && Partner.GetComponent<FighterClass>().move is null &&
            Clip.GetComponent<FighterClass>().move is null && allEnemyMoveDone) ||
            EnemyList.Count == 0)
        {
            currentTurn = turnManager.NextTurn();
        }*/
    }

    private void movePlayerCharacter(bool allEnemyMoveDone)
    {
        int HorMov = 0;
        int VerMov = 0;
        if (Clip.GetComponent<FighterClass>().move is null)
        {
            FighterClass clipInfo = Clip.GetComponent<FighterClass>();
            clipInfo.animator.speed = 1.0f;
            if ((turnTimeLeft > 0 || !allEnemyMoveDone) && !clipInfo.Paralyzed && !clipInfo.Dead)
            {
                if (Input.GetButton("Fire2"))
                {
                    if (Input.GetAxis("Horizontal") > 0.3)
                    {
                        HorMov = 1;
                    }
                    else if (Input.GetAxis("Horizontal") < -0.3)
                    {
                        HorMov = -1;
                    }
                    else if (Input.GetAxis("Vertical") > 0.3)
                    {
                        VerMov = 1;
                    }
                    else if (Input.GetAxis("Vertical") < -0.3)
                    {
                        VerMov = -1;
                    }
                    if(HorMov != 0 || VerMov != 0) clipInfo.MoveCharacter(HorMov, VerMov);
                }
            }
        }
        else
        {
            updateMove(Clip.GetComponent<FighterClass>());
        }
        if (Partner != null)
        {
            if (Partner.GetComponent<FighterClass>().move is null)
            {
                FighterClass partnerInfo = Partner.GetComponent<FighterClass>();
                if ((turnTimeLeft > 0 || !allEnemyMoveDone) && !partnerInfo.Paralyzed && !partnerInfo.Dead)
                {
                    if (Input.GetButton("Fire3"))
                    {
                        if (Input.GetAxis("Horizontal") > 0.3)
                        {
                            HorMov = 1;
                        }
                        else if (Input.GetAxis("Horizontal") < -0.3)
                        {
                            HorMov = -1;
                        }
                        else if (Input.GetAxis("Vertical") > 0.3)
                        {
                            VerMov = 1;
                        }
                        else if (Input.GetAxis("Vertical") < -0.3)
                        {
                            VerMov = -1;
                        }
                        if (HorMov != 0 || VerMov != 0) partnerInfo.MoveCharacter(HorMov, VerMov);
                    }
                }
            }
            else
            {
                updateMove(Partner.GetComponent<FighterClass>());
            }
        }
    }

    private bool noCutscenes()
    {
        //Check deconstructor.
        if(!(cutsceneDeconstruct is null))
        {
            if (cutsceneDeconstruct.done)
            {
                Destroy(cutsceneDeconstruct);
                cutsceneDeconstruct = null;
            }
        }
        return CutsceneController.noCutscenes() && (cutsceneDeconstruct is null);
    }

    void PlayerTurnStart(GameObject Player)
    {
        FighterClass playerInfo = Player.GetComponent<FighterClass>();
        FocusOnCharacter(playerInfo.pos);
        playerInfo.GetComponent<FighterClass>().TurnStart();
        currentTurn = turnManager.NextTurn();
        if(playerInfo.Dead == true)
        {
            currentTurn = turnManager.NextTurn();
        }
    }

    void PlayerTurn(GameObject character)
    {
        FighterClass stats = character.GetComponent<FighterClass>();
        FocusOnCharacter(stats.pos);
        //If no option menu create one.
        if (currentMenu.Count == 0)
        {
            movesetContainer moves = character.GetComponent<movesetContainer>();

            BattleMenu menu = ScriptableObject.CreateInstance<BattleMenu>();
            menu.characterTarget = character;
            menu.characterHeight = stats.CharacterHeight;
            menu.characterWidth = stats.CharacterWidth;
            menu.movesList = moves.moves;
            menu.spriteList = moves.sprites;
            AddMenu(menu);
        } else
        {
            //Move back from a submenu.
            BattleMenu selectedMenu = currentMenu[currentMenu.Count - 1];
            if (Input.GetButtonDown("Fire3") && currentMenu.Count > 1)
            {
                selectedMenu.Deactivate();
                currentMenu.Remove(selectedMenu);
                selectedMenu = currentMenu[currentMenu.Count - 1];
                selectedMenu.Activate();
            }

            //Check if an action was selected.
            GameObject selectedObject = selectedMenu.UpdateMenu(Input.GetAxis("Horizontal"), Input.GetButtonDown("Fire2"));
            if (!(selectedObject is null))
            {
                selectedMenu.Deactivate();
                selectedMove = selectedObject.GetComponent<moveTemplate>();
                int prevMenuCount = currentMenu.Count;
                if (selectedMove.targetType == moveTemplate.TargetType.None)
                {
                    selectedMove.Activate(null);
                    if (prevMenuCount == currentMenu.Count)
                    {
                        currentMenu = new List<BattleMenu>();
                    }
                    selectedMove = null;
                }
            }
        }
    }

    void PlayerTurnEnd(GameObject Player)
    {
        FighterClass playerInfo = Player.GetComponent<FighterClass>();
        blockGrid[(int)playerInfo.pos.x, (int)playerInfo.pos.y].GetComponent<BlockTemplate>().EndTurnOn(playerInfo);
        currentTurn = turnManager.NextTurn();
    }

    public void AddMenu(BattleMenu newMenu)
    {
        newMenu.spriteShader = spriteMaterial;
        newMenu.Activate();
        currentMenu.Add(newMenu);
    }

    void EnemyTurnStart()
    {
        SetCameraToWorld();
        TimerContainer.SetActive(true);
        turnTimeLeft = turnLength;
        currentTurn = turnManager.NextTurn();
    }

    void EnemyTurn()
    {
        if (turnTimeLeft > 0)
        {
            turnTimeLeft -= Time.deltaTime;
            TimerBar.transform.localScale = new Vector3(turnTimeLeft / turnLength, 1, 1);
        } else
        {
            turnTimeLeft = 0;
        }

        bool allEnemyMoveDone = true;
        for (int enemyIdx = 0; enemyIdx < EnemyList.Count; enemyIdx++)
        {
            FighterClass enemy = EnemyList[enemyIdx].GetComponent<FighterClass>();
            if (enemy.move is null)
            {
                if (turnTimeLeft > 0 && !enemy.Paralyzed)
                {
                    allEnemyMoveDone = false;
                    enemy.GetRoute(this);
                }
            }
            else
            {
                allEnemyMoveDone = false;
                updateMove(enemy);
            }
        }

        movePlayerCharacter(allEnemyMoveDone);

        if ((turnTimeLeft <= 0 && Partner.GetComponent<FighterClass>().move is null && 
            Clip.GetComponent<FighterClass>().move is null && allEnemyMoveDone) || 
            EnemyList.Count == 0)
        {
            currentTurn = turnManager.NextTurn();
        }
    }

    void EnemyTurnEnd()
    {
        int enemyCount = EnemyList.Count;
        for(int enemyIdx = enemyCount - 1; enemyIdx >= 0; enemyIdx--)
        {
            GameObject enemy = EnemyList[enemyIdx];
            FighterClass enemyInfo = enemy.GetComponent<FighterClass>();
            enemyInfo.TurnEnd();
            blockGrid[(int)enemyInfo.pos.x, (int)enemyInfo.pos.y].GetComponent<BlockTemplate>().EndTurnOn(enemyInfo);
        }
        TimerContainer.SetActive(false);
        currentTurn = turnManager.NextTurn();
    }

    private void updateMove(FighterClass character)
    {
        if (character.move.Update())
        {
            character.animator.SetTrigger("Stop");
            character.animator.speed = 1.0f;
            Destroy(character.move);
            character.move = null;
            blockGrid[character.pos.x, character.pos.y].GetComponent<BlockTemplate>().TileEntered(character);
        }
    }

    void Targeting()
    {
        if (targeter is null)
        {
            SetCameraToWorld();
            AbilityDescription.SetActive(true);
            AbilityDescriptionText.GetComponent<Text>().text = selectedMove.combatDescription;
            targeter = ScriptableObject.CreateInstance<Targeter>();
            moveTemplate.TargetType targetType = selectedMove.targetType;
            targeter.source = selectedMove.character;
            List<GameObject> potentialTargets = getTargets(targetType);
            targeter.potentialTargets = selectedMove.targetFilter(potentialTargets);
            targeter.targetQuantity = selectedMove.targetQuantity;
            targeter.targeterSprite = selectedMove.character.GetComponent<FighterClass>().characterSelector;
            targeter.targeterSpriteFloor = selectedMove.character.GetComponent<FighterClass>().characterSelectorFloor;
            targeter.targeterSpriteMaterial = selectedMove.character.GetComponent<FighterClass>().characterSelectorMaterial;
            targeter.Activate();
        }
        else
        {
            if (Input.GetButtonDown("Fire3") && currentMenu.Count > 0)
            {
                if (targeter.Undo())
                {
                    selectedMove.hideRange();
                    selectedMove = null;
                    targeter = null;
                    BattleMenu selectedMenu = currentMenu[currentMenu.Count - 1];
                    selectedMenu.Activate();
                    AbilityDescription.SetActive(false);
                }
            }
            else if (targeter.potentialTargets.Count > 0)
            {
                List<GameObject> moveTargets = targeter.TargeterUpdate(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), Input.GetButtonDown("Fire2"));
                if (!(moveTargets is null))
                {
                    selectedMove.hideRange();
                    selectedMove.Activate(moveTargets);
                    selectedMove = null;
                    targeter = null;
                    currentMenu = new List<BattleMenu>();
                    currentTurn = turnManager.NextTurn();
                    AbilityDescription.SetActive(false);
                }
            }
        }
    }

    public List<GameObject> getTargets(moveTemplate.TargetType targetType)
    {
        List<GameObject> potentialTargets = new List<GameObject>();
        if (targetType == moveTemplate.TargetType.Self)
        {
            potentialTargets.Add(selectedMove.character);
        }
        if (targetType == moveTemplate.TargetType.Clip)
        {
            potentialTargets.Add(Clip);
        }
        if (targetType == moveTemplate.TargetType.Partner)
        {
            potentialTargets.Add(Partner);
        }
        if (targetType == moveTemplate.TargetType.Allies)
        {
            potentialTargets.Add(Clip);
            potentialTargets.Add(Partner);
        }
        //Enemy Targets
        if (targetType == moveTemplate.TargetType.Enemies)
        {
            foreach (GameObject enemy in EnemyList)
            {
                potentialTargets.Add(enemy);
            }
        }
        if (targetType == moveTemplate.TargetType.Ground)
        {
            foreach (GameObject enemy in EnemyList)
            {
                FighterClass enemyInfo = enemy.GetComponent<FighterClass>();
                if (enemyInfo.characterPosition == FighterClass.CharacterPosition.Ground)
                {
                    potentialTargets.Add(enemy);
                }
            }
        }
        if (targetType == moveTemplate.TargetType.Flying)
        {
            foreach (GameObject enemy in EnemyList)
            {
                FighterClass enemyInfo = enemy.GetComponent<FighterClass>();
                if (enemyInfo.characterPosition == FighterClass.CharacterPosition.Air)
                {
                    potentialTargets.Add(enemy);
                }
            }
        }
        if (targetType == moveTemplate.TargetType.Submerged)
        {
            foreach (GameObject enemy in EnemyList)
            {
                FighterClass enemyInfo = enemy.GetComponent<FighterClass>();
                if (enemyInfo.characterPosition == FighterClass.CharacterPosition.Water)
                {
                    potentialTargets.Add(enemy);
                }
            }
        }
        //Environment
        if (targetType == moveTemplate.TargetType.Tile)
        {
            Debug.Log("Not Yet Implemented");
        }
        if (targetType == moveTemplate.TargetType.Object)
        {
            Debug.Log("Not Yet Implemented");
        }
        return potentialTargets;
    }

    void UpdateHealth()
    {
        FighterClass ClipStats = Clip.GetComponent<FighterClass>();
        ClipHealth.GetComponent<Text>().text = ClipStats.HP.ToString();
        ClipMaxHealth.GetComponent<Text>().text = ClipStats.HPMax.ToString();
        if (Partner != null)
        {
            FighterClass PartnerStats = Partner.GetComponent<FighterClass>();
            PartnerHealth.GetComponent<Text>().text = PartnerStats.HP.ToString();
            PartnerMaxHealth.GetComponent<Text>().text = PartnerStats.HPMax.ToString();
        }
    }

    public void IDiedBye(GameObject deadBoi)
    {
        if (EnemyList.Contains(deadBoi))
        {
            Vector2 pos = deadBoi.GetComponent<FighterClass>().pos;
            characterGrid[(int)pos.x, (int)pos.y] = null;
            EnemyList.Remove(deadBoi);
            Destroy(deadBoi);
        }
        if (Clip == deadBoi)
        {
            Clip.GetComponent<FighterClass>().Dead = true;
        }
        if (Partner == deadBoi)
        {
            Partner.GetComponent<FighterClass>().Dead = true;
        }
    }
}
