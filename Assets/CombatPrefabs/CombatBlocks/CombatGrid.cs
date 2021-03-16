using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Linq;

public class CombatGrid : MonoBehaviour
{
    public int defaultBlock;

    public int rows;
    public int cols;

    public float xOffset;
    public float yOffset;
    public float zOffset;

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

    //UI Stuff
    public GameObject tileMenu;
    private int tileType = 1;
    private int selectedCharacter = 0;
    private int selectedObject = 0;
    private string editMode = "Height";

    //Saving
    public GameObject saveNameTextField;

    // Start is called before the first frame update
    void Start()
    {
        tileMenu.SetActive(false);

        blockGrid = new GameObject[rows, cols];
        characterGrid = new GameObject[rows, cols];
        objectGrid = new GameObject[rows, cols];
        gridHeight = new int[rows, cols];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                gridHeight[row, col] = 0;
                blockGrid[row, col] = Instantiate(CombatMapper.blockMap[defaultBlock], new Vector3(col * xOffset, gridHeight[row, col] * zOffset, row * yOffset), Quaternion.identity);
                //blockGrid[row, col].transform.localScale = new Vector3(30, 30, 30);
                //blockGrid[row, col].transform.eulerAngles = new Vector3(-90, 0, 0);
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
                cameraX -= cameraSpeed * Time.deltaTime;
            }
            if (Input.GetKey("d"))
            {
                cameraX += cameraSpeed * Time.deltaTime;
            }
            if (Input.GetKey("s"))
            {
                cameraZ -= cameraSpeed * Time.deltaTime;
            }
            if (Input.GetKey("w"))
            {
                cameraZ += cameraSpeed * Time.deltaTime;
            }
            if (Input.GetKey("q"))
            {
                cameraY -= cameraSpeed * Time.deltaTime;
            }
            if (Input.GetKey("e"))
            {
                cameraY += cameraSpeed * Time.deltaTime;
            }

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
        combatCamera.transform.position = new Vector3((cols - 1) * xOffset / 2 + cameraX, cameraHeight + cameraY, cameraOffset + cameraZ);
        combatCamera.transform.eulerAngles = new Vector3(cameraAngle, 0, 0);
    }

    public void SetTileType(int tileTypeInput)
    {
        tileType = tileTypeInput;
    }

    public void SetCharacterIndex(int characterTypeInput)
    {
        selectedCharacter = characterTypeInput;
    }

    public void SetObjectIndex(int objectTypeInput)
    {
        selectedObject = objectTypeInput;
    }

    public void SetEditMode(string editModeInput)
    {
        editMode = editModeInput;
    }

    void PlaceObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Vector3 object_pos = hit.transform.gameObject.transform.position;
                int col = (int)Mathf.Round(object_pos.x / xOffset);
                int row = (int)Mathf.Round(object_pos.z / yOffset);
                GameObject Pobject = objectGrid[row, col];
                GameObject character = characterGrid[row, col];
                if (Pobject is null)
                {
                    GameObject block = blockGrid[row, col];
                    objectGrid[row, col] = Instantiate(CombatMapper.objectMap[selectedObject], block.transform.position + new Vector3(0, 0, 0), Quaternion.identity);
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Vector3 object_pos = hit.transform.gameObject.transform.position;
                int col = (int)Mathf.Round(object_pos.x / xOffset);
                int row = (int)Mathf.Round(object_pos.z / yOffset);
                GameObject Pobject = objectGrid[row, col];
                if (!(Pobject is null))
                {
                    Destroy(Pobject);
                    objectGrid[row, col] = null;
                }
            }
        }
    }

    void PlaceCharacter()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Vector3 object_pos = hit.transform.gameObject.transform.position;
                int col = (int)Mathf.Round(object_pos.x / xOffset);
                int row = (int)Mathf.Round(object_pos.z / yOffset);
                GameObject character = characterGrid[row, col];
                if (character is null)
                {
                    GameObject block = blockGrid[row, col];
                    characterGrid[row, col] = Instantiate(CombatMapper.characterMap[selectedCharacter], block.transform.position + new Vector3(0, 0, 0), Quaternion.identity);
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Vector3 object_pos = hit.transform.gameObject.transform.position;
                int col = (int)Mathf.Round(object_pos.x / xOffset);
                int row = (int)Mathf.Round(object_pos.z / yOffset);
                GameObject character = characterGrid[row, col];
                if(!(character is null))
                {
                    Destroy(character);
                    characterGrid[row, col] = null;
                }
            }
        }
    }

    void ChangeBlock()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Vector3 object_pos = hit.transform.gameObject.transform.position;
                int col = (int)Mathf.Round(object_pos.x / xOffset);
                int row = (int)Mathf.Round(object_pos.z / yOffset);
                GameObject block = blockGrid[row, col];
                GameObject newBlock = Instantiate(CombatMapper.blockMap[tileType]) as GameObject;
                newBlock.transform.position = block.transform.position;
                blockGrid[row, col] = newBlock;
                Destroy(block);
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Vector3 object_pos = hit.transform.gameObject.transform.position;
                int col = (int)Mathf.Round(object_pos.x / xOffset);
                int row = (int)Mathf.Round(object_pos.z / yOffset);
                GameObject block = blockGrid[row, col];
                GameObject newBlock = Instantiate(CombatMapper.blockMap[0]) as GameObject;
                newBlock.transform.position = block.transform.position;
                blockGrid[row, col] = newBlock;
                Destroy(block);
            }
        }
    }

    void MoveBlocksHeight()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Vector3 object_pos = hit.transform.gameObject.transform.position;
                int col = (int)Mathf.Round(object_pos.x / xOffset);
                int row = (int)Mathf.Round(object_pos.z / yOffset);
                GameObject block = blockGrid[row, col];
                gridHeight[row, col] += 1;
                block.transform.position = new Vector3(block.transform.position.x, gridHeight[row, col] * zOffset, block.transform.position.z);
                GameObject character = characterGrid[row, col];
                if (!(character is null))
                {
                    character.transform.position = block.transform.position + new Vector3(0, 0, 0);
                }
                GameObject Pobject = objectGrid[row, col];
                if (!(Pobject is null))
                {
                    Pobject.transform.position = block.transform.position + new Vector3(0, 0, 0);
                }
                for (int above_row = row + 1; above_row < rows; above_row++)
                {
                    if(gridHeight[row, col] > gridHeight[above_row, col])
                    {
                        gridHeight[above_row, col] += 1;
                        block = blockGrid[above_row, col];
                        block.transform.position = new Vector3(block.transform.position.x, gridHeight[above_row, col] * zOffset, block.transform.position.z);
                        character = characterGrid[above_row, col];
                        if (!(character is null))
                        {
                            character.transform.position = block.transform.position + new Vector3(0, 0, 0);
                        }
                        Pobject = objectGrid[above_row, col];
                        if (!(Pobject is null))
                        {
                            Pobject.transform.position = block.transform.position + new Vector3(0, 0, 0);
                        }
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                Vector3 object_pos = hit.transform.gameObject.transform.position;
                int col = (int)Mathf.Round(object_pos.x / xOffset);
                int row = (int)Mathf.Round(object_pos.z / yOffset);
                GameObject block = blockGrid[row, col];
                gridHeight[row, col] -= 1;
                block.transform.position = new Vector3(block.transform.position.x, gridHeight[row, col] * zOffset, block.transform.position.z);
                GameObject character = characterGrid[row, col];
                if (!(character is null))
                {
                    character.transform.position = block.transform.position + new Vector3(0, 0, 0);
                }
                GameObject Pobject = objectGrid[row, col];
                if (!(Pobject is null))
                {
                    Pobject.transform.position = block.transform.position + new Vector3(0, 0, 0);
                }
                for (int below_row = row - 1; below_row > -1; below_row--)
                {
                    if (gridHeight[row, col] < gridHeight[below_row, col])
                    {
                        gridHeight[below_row, col] -= 1;
                        block = blockGrid[below_row, col];
                        block.transform.position = new Vector3(block.transform.position.x, gridHeight[below_row, col] * zOffset, block.transform.position.z);
                        character = characterGrid[below_row, col];
                        if (!(character is null))
                        {
                            character.transform.position = block.transform.position + new Vector3(0, 0, 0);
                        }
                        Pobject = objectGrid[below_row, col];
                        if (!(Pobject is null))
                        {
                            Pobject.transform.position = block.transform.position + new Vector3(0, 0, 0);
                        }
                    }
                }
            }
        }

    }

    public void AddLeft()
    {
        GameObject[,] newCharacterGrid = new GameObject[rows, cols + 1];
        GameObject[,] newObjectGrid = new GameObject[rows, cols + 1];
        GameObject[,] newBlockGrid = new GameObject[rows, cols + 1];
        int[,] newGridHeight = new int[rows, cols + 1];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                newBlockGrid[row, col + 1] = blockGrid[row, col];
                newCharacterGrid[row, col + 1] = characterGrid[row, col];
                newObjectGrid[row, col + 1] = objectGrid[row, col];
                newGridHeight[row, col + 1] = gridHeight[row, col];
            }
        }
        for (int row = 0; row < rows; row++)
        {
            int col = 0;
            newGridHeight[row, col] = 0;
            newBlockGrid[row, col] = Instantiate(CombatMapper.blockMap[defaultBlock], new Vector3(col * xOffset, newGridHeight[row, col] * zOffset, row * yOffset), Quaternion.identity);
            //newBlockGrid[row, col].transform.eulerAngles = new Vector3(-90, 0, 0);
        }
        blockGrid = newBlockGrid;
        characterGrid = newCharacterGrid;
        gridHeight = newGridHeight;
        objectGrid = newObjectGrid;
        cols++;
        UpdatePositions();
    }

    public void SubLeft()
    {
        GameObject[,] newCharacterGrid = new GameObject[rows, cols - 1];
        GameObject[,] newObjectGrid = new GameObject[rows, cols - 1];
        GameObject[,] newBlockGrid = new GameObject[rows, cols - 1];
        int[,] newGridHeight = new int[rows, cols - 1];
        cols--;
        for (int row = 0; row < rows; row++)
        {
            int col = 0;
            GameObject character = characterGrid[row, col];
            if (!(character is null))
            {
                Destroy(characterGrid[row, col]);
            }
            GameObject Pobject = objectGrid[row, col];
            if (!(Pobject is null))
            {
                Destroy(objectGrid[row, col]);
            }
            Destroy(blockGrid[row, col]);
        }
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                newBlockGrid[row, col] = blockGrid[row, col + 1];
                newCharacterGrid[row, col] = characterGrid[row, col + 1];
                newObjectGrid[row, col] = objectGrid[row, col + 1];
                newGridHeight[row, col] = gridHeight[row, col + 1];
            }
        }
        blockGrid = newBlockGrid;
        characterGrid = newCharacterGrid;
        gridHeight = newGridHeight;
        objectGrid = newObjectGrid;
        UpdatePositions();
    }

    public void AddRight()
    {
        GameObject[,] newCharacterGrid = new GameObject[rows, cols + 1];
        GameObject[,] newObjectGrid = new GameObject[rows, cols + 1];
        GameObject[,] newBlockGrid = new GameObject[rows, cols + 1];
        int[,] newGridHeight = new int[rows, cols + 1];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                newBlockGrid[row, col] = blockGrid[row, col];
                newCharacterGrid[row, col] = characterGrid[row, col];
                newObjectGrid[row, col] = objectGrid[row, col];
                newGridHeight[row, col] = gridHeight[row, col];
            }
        }
        for (int row = 0; row < rows; row++)
        {
            int col = cols;
            newGridHeight[row, col] = 0;
            newBlockGrid[row, col] = Instantiate(CombatMapper.blockMap[defaultBlock], new Vector3(col * xOffset, newGridHeight[row, col] * zOffset, row * yOffset), Quaternion.identity);
            //newBlockGrid[row, col].transform.eulerAngles = new Vector3(-90, 0, 0);
        }
        blockGrid = newBlockGrid;
        characterGrid = newCharacterGrid;
        objectGrid = newObjectGrid;
        gridHeight = newGridHeight;
        cols++;
        UpdatePositions();
    }

    public void SubRight()
    {
        GameObject[,] newCharacterGrid = new GameObject[rows, cols - 1];
        GameObject[,] newObjectGrid = new GameObject[rows, cols - 1];
        GameObject[,] newBlockGrid = new GameObject[rows, cols - 1];
        int[,] newGridHeight = new int[rows, cols - 1];
        cols--;
        for (int row = 0; row < rows; row++)
        {
            int col = cols;
            GameObject character = characterGrid[row, col];
            if (!(character is null))
            {
                Destroy(characterGrid[row, col]);
            }
            GameObject Pobject = objectGrid[row, col];
            if (!(Pobject is null))
            {
                Destroy(objectGrid[row, col]);
            }
            Destroy(blockGrid[row, col]);
        }
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                newBlockGrid[row, col] = blockGrid[row, col];
                newCharacterGrid[row, col] = characterGrid[row, col];
                newObjectGrid[row, col] = objectGrid[row, col];
                newGridHeight[row, col] = gridHeight[row, col];
            }
        }
        blockGrid = newBlockGrid;
        characterGrid = newCharacterGrid;
        objectGrid = newObjectGrid;
        gridHeight = newGridHeight;
        UpdatePositions();
    }

    public void AddBottom()
    {
        GameObject[,] newCharacterGrid = new GameObject[rows + 1, cols];
        GameObject[,] newObjectGrid = new GameObject[rows + 1, cols];
        GameObject[,] newBlockGrid = new GameObject[rows + 1, cols];
        int[,] newGridHeight = new int[rows + 1, cols];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                newBlockGrid[row + 1, col] = blockGrid[row, col];
                newCharacterGrid[row + 1, col] = characterGrid[row, col];
                newObjectGrid[row + 1, col] = objectGrid[row, col];
                newGridHeight[row + 1, col] = gridHeight[row, col];
            }
        }
        for (int col = 0; col < cols; col++)
        {
            int row = 0;
            newGridHeight[row, col] = gridHeight[row, col];
            newBlockGrid[row, col] = Instantiate(CombatMapper.blockMap[defaultBlock], new Vector3(col * xOffset, newGridHeight[row, col] * zOffset, row * yOffset), Quaternion.identity);
            //newBlockGrid[row, col].transform.eulerAngles = new Vector3(-90, 0, 0);
        }
        blockGrid = newBlockGrid;
        characterGrid = newCharacterGrid;
        objectGrid = newObjectGrid;
        gridHeight = newGridHeight;
        rows++;
        UpdatePositions();
    }

    public void SubBottom()
    {
        GameObject[,] newCharacterGrid = new GameObject[rows - 1, cols];
        GameObject[,] newObjectGrid = new GameObject[rows - 1, cols];
        GameObject[,] newBlockGrid = new GameObject[rows - 1, cols];
        int[,] newGridHeight = new int[rows - 1, cols];
        rows--;
        for (int col = 0; col < cols; col++)
        {
            int row = 0;
            GameObject character = objectGrid[row, col];
            if (!(character is null))
            {
                Destroy(objectGrid[row, col]);
            }
            Destroy(blockGrid[row, col]);
        }
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                newBlockGrid[row, col] = blockGrid[row + 1, col];
                newObjectGrid[row, col] = objectGrid[row + 1, col];
                newGridHeight[row, col] = gridHeight[row + 1, col];
            }
        }
        blockGrid = newBlockGrid;
        characterGrid = newCharacterGrid;
        objectGrid = newObjectGrid;
        gridHeight = newGridHeight;
        UpdatePositions();
    }

    public void AddTop()
    {
        GameObject[,] newCharacterGrid = new GameObject[rows + 1, cols];
        GameObject[,] newObjectGrid = new GameObject[rows + 1, cols];
        GameObject[,] newBlockGrid = new GameObject[rows + 1, cols];
        int[,] newGridHeight = new int[rows + 1, cols];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                newBlockGrid[row, col] = blockGrid[row, col];
                newCharacterGrid[row, col] = characterGrid[row, col];
                newObjectGrid[row, col] = objectGrid[row, col];
                newGridHeight[row, col] = gridHeight[row, col];
            }
        }
        for (int col = 0; col < cols; col++)
        {
            int row = rows;
            newGridHeight[row, col] = gridHeight[row - 1, col];
            newBlockGrid[row, col] = Instantiate(CombatMapper.blockMap[defaultBlock], new Vector3(col * xOffset, newGridHeight[row, col] * zOffset, row * yOffset), Quaternion.identity);
            //newBlockGrid[row, col].transform.eulerAngles = new Vector3(-90, 0, 0);
        }
        blockGrid = newBlockGrid;
        characterGrid = newCharacterGrid;
        objectGrid = newObjectGrid;
        gridHeight = newGridHeight;
        rows++;
        UpdatePositions();
    }

    public void SubTop()
    {
        GameObject[,] newCharacterGrid = new GameObject[rows - 1, cols];
        GameObject[,] newObjectGrid = new GameObject[rows - 1, cols];
        GameObject[,] newBlockGrid = new GameObject[rows - 1, cols];
        int[,] newGridHeight = new int[rows - 1, cols];
        rows--;
        for (int col = 0; col < cols; col++)
        {
            int row = rows;
            GameObject character = characterGrid[row, col];
            if (!(character is null))
            {
                Destroy(characterGrid[row, col]);
            }
            GameObject Pobject = objectGrid[row, col];
            if (!(Pobject is null))
            {
                Destroy(objectGrid[row, col]);
            }
            Destroy(blockGrid[row, col]);
        }
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                newBlockGrid[row, col] = blockGrid[row, col];
                newCharacterGrid[row, col] = characterGrid[row, col];
                newObjectGrid[row, col] = objectGrid[row, col];
                newGridHeight[row, col] = gridHeight[row, col];
            }
        }
        blockGrid = newBlockGrid;
        characterGrid = newCharacterGrid;
        objectGrid = newObjectGrid;
        gridHeight = newGridHeight;
        UpdatePositions();
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

    public void Save()
    {
        string filename = saveNameTextField.GetComponent<InputField>().text;
        CombatContainer combatContainer = ScriptableObject.CreateInstance<CombatContainer>();

        //Save Grid Height
        combatContainer.gridHeight = new int[rows * cols];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                combatContainer.gridHeight[row * cols + col] = gridHeight[row, col];
            }
        }
        //Save Block Grid
        combatContainer.blockGrid = new int[rows * cols];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject block = blockGrid[row, col];
                for (int block_idx = 0; block_idx < CombatMapper.blockMap.ToList().Count; block_idx++)
                {
                    if (block.name.Remove(block.name.Length - 7) == CombatMapper.blockMap[block_idx].name)
                    {
                        combatContainer.blockGrid[row * cols + col] = block_idx;
                        break;
                    }
                }
            }
        }
        //Save Character Grid
        combatContainer.characterGrid = new int[rows * cols];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject character = characterGrid[row, col];
                for (int character_idx = 0; character_idx < CombatMapper.characterMap.ToList().Count; character_idx++)
                {
                    if (character is null)
                    {
                        combatContainer.characterGrid[row * cols + col] = -1;
                        break;
                    }
                    else
                    {
                        if (character.name.Remove(character.name.Length - 7) == CombatMapper.characterMap[character_idx].name)
                        {
                            combatContainer.characterGrid[row * cols + col] = character_idx;
                            break;
                        }
                    }
                }
            }
        }
        //Save Object Grid
        combatContainer.objectGrid = new int[rows * cols];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject Pobject = objectGrid[row, col];
                for (int object_idx = 0; object_idx < CombatMapper.blockMap.ToList().Count; object_idx++)
                {
                    if (Pobject is null)
                    {
                        combatContainer.objectGrid[row * cols + col] = -1;
                        break;
                    }
                    else
                    {
                        if (Pobject.name.Remove(Pobject.name.Length - 7) == CombatMapper.objectMap[object_idx].name)
                        {
                            combatContainer.objectGrid[row * cols + col] = object_idx;
                            break;
                        }
                    }
                }
            }
        }
        //Save rows
        combatContainer.rows = rows;
        //Save cols 
        combatContainer.cols = cols;

        AssetDatabase.CreateAsset(combatContainer, $"Assets/CombatPrefabs/CombatBlocks/Resources/{filename}.asset");
        AssetDatabase.SaveAssets();
    }

    void Clear()
    {
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                GameObject block = blockGrid[row, col];
                Destroy(block);
                GameObject character = characterGrid[row, col];
                Destroy(character);
            }
        }
    }

    public void Load()
    {
        Clear();
        string filename = saveNameTextField.GetComponent<InputField>().text;

        CombatContainer _containerCache = Resources.Load<CombatContainer>(filename);
        if (_containerCache == null)
        {
            EditorUtility.DisplayDialog("File Not Found", "Target dialogue graph file does not exist!", "OK");
        }
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
                }
                if (_containerCache.objectGrid[row * cols + col] > -1)
                {
                    objectGrid[row, col] = Instantiate(CombatMapper.objectMap[_containerCache.objectGrid[row * cols + col]], blockGrid[row, col].transform.position + new Vector3(0, 0, 0), Quaternion.identity);
                }
            }
        }
        UpdatePositions();
    }
}
