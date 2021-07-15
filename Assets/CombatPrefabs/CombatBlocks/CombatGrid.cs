using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Linq;

public class CombatGrid : GridManager
{
    public int defaultBlock;

    [Header("UI Info")]
    //UI Stuff
    public GameObject tileMenu;
    private int tileType = 1;
    private int selectedCharacter = 0;
    private int selectedObject = 0;
    private string editMode = "Height";

    //Saving
    public GameObject saveNameTextField;

    [Header("SelectionIndicator")]
    public Material rangeIndicator;
    public Material rangeErrorIndicator;
    private Vector2Int selectionRange = new Vector2Int(1, 1);
    private Vector2Int selectionPos = new Vector2Int(0, 0);
    List<GameObject> decalProjectors = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        tileMenu.SetActive(false);

        blockGrid = new GameObject[mapShape.x, mapShape.y];
        characterGrid = new GameObject[mapShape.x, mapShape.y];
        objectGrid = new GameObject[mapShape.x, mapShape.y];
        gridHeight = new int[mapShape.x, mapShape.y];
        for (int x = 0; x < mapShape.x; x++)
        {
            for (int y = 0; y < mapShape.y; y++)
            {
                gridHeight[x, y] = 0;
                CreateObject(blockGrid, new Vector2Int(x, y), CombatMapper.blockMap[defaultBlock], defaultBlock);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (tileMenu.active)
            {
                tileMenu.SetActive(false);
            } else
            {
                tileMenu.SetActive(true);
            }
        }
        if (!tileMenu.active)
        {
            if (Input.GetKey("a"))
            {
                cameraPos.x -= cameraSpeed * Time.deltaTime;
            }
            if (Input.GetKey("d"))
            {
                cameraPos.x += cameraSpeed * Time.deltaTime;
            }
            if (Input.GetKey("s"))
            {
                cameraPos.z -= cameraSpeed * Time.deltaTime;
            }
            if (Input.GetKey("w"))
            {
                cameraPos.z += cameraSpeed * Time.deltaTime;
            }
            if (Input.GetKey("q"))
            {
                cameraPos.y -= cameraSpeed * Time.deltaTime;
            }
            if (Input.GetKey("e"))
            {
                cameraPos.y += cameraSpeed * Time.deltaTime;
            }
            HoverDisplay();
            if (editMode == "Height")
            {
                MoveBlocksHeight();
            }
            if (editMode == "Type")
            {
                ChangeBlock();
            }
            if (editMode == "Character")
            {
                PlaceCharacter();
            }
            if (editMode == "Object")
            {
                PlaceObject();
            }
        }
        combatCamera.transform.position = new Vector3((mapShape.x - 1) * blockOffset.x / 2 + cameraPos.x, cameraHeight + cameraPos.y, cameraOffset + cameraPos.z);
        combatCamera.transform.eulerAngles = new Vector3(cameraAngle, 0, 0);
    }

    public void HoverDisplay()
    {
        Vector2Int grid_pos = BlockAtMouse();
        if (grid_pos != new Vector2Int(-1, -1))
        {
            if (grid_pos != selectionPos)
            {
                BlockRangeDisplay.ClearDisplay(decalProjectors);
                selectionPos = grid_pos;
                if (LevelFloor(grid_pos, selectionRange))
                {
                    decalProjectors = BlockRangeDisplay.RectangleDisplay(rangeIndicator, blockGrid, selectionPos, selectionRange);
                } else
                {
                    decalProjectors = BlockRangeDisplay.RectangleDisplay(rangeErrorIndicator, blockGrid, selectionPos, selectionRange);
                }
            }
        }
    }

    public void SetTileType(int tileTypeInput)
    {
        tileType = tileTypeInput;
        selectionRange = new Vector2Int(1, 1);
    }

    public void SetCharacterIndex(int characterTypeInput)
    {
        selectedCharacter = characterTypeInput;
        selectionRange = CombatMapper.characterMap[selectedCharacter].GetComponent<FighterClass>().TileSize;
    }

