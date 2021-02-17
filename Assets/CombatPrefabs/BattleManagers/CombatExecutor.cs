using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatExecutor : MonoBehaviour
{
    private int rows;
    private int cols;

    [Header("Block Spacing")]
    public float xOffset;
    public float yOffset;
    public float zOffset;

    [Header("Camera Variables")]
    public GameObject combatCamera;
    public float cameraHeight;
    public float cameraAngle;
    public float cameraOffset;
    public float cameraSpeed;
    private float cameraX = 0;
    private float cameraY = 0;
    private float cameraZ = 0;

    private GameObject[,] blockGrid;
    private GameObject[,] characterGrid;
    private GameObject[,] objectGrid;
    private int[,] gridHeight;

    [Header("Loading")]
    public CombatContainer _containerCache;

    //Ally Info
    private GameObject Clip;
    private CutSceneClass ClipMov;
    private GameObject Partner;
    private CutSceneClass PartnerMov;
    //Enemy Info
    private List<GameObject> EnemyList = new List<GameObject>();

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
    private float startGameWait = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        AbilityDescription.SetActive(false);
        TimerContainer.SetActive(false);

        GameDataTracker.combatExecutor = this;
        rows = _containerCache.rows;
        cols = _containerCache.cols;

        gridHeight = new int[rows, cols];
        blockGrid = new GameObject[rows, cols];
        characterGrid = new GameObject[rows, cols];
        objectGrid = new GameObject[rows, cols];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                gridHeight[row, col] = _containerCache.gridHeight[row * cols + col];
                blockGrid[row, col] = Instantiate(CombatMapper.blockMap[_containerCache.blockGrid[row * cols + col]], new Vector3(col * xOffset, gridHeight[row, col] * zOffset, row * yOffset), Quaternion.identity);
                //blockGrid[row, col].transform.localScale = new Vector3(30, 30, 30);
                //blockGrid[row, col].transform.eulerAngles = new Vector3(-90, 0, 0);
                if (_containerCache.characterGrid[row * cols + col] > -1)
                {
                    characterGrid[row, col] = Instantiate(CombatMapper.characterMap[_containerCache.characterGrid[row * cols + col]], blockGrid[row, col].transform.position + new Vector3(0, 0, 0), Quaternion.identity);
                    if (_containerCache.characterGrid[row * cols + col] == 0)
                    {
                        Clip = characterGrid[row, col];
                        Clip.GetComponent<FighterClass>().pos = new Vector2(row, col);
                    }
                    else if (_containerCache.characterGrid[row * cols + col] == 1)
                    {
                        Partner = characterGrid[row, col];
                        Partner.GetComponent<FighterClass>().pos = new Vector2(row, col);
                    }
                    else
                    {
                        characterGrid[row, col].GetComponent<FighterClass>().pos = new Vector2(row, col);
                        EnemyList.Add(characterGrid[row, col]);
                    }
                }
                if (_containerCache.objectGrid[row * cols + col] > -1)
                {
                    objectGrid[row, col] = Instantiate(CombatMapper.objectMap[_containerCache.objectGrid[row * cols + col]], blockGrid[row, col].transform.position + new Vector3(0, 0, 0), Quaternion.identity);
                }
            }
        }
        UpdatePositions();

        //Update Clip And Partner Stats
        FighterClass ClipStats = Clip.GetComponent<FighterClass>();
        ClipStats.HP = GameDataTracker.playerData.health;
        ClipStats.HPMax = GameDataTracker.playerData.maxHealth;
        FighterClass PartnerStats = Partner.GetComponent<FighterClass>();
        PartnerStats.HP = GameDataTracker.playerData.FaeHealth;
        PartnerStats.HPMax = GameDataTracker.playerData.CompanionMaxHealth;

        //Set Turn
        turnManager = new TurnManager();
        currentTurn = turnManager.GoodGuysFirst();

        //Set Camera Position
        SetCameraToWorld();
    }

    // Update is called once per frame
    void Update()
    {
        if (startGameWait > 0)
        {
            startGameWait -= Time.deltaTime;
        } else
        {
            FighterClass ClipStats = Clip.GetComponent<FighterClass>();
            //print(ClipStats.Defense);

            UpdateHealth();

            bool cutScenesDone = true;

            Vector2 ClipPos = Clip.GetComponent<FighterClass>().pos;
            Vector2 PartnerPos = Partner.GetComponent<FighterClass>().pos;

            if (cutScenesDone)
            {
                if (currentTurn == TurnManager.turnPhases.ClipTurnStart)
                {
                    PlayerTurnStart(ClipPos);
                }
                if (currentTurn == TurnManager.turnPhases.ClipTurn)
                {
                    if (selectedMove is null)
                    {
                        PlayerTurn(ClipPos);
                    }
                    else
                    {
                        Targeting();
                    }
                }
                if (currentTurn == TurnManager.turnPhases.ClipTurnEnd)
                {
                    PlayerTurnEnd(ClipPos);
                }
                if (currentTurn == TurnManager.turnPhases.PartnerTurnStart)
                {
                    PlayerTurnStart(PartnerPos);
                }
                if (currentTurn == TurnManager.turnPhases.PartnerTurn)
                {
                    if (selectedMove is null)
                    {
                        PlayerTurn(PartnerPos);
                    }
                    else
                    {
                        Targeting();
                    }
                }
                if (currentTurn == TurnManager.turnPhases.PartnerTurnEnd)
                {
                    PlayerTurnEnd(PartnerPos);
                }
                if (currentTurn == TurnManager.turnPhases.EnemyTurnStart)
                {
                    EnemyTurnStart();
                }
                if (currentTurn == TurnManager.turnPhases.EnemyTurn)
                {
                    EnemyTurn();
                }
                if (currentTurn == TurnManager.turnPhases.EnemyTurnEnd)
                {
                    EnemyTurnEnd();
                }
            }
        }
    }

    void PlayerTurnStart(Vector2 PlayerPos)
    {
        FocusOnCharacter(PlayerPos);
        characterGrid[(int)PlayerPos.x, (int)PlayerPos.y].GetComponent<FighterClass>().TurnStart();
        currentTurn = turnManager.NextTurn();
    }

    void PlayerTurn(Vector2 PlayerPos)
    {
        FocusOnCharacter(PlayerPos);
        GameObject character = characterGrid[(int)PlayerPos.x, (int)PlayerPos.y];
        //If no option menu create one.
        if (currentMenu.Count == 0)
        {
            movesetContainer moves = character.GetComponent<movesetContainer>();
            FighterClass stats = character.GetComponent<FighterClass>();

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
                selectedMove.character = character;
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

    void PlayerTurnEnd(Vector2 PlayerPos)
    {
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
        int HorMov = 0;
        int VerMov = 0;
        if (ClipMov is null)
        {
            if (turnTimeLeft > 0)
            {
                if (Input.GetButton("Fire2"))
                {
                    if (Input.GetAxis("Horizontal") > 0.3)
                    {
                        HorMov = 1;
                    }
                    if (Input.GetAxis("Horizontal") < -0.3)
                    {
                        HorMov = -1;
                    }
                    if (Input.GetAxis("Vertical") > 0.3)
                    {
                        VerMov = 1;
                    }
                    if (Input.GetAxis("Vertical") < -0.3)
                    {
                        VerMov = -1;
                    }
                    (Clip.GetComponent<FighterClass>().pos, ClipMov) = MoveCharacter(Clip.GetComponent<FighterClass>().pos, HorMov, VerMov);
                }
            }
        }
        else
        {
            if (ClipMov.Update())
            {
                Destroy(ClipMov);
                ClipMov = null;
            }
        }
        if (PartnerMov is null)
        {
            if (turnTimeLeft > 0)
            {
                if (Input.GetButton("Fire3"))
                {
                    if (Input.GetAxis("Horizontal") > 0.3)
                    {
                        HorMov = 1;
                    }
                    if (Input.GetAxis("Horizontal") < -0.3)
                    {
                        HorMov = -1;
                    }
                    if (Input.GetAxis("Vertical") > 0.3)
                    {
                        VerMov = 1;
                    }
                    if (Input.GetAxis("Vertical") < -0.3)
                    {
                        VerMov = -1;
                    }
                    (Partner.GetComponent<FighterClass>().pos, PartnerMov) = MoveCharacter(Partner.GetComponent<FighterClass>().pos, HorMov, VerMov);
                }

            }
        }
        else
        {
            if (PartnerMov.Update())
            {
                Destroy(PartnerMov);
                PartnerMov = null;
            }
        }

        bool allEnemyMoveDone = true;
        for (int enemyIdx = 0; enemyIdx < EnemyList.Count; enemyIdx++)
        {
            Vector2 enemyPos = EnemyList[enemyIdx].GetComponent<FighterClass>().pos;
            FighterClass enemy = characterGrid[(int)enemyPos.x, (int)enemyPos.y].GetComponent<FighterClass>();
            if (enemy.move is null)
            {
                if (turnTimeLeft > 0)
                {
                    allEnemyMoveDone = false;
                    AStar router = ScriptableObject.CreateInstance<AStar>();
                    List<Vector2> goalPos = new List<Vector2>();
                    //Get Move
                    moveTemplate move = enemy.GetMove();
                    //Get Goals
                    goalPos = move.findGoals(getTargets(move.targetType), rows, cols);
                    (Vector2 newPos, FighterClass.CharacterPosition moveType) = router.GetNextTile(
                        enemy,
                        blockGrid,
                        characterGrid,
                        objectGrid,
                        gridHeight,
                        enemyPos,
                        goalPos
                        );
                    /*
                    if (Input.GetButton("Fire1"))
                    {
                        router.Debug(
                            enemy,
                            blockGrid,
                            characterGrid,
                            objectGrid,
                            gridHeight,
                            enemyPos,
                            goalPos
                            );
                    }
                    */
                    (EnemyList[enemyIdx].GetComponent<FighterClass>().pos, enemy.move) = MoveCharacter(enemyPos, (int)(newPos.y - enemyPos.y), (int)(newPos.x - enemyPos.x));
                }
            }
            else
            {
                allEnemyMoveDone = false;
                if (enemy.move.Update())
                {
                    Destroy(enemy.move);
                    enemy.move = null;
                }
            }
        }

        if (turnTimeLeft <= 0 && PartnerMov is null && ClipMov is null && allEnemyMoveDone)
        {
            currentTurn = turnManager.NextTurn();
        }
    }

    void EnemyTurnEnd()
    {
        TimerContainer.SetActive(false);
        currentTurn = turnManager.NextTurn();
    }

    (Vector2, CutSceneClass) MoveCharacter(Vector2 CurrentPos, int HorChange, int VerChange)
    {
        Vector2 EndPos = new Vector2(CurrentPos.x + VerChange, CurrentPos.y + HorChange);
        GameObject character = characterGrid[(int)CurrentPos.x, (int)CurrentPos.y];
        FighterClass stats = character.GetComponent<FighterClass>();
        if (EndPos.x >= 0 && EndPos.x < rows && EndPos.y >= 0 && EndPos.y < cols && characterGrid[(int)EndPos.x, (int)EndPos.y] is null &&
            (gridHeight[(int)EndPos.x, (int)EndPos.y] - gridHeight[(int)CurrentPos.x, (int)CurrentPos.y] <= stats.MaxJumpHeight) &&
            blockGrid[(int)EndPos.x, (int)EndPos.y].GetComponent<BlockTemplate>().Walkable)
        {
            CutSceneClass MoveTo;
            characterGrid[(int)CurrentPos.x, (int)CurrentPos.y] = null;
            characterGrid[(int)EndPos.x, (int)EndPos.y] = character;
            if (gridHeight[(int)CurrentPos.x, (int)CurrentPos.y] != gridHeight[(int)EndPos.x, (int)EndPos.y])
            {
                JumpToLocation JumpTo = ScriptableObject.CreateInstance<JumpToLocation>();
                JumpTo.endPosition = new Vector3(EndPos.y * xOffset, gridHeight[(int)EndPos.x, (int)EndPos.y] * zOffset + 0, EndPos.x * yOffset);
                JumpTo.parent = character;
                JumpTo.heightOverHighestCharacter = 1;
                JumpTo.speed = stats.JumpSpeed;
                JumpTo.Activate();
                MoveTo = JumpTo;
            }
            else
            {
                MoveToLocation WalkTo = ScriptableObject.CreateInstance<MoveToLocation>();
                WalkTo.endPosition = new Vector3(EndPos.y * xOffset, gridHeight[(int)EndPos.x, (int)EndPos.y] * zOffset + 0, EndPos.x * yOffset);
                WalkTo.parent = character;
                WalkTo.speed = stats.WalkSpeed;
                WalkTo.Activate();
                MoveTo = WalkTo;

            }
            return (EndPos, MoveTo);
        } else
        {
            return (CurrentPos, null);
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
            targeter.potentialTargets = getTargets(targetType);
            targeter.targetQuantity = selectedMove.targetQuantity;
            targeter.targeterSprite = selectedMove.character.GetComponent<FighterClass>().characterSelector;
            targeter.Activate();
        }
        else
        {
            if (Input.GetButtonDown("Fire3") && currentMenu.Count > 0)
            {
                if (targeter.Undo())
                {
                    selectedMove = null;
                    targeter = null;
                    BattleMenu selectedMenu = currentMenu[currentMenu.Count - 1];
                    selectedMenu.Activate();
                    AbilityDescription.SetActive(false);
                }
            }
            else
            {
                List<GameObject> moveTargets = targeter.TargeterUpdate(Input.GetAxis("Horizontal"), Input.GetButtonDown("Fire2"));
                if (!(moveTargets is null))
                {
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

    private List<GameObject> getTargets(moveTemplate.TargetType targetType)
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

    private void UpdatePositions()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                blockGrid[row, col].transform.position = new Vector3(col * xOffset, gridHeight[row, col] * zOffset, row * yOffset);
                GameObject character = characterGrid[row, col];
                if (!(character is null))
                {
                    character.transform.position = new Vector3(col * xOffset, gridHeight[row, col] * zOffset + 0, row * yOffset);
                }
                GameObject Pobject = objectGrid[row, col];
                if (!(Pobject is null))
                {
                    Pobject.transform.position = new Vector3(col * xOffset, gridHeight[row, col] * zOffset + 0, row * yOffset);
                }
            }
        }
    }

    void UpdateHealth()
    {
        FighterClass ClipStats = Clip.GetComponent<FighterClass>();
        ClipHealth.GetComponent<Text>().text = ClipStats.HP.ToString();
        ClipMaxHealth.GetComponent<Text>().text = ClipStats.HPMax.ToString();
        FighterClass PartnerStats = Partner.GetComponent<FighterClass>();
        PartnerHealth.GetComponent<Text>().text = PartnerStats.HP.ToString();
        PartnerMaxHealth.GetComponent<Text>().text = PartnerStats.HPMax.ToString();
    }

    void SetCameraToWorld()
    {
        combatCamera.transform.position = new Vector3((cols - 1) * xOffset / 2 + cameraX, cameraHeight + cameraY, cameraOffset + cameraZ);
        combatCamera.transform.eulerAngles = new Vector3(cameraAngle, 0, 0);
    }

    void FocusOnCharacter(Vector2 PlayerPos)
    {
        combatCamera.transform.position = characterGrid[(int)PlayerPos.x, (int)PlayerPos.y].transform.position
            + new Vector3(1f, 5.8f, -8);
        combatCamera.transform.eulerAngles = new Vector3(30f, 0, 0);
    }
}
