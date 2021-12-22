using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatExecutor : GridManager
{
    GameControls controls;

    [Header("Loading")]
    public CombatContainer _containerCache;
    public CutsceneDeconstruct cutsceneDeconstruct;
    public List<(DialogueContainer, string, GameObject)> DialogueContainerList = new List<(DialogueContainer, string, GameObject)>();

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
    public TurnManager turnManager;
    public TurnManager.turnPhases currentTurn;
    private float turnLength = 10.0f;
    private float turnTimeLeft;
    private float startGameWait = 3.5f;


    private void Awake()
    {
        GameDataTracker.clearCharacterList();
        GameDataTracker.spawnLastTransitionObject();
        GameDataTracker.transitioning = false;
        controls = new GameControls();
    }

    private void OnEnable()
    {
        controls.CombatControls.Enable();
    }

    private void OnDisable()
    {
        controls.CombatControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        OverworldController.CameraHeading = 0;
        if (!(GameDataTracker.combatScene is null))
        {
            _containerCache = GameDataTracker.combatScene;
        }

        AbilityDescription.SetActive(false);
        TimerContainer.SetActive(false);

        GameDataTracker.combatExecutor = this;

        Load(_containerCache);

        //Update Clip And Partner Stats
        UpdateHealth();

        //Set Turn
        turnManager = ScriptableObject.CreateInstance<TurnManager>();
        if(puzzleMode || doublePuzzleMode || turnTie)
        {
            if (turnTie)
            {
                currentTurn = turnManager.TurnTiePuzzle(doublePuzzleMode);
            } else
            {
                currentTurn = turnManager.Puzzle(doublePuzzleMode);
            }
        } else
        {
            currentTurn = turnManager.GoodGuysFirst();
        }
        //Set Camera Position
        SetCameraToWorld();
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueContainerList.Count > 0)
        {
            turnTimeLeft = 0;
        }
        UpdateHealth();
        if (startGameWait > 0)
        {
            startGameWait -= Time.deltaTime;
        } else
        {
            if (CutsceneController.CutscenesPlaying != 0)
            {
                return;
            }
            if (currentTurn == TurnManager.turnPhases.Cutscene)
            {
                Cutscene();
                return;
            }
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

            if (currentTurn == TurnManager.turnPhases.TurnTiePlayerStart)
            {
                TurnTiePlayerStart();
            }
            if (currentTurn == TurnManager.turnPhases.TurnTiePlayerWait)
            {
                TurnTiePlayerWait();
            }
            if (currentTurn == TurnManager.turnPhases.TurnTieEnemyStart)
            {
                TurnTieEnemyStart();
            }
            if (currentTurn == TurnManager.turnPhases.TurnTieEnemyWait)
            {
                TurnTieEnemyWait();
            }
        } 
    }

    public void AddCutsceneToQueue(DialogueContainer newDialogueContainer, string name, GameObject source)
    {
        DialogueContainerList.Add((newDialogueContainer, name, source));
    }

    void Cutscene()
    {
        if (!(cutsceneDeconstruct is null))
        {
            if (cutsceneDeconstruct.CheckIfDone())
            {
                Destroy(cutsceneDeconstruct);
                cutsceneDeconstruct = null;
            }
        } else
        {
            if (DialogueContainerList.Count > 0)
            {
                (DialogueContainer dialogueContainer, string sourceName, GameObject sourceObject) = DialogueContainerList[0];
                DialogueContainerList.RemoveAt(0);
                GameDataTracker.combatExecutor.cutsceneDeconstruct = ScriptableObject.CreateInstance<CutsceneDeconstruct>();
                GameDataTracker.combatExecutor.cutsceneDeconstruct.Deconstruct(dialogueContainer, sourceName, sourceObject);
            }
            else
            {
                currentTurn = turnManager.NextTurn();
            }
        }
    }

    private void TurnTiePlayerStart()
    {
        turnTimeLeft = 1;

        LetClipMove(true);
        if (Clip.GetComponent<FighterClass>().move != null)
        {
            currentTurn = turnManager.NextTurn();
            return;
        }

        LetPartnerMove(true);
        if (Partner != null)
        {
            if (Partner.GetComponent<FighterClass>().move != null)
            {
                currentTurn = turnManager.NextTurn();
                return;
            }
        }
    }

    private void TurnTiePlayerWait()
    {
        turnTimeLeft = 1;
        bool allAlliesMoveDone = true;

        if (Clip.GetComponent<FighterClass>().move != null)
        {
            allAlliesMoveDone = false;
            updateMove(Clip.GetComponent<FighterClass>());
        }
        if (Partner != null)
        {
            if (Partner.GetComponent<FighterClass>().move != null)
            {
                allAlliesMoveDone = false;
                updateMove(Partner.GetComponent<FighterClass>());
            }
        }

        if (allAlliesMoveDone) currentTurn = turnManager.NextTurn();
    }

    private void TurnTieEnemyStart()
    {
        turnTimeLeft = 1;
        for (int enemyIdx = 0; enemyIdx < EnemyList.Count; enemyIdx++)
        {
            FighterClass enemy = EnemyList[enemyIdx].GetComponent<FighterClass>();
            if (enemy.move is null)
            {
                if (turnTimeLeft > 0 && !enemy.Paralyzed)
                {
                    enemy.GetRoute(this);
                }
            }
        }
        currentTurn = turnManager.NextTurn();
    }

    private void TurnTieEnemyWait()
    {
        turnTimeLeft = 1;
        bool allEnemyMoveDone = true;
        for (int enemyIdx = 0; enemyIdx < EnemyList.Count; enemyIdx++)
        {
            FighterClass enemy = EnemyList[enemyIdx].GetComponent<FighterClass>();
            if (enemy.move != null)
            {
                updateMove(enemy);
                allEnemyMoveDone = false;
            }
        }
        if(allEnemyMoveDone) currentTurn = turnManager.NextTurn();
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

        LetClipMove(allEnemyMoveDone);
        LetPartnerMove(allEnemyMoveDone);

        currentTurn = turnManager.NextTurn();
        /*if ((turnTimeLeft <= 0 && Partner.GetComponent<FighterClass>().move is null &&
            Clip.GetComponent<FighterClass>().move is null && allEnemyMoveDone) ||
            EnemyList.Count == 0)
        {
            currentTurn = turnManager.NextTurn();
        }*/
    }

    private void LetClipMove(bool allEnemyMoveDone)
    {
        int HorMov = 0;
        int VerMov = 0;
        if (Clip.GetComponent<FighterClass>().move is null)
        {
            FighterClass clipInfo = Clip.GetComponent<FighterClass>();
            clipInfo.animator.speed = 1.0f;
            if ((turnTimeLeft > 0 || !allEnemyMoveDone) && !clipInfo.Paralyzed && !clipInfo.Dead)
            {
                Vector2 thumbstick_values = controls.CombatControls.Movement_Clip.ReadValue<Vector2>();
                float moveHorizontal = thumbstick_values[0];
                float moveVertical = thumbstick_values[1];
                if (moveHorizontal > 0.3)
                {
                    HorMov = 1;
                }
                else if (moveHorizontal < -0.3)
                {
                    HorMov = -1;
                }
                else if (moveVertical > 0.3)
                {
                    VerMov = 1;
                }
                else if (moveVertical < -0.3)
                {
                    VerMov = -1;
                }
                if (HorMov != 0 || VerMov != 0) clipInfo.MoveCharacter(HorMov, VerMov);
            }
        }
        else
        {
            updateMove(Clip.GetComponent<FighterClass>());
        }
    }

    private void LetPartnerMove(bool allEnemyMoveDone)
    {
        int HorMov = 0;
        int VerMov = 0;
        if (Partner != null)
        {
            if (Partner.GetComponent<FighterClass>().move is null)
            {
                FighterClass partnerInfo = Partner.GetComponent<FighterClass>();
                if ((turnTimeLeft > 0 || !allEnemyMoveDone) && !partnerInfo.Paralyzed && !partnerInfo.Dead)
                {
                    Vector2 thumbstick_values = controls.CombatControls.Movement_Partner.ReadValue<Vector2>();
                    float moveHorizontal = thumbstick_values[0];
                    float moveVertical = thumbstick_values[1];
                    if (moveHorizontal > 0.3)
                    {
                        HorMov = 1;
                    }
                    else if (moveHorizontal < -0.3)
                    {
                        HorMov = -1;
                    }
                    else if (moveVertical > 0.3)
                    {
                        VerMov = 1;
                    }
                    else if (moveVertical < -0.3)
                    {
                        VerMov = -1;
                    }
                    if (HorMov != 0 || VerMov != 0) partnerInfo.MoveCharacter(HorMov, VerMov);
                }
            }
            else
            {
                updateMove(Partner.GetComponent<FighterClass>());
            }
        }
    }

    void PlayerTurnStart(GameObject player)
    {
        if (player == null)
        {
            currentTurn = turnManager.NextTurn();
            return;
        }
        FighterClass playerInfo = player.GetComponent<FighterClass>();
        FocusOnCharacter(playerInfo.pos);
        playerInfo.GetComponent<FighterClass>().TurnStart();
        currentTurn = turnManager.NextTurn();
        if(playerInfo.Dead == true)
        {
            currentTurn = turnManager.NextTurn();
        }
    }

    void PlayerTurn(GameObject player)
    {
        if (player == null)
        {
            currentTurn = turnManager.NextTurn();
            return;
        }
        FighterClass stats = player.GetComponent<FighterClass>();
        FocusOnCharacter(stats.pos);
        //If no option menu create one.
        if (currentMenu.Count == 0)
        {
            movesetContainer moves = player.GetComponent<movesetContainer>();

            BattleMenu menu = ScriptableObject.CreateInstance<BattleMenu>();
            menu.characterTarget = player;
            menu.characterHeight = stats.CharacterHeight;
            menu.characterWidth = stats.CharacterWidth;
            menu.movesList = moves.moves;
            menu.spriteList = moves.sprites;
            AddMenu(menu);
        } else
        {
            //Move back from a submenu.
            BattleMenu selectedMenu = currentMenu[currentMenu.Count - 1];
            if (controls.CombatControls.SecondaryAction.triggered && currentMenu.Count > 1)
            {
                selectedMenu.Deactivate();
                currentMenu.Remove(selectedMenu);
                selectedMenu = currentMenu[currentMenu.Count - 1];
                selectedMenu.Activate();
            }
            Vector2 thumbstick_values = controls.CombatControls.Movement_Clip.ReadValue<Vector2>();
            float moveHorizontal = thumbstick_values[0];

            //Check if an action was selected.
            GameObject selectedObject = selectedMenu.UpdateMenu(moveHorizontal, controls.CombatControls.MainAction.triggered);
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

    void PlayerTurnEnd(GameObject player)
    {
        if (player == null)
        {
            currentTurn = turnManager.NextTurn();
            return;
        }
        FighterClass playerInfo = player.GetComponent<FighterClass>();
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
        } else
        {
            turnTimeLeft = 0;
        }
        TimerBar.transform.localScale = new Vector3(turnTimeLeft / turnLength, 1, 1);

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

        LetClipMove(allEnemyMoveDone);
        LetPartnerMove(allEnemyMoveDone);

        bool partnerReady = false;
        if (Partner == null) partnerReady = true;
        else partnerReady = Partner.GetComponent<FighterClass>().move is null;
        if ((turnTimeLeft <= 0 && partnerReady && 
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
            targeter.targeterSprite = selectedMove.character.GetComponent<PlayerFighter>().characterSelector;
            targeter.targeterSpriteFloor = selectedMove.character.GetComponent<PlayerFighter>().characterSelectorFloor;
            targeter.targeterSpriteMaterial = selectedMove.character.GetComponent<PlayerFighter>().characterSelectorMaterial;
            targeter.Activate();
        }
        else
        {
            if (controls.CombatControls.SecondaryAction.triggered && currentMenu.Count > 0)
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
                Vector2 thumbstick_values = controls.CombatControls.Movement_Clip.ReadValue<Vector2>();
                float moveHorizontal = thumbstick_values[0];
                float moveVertical = thumbstick_values[1];
                List<GameObject> moveTargets = targeter.TargeterUpdate(moveHorizontal, moveVertical, controls.CombatControls.MainAction.triggered);
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
            if (Partner != null) potentialTargets.Add(Partner);
        }
        if (targetType == moveTemplate.TargetType.Allies)
        {
            potentialTargets.Add(Clip);
            if (Partner != null) potentialTargets.Add(Partner);
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
        if (Partner != null)
        {
            if (Partner == deadBoi)
            {
                Partner.GetComponent<FighterClass>().Dead = true;
            }
        }
    }
}
