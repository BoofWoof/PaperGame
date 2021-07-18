using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [Header("Shape")]
    public static Vector2Int mapShape = new Vector2Int(7, 5);
    public static Vector3 blockOffset = new Vector3(1.2f, 1.2f, 0.4f);

    [Header("Camera Info")]
    public GameObject combatCamera;
    public float cameraHeight;
    public float cameraAngle;
    public float cameraOffset;
    public float cameraSpeed;
    public Vector3 cameraPos = new Vector3(0, 0, 0);

    //Ally Info
    [HideInInspector] public GameObject Clip;
    [HideInInspector] public GameObject Partner;

    //Enemy Info
    [HideInInspector] public List<GameObject> EnemyList = new List<GameObject>();

    public static GameObject[,] blockGrid;
    public static GameObject[,] characterGrid;
    public static GameObject[,] objectGrid;
    public static int[,] gridHeight;
    public float maxBlockHeight = -100;
    public static List<GoalBlock> goalBlockList;

    [Header("Puzzle Info")]
    public bool puzzleMode = false;
    public bool doublePuzzleMode = false;
    public bool turnTie = false;

    static public Vector3 GridToPosition(Vector2Int gridPos, Vector2 gridSize)
    {
        Vector3 bottomLeftPos = new Vector3(gridPos.x * blockOffset.x, gridHeight[gridPos.x, gridPos.y] * blockOffset.z, gridPos.y * blockOffset.y);
        Vector3 topRightPos = new Vector3((gridPos.x + gridSize.x - 1) * blockOffset.x, gridHeight[gridPos.x, gridPos.y] * blockOffset.z, (gridPos.y + gridSize.y - 1) * blockOffset.y);
        return (bottomLeftPos + topRightPos)/2 + new Vector3(blockOffset.x / 2, 0, blockOffset.y / 2);
    }

    public void CreateObject(GameObject[,] grid, Vector2Int grid_pos, GameObject target_object, int objectID)
    {
        if (grid == characterGrid)
        {
            if (objectID == 0)
            {
                if (Clip != null)
                {
                    return;
                }
            }
            else if (objectID <= 10)
            {
                if (Partner != null)
                {
                    return;
                }
            }
        }
        GameObject newObject = Instantiate(target_object, new Vector3(0, 0, 0), Quaternion.identity);
        GridObject gridClass = newObject.GetComponent<GridObject>();
        if (!LevelFloor(grid_pos, gridClass.TileSize) && grid != blockGrid)
        {
            Destroy(newObject);
            return;
        }
        newObject.transform.position = GridToPosition(grid_pos, gridClass.TileSize);
        grid[grid_pos.x, grid_pos.y] = newObject;
        gridClass.pos = grid_pos;
        gridClass.objectID = objectID;
        gridClass.ContainingGrid = grid;
        gridClass.AddObjectToGrid(grid_pos);
        if (gridClass.name == "GoalBlock")
        {
            Debug.Log("BlockAddedToList");
            goalBlockList.Add(newObject.GetComponent<GoalBlock>());
        }
        SpriteFlipper flipper = newObject.GetComponent<SpriteFlipper>();
        if(flipper != null)
        {
            if(grid_pos.x <= mapShape.x/2)
            {
                flipper.setFacingRightInstant();
            } else
            {
                flipper.setFacingLeftInstant();
            }
        }
        if (grid == characterGrid)
        {
            if (objectID == 0)
            {
                if (Clip == null)
                {
                    Clip = newObject;
                }
            }
            else if (objectID <= 10)
            {
                if (Partner == null)
                {
                    Partner = newObject;
                }
            }
            else
            {
                EnemyList.Add(newObject);
            }
        }
    }

    public static bool LevelFloor(Vector2Int grid_pos, Vector2Int tile_size)
    {
        int compareHeight = gridHeight[grid_pos.x, grid_pos.y];
        for(int x = 0; x < tile_size.x; x++)
        {
            for (int y = 0; y < tile_size.y; y++)
            {
                if (grid_pos.x + x >= mapShape.x || grid_pos.y + y >= mapShape.y)
                {
                    return false;
                }
                if (compareHeight != gridHeight[grid_pos.x + x, grid_pos.y + y])
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void Clear()
    {
        for (int x = 0; x < mapShape.x; x++)
        {
            for (int y = 0; y < mapShape.y; y++)
            {
                if(blockGrid[x,y] != null)
                {
                    blockGrid[x, y].GetComponent<GridObject>().DestoryObject();
                }
                if(characterGrid[x,y] != null)
                {
                    characterGrid[x, y].GetComponent<GridObject>().DestoryObject();
                }
                if (objectGrid[x, y] != null)
                {
                    objectGrid[x, y].GetComponent<GridObject>().DestoryObject();
                }
            }
        }
    }

    public void Load(CombatContainer _containerCache)
    {
        goalBlockList = new List<GoalBlock>();
        mapShape = _containerCache.mapShape;
        gridHeight = new int[mapShape.x, mapShape.y];
        blockGrid = new GameObject[mapShape.x, mapShape.y];
        characterGrid = new GameObject[mapShape.x, mapShape.y];
        objectGrid = new GameObject[mapShape.x, mapShape.y];
        for (int x = 0; x < mapShape.x; x++)
        {
            for (int y = 0; y < mapShape.y; y++)
            {
                gridHeight[x, y] = _containerCache.gridHeight[x * mapShape.y + y];
                if (gridHeight[x, y] > maxBlockHeight) maxBlockHeight = gridHeight[x, y];
                if (_containerCache.blockGrid[x * mapShape.y + y] > -1)
                {
                    CreateObject(blockGrid, new Vector2Int(x, y), CombatMapper.blockMap[_containerCache.blockGrid[x * mapShape.y + y]], _containerCache.blockGrid[x * mapShape.y + y]);
                }
            }
        }
        for (int x = 0; x < mapShape.x; x++)
        {
            for (int y = 0; y < mapShape.y; y++)
            {
                if (_containerCache.characterGrid[x * mapShape.y + y] > -1)
                {
                    CreateObject(characterGrid, new Vector2Int(x, y), CombatMapper.characterMap[_containerCache.characterGrid[x * mapShape.y + y]], _containerCache.characterGrid[x * mapShape.y + y]);
                }
                if (_containerCache.objectGrid[x * mapShape.y + y] > -1)
                {
                    CreateObject(objectGrid, new Vector2Int(x, y), CombatMapper.objectMap[_containerCache.objectGrid[x * mapShape.y + y]], _containerCache.objectGrid[x * mapShape.y + y]);
                }
            }
        }
        puzzleMode = _containerCache.puzzleMode;
        doublePuzzleMode = _containerCache.doublePuzzleMode;
        turnTie = _containerCache.turnTie;
        UpdatePositions();
    }

    public void UpdatePositions()
    {
        for (int x = 0; x < mapShape.x; x++)
        {
            for (int y = 0; y < mapShape.y; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);
                GameObject block = blockGrid[x, y];
                if (block == null)
                {
                    CreateObject(blockGrid, pos, CombatMapper.blockMap[0], 0);
                } else
                {
                    GridObject blockInfo = block.GetComponent<GridObject>();
                    blockInfo.ContainingGrid = blockGrid;
                    blockInfo.AddObjectToGrid(new Vector2Int(x, y));
                    UpdatePositionHelper(blockGrid, pos);
                    block.GetComponent<BlockTemplate>().finalPosition = block.transform.position;
                }

                GameObject character = characterGrid[x, y];
                if (!(character is null))
                {
                    GridObject characterInfo = character.GetComponent<GridObject>();
                    characterInfo.ContainingGrid = characterGrid;
                    characterInfo.AddObjectToGrid(new Vector2Int(x, y));
                    UpdatePositionHelper(characterGrid, pos);
                } 

                GameObject Pobject = objectGrid[x, y];
                if (!(Pobject is null))
                {
                    GridObject PobjectInfo = Pobject.GetComponent<GridObject>();
                    PobjectInfo.ContainingGrid = objectGrid;
                    PobjectInfo.AddObjectToGrid(new Vector2Int(x, y));
                    UpdatePositionHelper(objectGrid, pos);
                }
            }
        }
    }

    private void UpdatePositionHelper(GameObject[,] grid, Vector2Int pos)
    {
        GameObject targetObject = grid[pos.x, pos.y];
        if (isBaseOfObject(grid, pos))
        {
            targetObject.transform.position = GridToPosition(pos, targetObject.GetComponent<GridObject>().TileSize);
            targetObject.GetComponent<GridObject>().pos = pos;
            targetObject.GetComponent<GridObject>().extra_pos = UpdateExtraPos(pos, targetObject.GetComponent<GridObject>().TileSize);
        }
    }

    public List<Vector2Int> UpdateExtraPos(Vector2Int grid_pos, Vector2Int tile_size){
        List<Vector2Int> extra_pos = new List<Vector2Int>();
        for (int x = 0; x < tile_size.x; x++)
        {
            for (int y = 0; y < tile_size.y; y++)
            {
                if (!(x == 0 && y == 0))
                {
                    extra_pos.Add(new Vector2Int(grid_pos.x + x, grid_pos.y + y));
                }
            }
        }
        return extra_pos;
    }

    public bool isBaseOfObject(GameObject[,] grid, Vector2Int grid_pos)
    {
        if (grid_pos.x >= 1)
        {
            if (grid[grid_pos.x - 1, grid_pos.y] == grid[grid_pos.x, grid_pos.y])
            {
                return false;
            }
        }
        if (grid_pos.y >= 1)
        {
            if (grid[grid_pos.x, grid_pos.y - 1] == grid[grid_pos.x, grid_pos.y])
            {
                return false;
            }
        }
        return true;
    }

    public void SetCameraToWorld()
    {
        float verticalFOV = combatCamera.GetComponent<Camera>().fieldOfView;
        float horizontalFOV = Camera.VerticalToHorizontalFieldOfView(verticalFOV, (Screen.width * 1f) / (Screen.height * 1f));
        combatCamera.transform.eulerAngles = new Vector3(cameraAngle, 0, 0);
        CameraMath.GetCameraPosition(mapShape, blockOffset, maxBlockHeight * blockOffset.z, cameraAngle, combatCamera.GetComponent<Camera>());
        //combatCamera.transform.position = new Vector3(mapShape.x * blockOffset.x / 2 + cameraPos.x, cameraHeight * mapShape.y / 10f + cameraPos.y + 5.5f, cameraOffset * mapShape.x / 10f + cameraPos.z - -1f);
    }

    public void FocusOnCharacter(Vector2 PlayerPos)
    {
        combatCamera.transform.position = characterGrid[(int)PlayerPos.x, (int)PlayerPos.y].transform.position
            + new Vector3(1f, 5.8f, -8);
        combatCamera.transform.eulerAngles = new Vector3(30f, 0, 0);
    }
}
