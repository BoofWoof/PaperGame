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
    private Vector2 ClipPos;
    private CutSceneClass ClipMov;
    private Vector2 PartnerPos;
    private CutSceneClass PartnerMov;
    //Enemy Info
    private List<Vector2> EnemyPosList = new List<Vector2>();
    private List<CutSceneClass> EnemyMovList = new List<CutSceneClass>();

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
                blockGrid[row, col].transform.eulerAngles = new Vector3(-90, 0, 0);
                if (_containerCache.characterGrid[row * cols + col] > -1)
                {
                    characterGrid[row, col] = Instantiate(CombatMapper.characterMap[_containerCache.characterGrid[row * cols + col]], blockGrid[row, col].transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity);
                    if (_containerCache.characterGrid[row * cols + col] == 0)
                    {
                        ClipPos = new Vector2(row, col);
                    }
                    else if (_containerCache.characterGrid[row * cols + col] == 1)
                    {
                        PartnerPos = new Vector2(row, col);
                    }
                    else
                    {
                        EnemyPosList.Add(new Vector2(row, col));
                    }
                }
                if (_containerCache.objectGrid[row * cols + col] > -1)
                {
                    objectGrid[row, col] = Instantiate(CombatMapper.objectMap[_containerCache.objectGrid[row * cols + col]], blockGrid[row, col].transform.position + new Vector3(0, 0.6f, 0), Quaternion.identity);
                }
            }
        }
        UpdatePositions();

        //Update Clip And Partner Stats
        FighterClass ClipStats = characterGrid[(int)ClipPos.x, (int)ClipPos.y].GetComponent<FighterClass>();
        ClipStats.HP = GameDataTracker.playerData.health;
        ClipStats.HPMax = GameDataTracker.playerData.maxHealth;
        FighterClass PartnerStats = characterGrid[(int)PartnerPos.x, (int)PartnerPos.y].GetComponent<FighterClass>();
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
            FighterClass ClipStats = characterGrid[(int)ClipPos.x, (int)ClipPos.y].GetComponent<FighterClass>();
            print(ClipStats.Defense);

            UpdateHealth();

            bool cutScenesDone = true;

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
        turnTimeLeft -= Time.deltaTime;
        TimerBar.transform.localScale = new Vector3(turnTimeLeft / turnLength, 1, 1);
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
                    (ClipPos, ClipMov) = MoveCharacter(ClipPos, HorMov, VerMov);
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
                    (PartnerPos, PartnerMov) = MoveCharacter(PartnerPos, HorMov, VerMov);
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

        if (turnTimeLeft < 0 && PartnerMov is null && ClipMov is null)
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
        if (EndPos.x >= 0 && EndPos.x < rows && EndPos.y >= 0 && EndPos.y < cols && characterGrid[(int)EndPos.x, (int)EndPos.y] is null)
        {
            CutSceneClass MoveTo;
            GameObject character = characterGrid[(int)CurrentPos.x, (int)CurrentPos.y];
            characterGrid[(int)CurrentPos.x, (int)CurrentPos.y] = null;
            characterGrid[(int)EndPos.x, (int)EndPos.y] = character;
            if (gridHeight[(int)CurrentPos.x, (int)CurrentPos.y] != gridHeight[(int)EndPos.x, (int)EndPos.y])
            {
                JumpToLocation JumpTo = ScriptableObject.CreateInstance<JumpToLocation>();
                JumpTo.endPosition = new Vector3(EndPos.y * xOffset, gridHeight[(int)EndPos.x, (int)EndPos.y] * zOffset + 0.6f, EndPos.x * yOffset);
                JumpTo.parent = character;
                JumpTo.heightOverHighestCharacter = 1;
                JumpTo.speed = 2f;
                JumpTo.Activate();
                MoveTo = JumpTo;
            } else
            {
                MoveToLocation WalkTo = ScriptableObject.CreateInstance<MoveToLocation>();
                WalkTo.endPosition = new Vector3(EndPos.y * xOffset, gridHeight[(int)EndPos.x, (int)EndPos.y] * zOffset + 0.6f, EndPos.x * yOffset);
                WalkTo.parent = character;
                WalkTo.speed = 5f;
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
            List<GameObject> potentialTargets = new List<GameObject>();
            if (targetType == moveTemplate.TargetType.Self)
            {
                potentialTargets.Add(selectedMove.character);
            }
            if (targetType == moveTemplate.TargetType.Clip)
            {
                potentialTargets.Add(characterGrid[(int)ClipPos.x, (int)ClipPos.y]);
            }
            if (targetType == moveTemplate.TargetType.Partner)
            {
                potentialTargets.Add(characterGrid[(int)PartnerPos.x, (int)PartnerPos.y]);
            }
            if (targetType == moveTemplate.TargetType.Allies)
            {
                potentialTargets.Add(characterGrid[(int)ClipPos.x, (int)ClipPos.y]);
                potentialTargets.Add(characterGrid[(int)PartnerPos.x, (int)PartnerPos.y]);
            }
            //Enemy Targets
            if (targetType == moveTemplate.TargetType.Enemies)
            {
                foreach (Vector2 EnemyPos in EnemyPosList)
                {
                    potentialTargets.Add(characterGrid[(int)EnemyPos.x, (int)EnemyPos.y]);
                }
            }
            if (targetType == moveTemplate.TargetType.Ground)
            {
                foreach (Vector2 EnemyPos in EnemyPosList)
                {
                    GameObject enemy = characterGrid[(int)EnemyPos.x, (int)EnemyPos.y];
                    FighterClass enemyInfo = enemy.GetComponent<FighterClass>();
                    if (enemyInfo.characterPosition == FighterClass.CharacterPosition.Ground)
                    {
                        potentialTargets.Add(enemy);
                    }
                }
            }
            if (targetType == moveTemplate.TargetType.Flying)
            {
                foreach (Vector2 EnemyPos in EnemyPosList)
                {
                    GameObject enemy = characterGrid[(int)EnemyPos.x, (int)EnemyPos.y];
                    FighterClass enemyInfo = enemy.GetComponent<FighterClass>();
                    if (enemyInfo.characterPosition == FighterClass.CharacterPosition.Air)
                    {
                        potentialTargets.Add(enemy);
                    }
                }
            }
            if (targetType == moveTemplate.TargetType.Submerged)
            {
                foreach (Vector2 EnemyPos in EnemyPosList)
                {
                    GameObject enemy = characterGrid[(int)EnemyPos.x, (int)EnemyPos.y];
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
            targeter.potentialTargets = potentialTargets;
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
                }
            }
        }
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
                    character.transform.position = new Vector3(col * xOffset, gridHeight[row, col] * zOffset + 0.6f, row * yOffset);
                }
                GameObject Pobject = objectGrid[row, col];
                if (!(Pobject is null))
                {
                    Pobject.transform.position = new Vector3(col * xOffset, gridHeight[row, col] * zOffset + 0.6f, row * yOffset);
                }
            }
        }
    }

    void UpdateHealth()
    {
        FighterClass ClipStats = characterGrid[(int)ClipPos.x, (int)ClipPos.y].GetComponent<FighterClass>();
        ClipHealth.GetComponent<Text>().text = ClipStats.HP.ToString();
        ClipMaxHealth.GetComponent<Text>().text = ClipStats.HPMax.ToString();
        FighterClass PartnerStats = characterGrid[(int)PartnerPos.x, (int)PartnerPos.y].GetComponent<FighterClass>();
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
            + new Vector3(1f, 6f, -8);
        combatCamera.transform.eulerAngles = new Vector3(30f, 0, 0);
    }
}