    public void SetObjectIndex(int objectTypeInput)
    {
        selectedObject = objectTypeInput;
        selectionRange = new Vector2Int(1, 1);
    }

    public void SetEditMode(string editModeInput)
    {
        if(editModeInput == "Height")
        {
            selectionRange = new Vector2Int(1, 1);
        }
        editMode = editModeInput;
    }

    Vector2Int BlockAtMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            GridObject blockHit = hit.transform.gameObject.GetComponent<GridObject>();
            if (blockHit != null)
            {
                return blockHit.pos;
            }
        }
        return new Vector2Int(-1, -1);
    }

    void PlaceOnGrid(GameObject[,] Grid, GameObject PlaceObject, bool ReplaceExisting, GameObject DeletionReplacement, int objectID)
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Vector2Int grid_pos = BlockAtMouse();
            if(grid_pos != new Vector2Int(-1, -1))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject currentObject = Grid[grid_pos.x, grid_pos.y];
                    if (!(currentObject is null))
                    {
                        if (ReplaceExisting)
                        {
                            Grid[grid_pos.x, grid_pos.y].GetComponent<GridObject>().DestoryObject();
                            CreateObject(Grid, grid_pos, PlaceObject, objectID);
                        }
                    }
                    else
                    {
                        CreateObject(Grid, grid_pos, PlaceObject, objectID);
                    }
                }
                if (Input.GetMouseButtonDown(1))
                {
                    GameObject currentObject = Grid[grid_pos.x, grid_pos.y];
                    if (!(currentObject is null))
                    {
                        Grid[grid_pos.x, grid_pos.y].GetComponent<GridObject>().DestoryObject();
                        Grid[grid_pos.x, grid_pos.y] = null;
                        if (DeletionReplacement != null)
                        {
                            CreateObject(Grid, grid_pos, DeletionReplacement, 0);
                        }
                    }
                }

            }
        }
    }

    void PlaceObject()
    {
        PlaceOnGrid(objectGrid, CombatMapper.objectMap[selectedObject], false, null, selectedObject);
    }

    void PlaceCharacter()
    {
        PlaceOnGrid(characterGrid, CombatMapper.characterMap[selectedCharacter], false, null, selectedCharacter);
    }

    void ChangeBlock()
    {
        PlaceOnGrid(blockGrid, CombatMapper.blockMap[tileType], true, CombatMapper.blockMap[0], tileType);
    }

    void ChangeBlockHeight(Vector2Int grid_pos, int heightChange)
    {
        GameObject block = blockGrid[grid_pos.x, grid_pos.y];
        gridHeight[grid_pos.x, grid_pos.y] += heightChange;
        block.transform.position = GridToPosition(grid_pos, block.GetComponent<GridObject>().TileSize);
        GameObject character = characterGrid[grid_pos.x, grid_pos.y];
        if (!(character is null))
        {
            character.transform.position = block.transform.position;
        }
        GameObject Pobject = objectGrid[grid_pos.x, grid_pos.y];
        if (!(Pobject is null))
        {
            Pobject.transform.position = block.transform.position;
        }
    }

    void MoveBlocksHeight()
    {

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            Vector2Int grid_pos = BlockAtMouse();
            if (grid_pos != new Vector2Int(-1, -1)) { 
                if (Input.GetMouseButtonDown(0))
                {
                    ChangeBlockHeight(grid_pos, 1);
                    for (int y = grid_pos.y + 1; y < mapShape.y; y++)
                    {
                        Vector2Int above_grid_pos = new Vector2Int(grid_pos.x, y);
                        if (gridHeight[grid_pos.x, grid_pos.y] > gridHeight[above_grid_pos.x, above_grid_pos.y])
                        {
                            ChangeBlockHeight(above_grid_pos, 1);
                        }
                    }
                }
                if (Input.GetMouseButtonDown(1))
                {
                    ChangeBlockHeight(grid_pos, -1);
                    for (int y = grid_pos.y - 1; y >= 0; y--)
                    {
                        Vector2Int below_grid_pos = new Vector2Int(grid_pos.x, y);
                        if (gridHeight[grid_pos.x, grid_pos.y] < gridHeight[below_grid_pos.x, below_grid_pos.y])
                        {
                            ChangeBlockHeight(below_grid_pos, -1);
                        }
                    }
                }
            }
        }
    }

    public void SubtractMap(int leftChange, int rightChange, int bottomChange, int topChange)
    {
        //Delete Things Outside of Current Grid
        for(int x = 0; x < mapShape.x; x++)
        {
            for (int y = 0; y < mapShape.y; y++)
            {
                if (!(x >= leftChange && x < mapShape.x - rightChange && y >= bottomChange && y < mapShape.y - topChange))
                {
                    GameObject character = characterGrid[x, y];
                    if (!(character is null))
                    {
                        characterGrid[x,y].GetComponent<GridObject>().DestoryObject();
                    }
                    GameObject Pobject = objectGrid[x, y];
                    if (!(Pobject is null))
                    {
                        objectGrid[x, y].GetComponent<GridObject>().DestoryObject();
                    }
                    blockGrid[x, y].GetComponent<GridObject>().DestoryObject();
                }
            }
        }
        
        //Create new Grids and copy contents over.
        int verChange = topChange + bottomChange;
        int horChange = leftChange + rightChange;
        GameObject[,] newCharacterGrid = new GameObject[mapShape.x - horChange, mapShape.y - verChange];
        GameObject[,] newObjectGrid = new GameObject[mapShape.x - horChange, mapShape.y - verChange];
        GameObject[,] newBlockGrid = new GameObject[mapShape.x - horChange, mapShape.y - verChange];
        int[,] newGridHeight = new int[mapShape.x - horChange, mapShape.y - verChange];
        mapShape -= new Vector2Int(horChange, verChange);
        for (int x = 0; x < mapShape.x; x++)
        {
            for (int y = 0; y < mapShape.y; y++)
            {
                newBlockGrid[x, y] = blockGrid[x + leftChange, y + bottomChange];
                newCharacterGrid[x, y] = characterGrid[x + leftChange, y + bottomChange];
                newObjectGrid[x, y] = objectGrid[x + leftChange, y + bottomChange];
                newGridHeight[x, y] = gridHeight[x + leftChange, y + bottomChange];
            }
        }
        blockGrid = newBlockGrid;
        characterGrid = newCharacterGrid;
        objectGrid = newObjectGrid;
        gridHeight = newGridHeight;
        UpdatePositions();
    }

    public void SubLeft()
    {
        SubtractMap(1, 0, 0, 0);
    }

    public void SubRight()
    {
        SubtractMap(0, 1, 0, 0);
    }

    public void SubBottom()
    {
        SubtractMap(0, 0, 1, 0);
    }

    public void SubTop()
    {
        SubtractMap(0, 0, 0, 1);
    }

    public void AddMap(int leftChange, int rightChange, int bottomChange, int topChange)
    {
        //Create new Grids and copy contents over.
        int verChange = topChange + bottomChange;
        int horChange = leftChange + rightChange;
        GameObject[,] newCharacterGrid = new GameObject[mapShape.x + horChange, mapShape.y + verChange];
        GameObject[,] newObjectGrid = new GameObject[mapShape.x + horChange, mapShape.y + verChange];
        GameObject[,] newBlockGrid = new GameObject[mapShape.x + horChange, mapShape.y + verChange];
        int[,] prevGridHeight = gridHeight;
        gridHeight = new int[mapShape.x + horChange, mapShape.y + verChange];

        for (int x = 0; x < mapShape.x; x++)
        {
            for (int y = 0; y < mapShape.y; y++)
            {
                newBlockGrid[x + leftChange, y + bottomChange] = blockGrid[x, y];
                gridHeight[x + leftChange, y + bottomChange] = prevGridHeight[x, y];
                newCharacterGrid[x + leftChange, y + bottomChange] = characterGrid[x, y];
                newObjectGrid[x + leftChange, y + bottomChange] = objectGrid[x, y];
            }
        }
        mapShape += new Vector2Int(horChange, verChange);

        for (int x = 0; x < leftChange; x++)
        {
            for (int y = bottomChange; y < mapShape.y - topChange; y++)
            {
                gridHeight[x, y] = prevGridHeight[0, y];
                CreateObject(newBlockGrid, new Vector2Int(x, y), CombatMapper.blockMap[defaultBlock], defaultBlock);
            }
        }
        for (int x = mapShape.x - rightChange; x < mapShape.x; x++)
        {
            for (int y = bottomChange; y < mapShape.y - topChange; y++)
            {
                gridHeight[x, y] = prevGridHeight[(mapShape.x - rightChange) - 1, y];
                CreateObject(newBlockGrid, new Vector2Int(x, y), CombatMapper.blockMap[defaultBlock], defaultBlock);
            }
        }
        for (int x = 0; x < mapShape.x; x++)
        {
            for (int y = 0; y < bottomChange; y++)
            {
                gridHeight[x, y] = prevGridHeight[x, 0];
                CreateObject(newBlockGrid, new Vector2Int(x, y), CombatMapper.blockMap[defaultBlock], defaultBlock);
            }
            for (int y = mapShape.y - topChange; y < mapShape.y; y++)
            {
                gridHeight[x, y] = prevGridHeight[x, (mapShape.y - topChange) - 1];
                CreateObject(newBlockGrid, new Vector2Int(x, y), CombatMapper.blockMap[defaultBlock], defaultBlock);
            }
        }

        blockGrid = newBlockGrid;
        characterGrid = newCharacterGrid;
        objectGrid = newObjectGrid;
        UpdatePositions();
    }

    public void AddLeft()
    {
        AddMap(1, 0, 0, 0);
    }

    public void AddRight()
    {
        AddMap(0, 1, 0, 0);
    }

    public void AddBottom()
    {
        AddMap(0, 0, 1, 0);
    }

    public void AddTop()
    {
        AddMap(0, 0, 0, 1);
    }

    public void LoadFromName()
    {
        string filename = saveNameTextField.GetComponent<InputField>().text;
        CombatContainer _containerCache = Resources.Load<CombatContainer>(filename);
        Load(_containerCache);
    }

    public void Save()
    {
        string filename = saveNameTextField.GetComponent<InputField>().text;
        CombatContainer combatContainer = ScriptableObject.CreateInstance<CombatContainer>();

        //Save Grid Height
        combatContainer.gridHeight = new int[mapShape.x * mapShape.y];
        for (int x = 0; x < mapShape.x; x++)
        {
            for (int y = 0; y < mapShape.y; y++)
            {
                combatContainer.gridHeight[x * mapShape.y + y] = gridHeight[x, y];
            }
        }
        combatContainer.blockGrid = SaveHelper(blockGrid);
        combatContainer.characterGrid = SaveHelper(characterGrid);
        combatContainer.objectGrid = SaveHelper(objectGrid);

        //Save Shape
        combatContainer.mapShape = mapShape;

        AssetDatabase.CreateAsset(combatContainer, $"Assets/CombatPrefabs/CombatBlocks/Resources/{filename}.asset");
        AssetDatabase.SaveAssets();
    }

    public int[] SaveHelper(GameObject[,] grid)
    {
        int[] saveGrid = new int[mapShape.x * mapShape.y];
        for (int x = 0; x < mapShape.x; x++)
        {
            for (int y = 0; y < mapShape.y; y++)
            {
                GameObject targetObject = grid[x, y];
                if (targetObject is null)
                {
                    saveGrid[x * mapShape.y + y] = -1;
                }
                else
                {
                    if (isBaseOfObject(grid, new Vector2Int(x, y)))
                    {
                        saveGrid[x * mapShape.y + y] = targetObject.GetComponent<GridObject>().objectID;
                    }
                    else
                    {
                        saveGrid[x * mapShape.y + y] = -1;
                    }
                }
            }
        }
        return saveGrid;
    }
}
