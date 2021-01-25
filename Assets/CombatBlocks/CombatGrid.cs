using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatGrid : MonoBehaviour
{
    public GameObject defaultBlock;

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
    private int[,] gridHeight;

    //UI Stuff
    public GameObject tileMenu;
    private string tileType = "GrassTile";
    private string editMode = "Height";

    // Start is called before the first frame update
    void Start()
    {
        tileMenu.SetActive(false);

        blockGrid = new GameObject[rows,cols];
        gridHeight = new int[rows, cols];
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                gridHeight[row, col] = 0;
                blockGrid[row, col] = Instantiate(defaultBlock, new Vector3(col * xOffset, gridHeight[row, col] * zOffset, row * yOffset), Quaternion.identity);
                //blockGrid[row, col].transform.localScale = new Vector3(30, 30, 30);
                blockGrid[row, col].transform.eulerAngles = new Vector3(-90, 0, 0);
            }
        }
    }

    // Update is called once per frame
    void Update()
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

        combatCamera.transform.position = new Vector3((cols - 1) * xOffset / 2 + cameraX, cameraHeight + cameraY, cameraOffset + cameraZ);
        combatCamera.transform.eulerAngles = new Vector3(cameraAngle, 0, 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            tileMenu.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            tileMenu.SetActive(false);
        }
        if (!Input.GetKey(KeyCode.Space))
        {
            if (editMode == "Height")
            {
                MoveBlocksHeight();
            }
            if (editMode == "Type")
            {
                ChangeBlock();
            }
        }
    }

    public void SetTileType(string tileTypeInput)
    {
        tileType = tileTypeInput;
    }

    public void SetEditMode(string editModeInput)
    {
        editMode = editModeInput;
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
                int col = Mathf.FloorToInt(object_pos.x / xOffset);
                int row = Mathf.FloorToInt(object_pos.z / yOffset);
                GameObject block = blockGrid[row, col];
                GameObject newBlock = Instantiate(Resources.Load(tileType, typeof(GameObject))) as GameObject;
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
                int col = Mathf.FloorToInt(object_pos.x / xOffset);
                int row = Mathf.FloorToInt(object_pos.z / yOffset);
                GameObject block = blockGrid[row, col];
                GameObject newBlock = Instantiate(Resources.Load("EmptyTile", typeof(GameObject))) as GameObject;
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
                int col = Mathf.FloorToInt(object_pos.x / xOffset);
                int row = Mathf.FloorToInt(object_pos.z / yOffset);
                GameObject block = blockGrid[row, col];
                gridHeight[row, col] += 1;
                block.transform.position = new Vector3(block.transform.position.x, gridHeight[row, col] * zOffset, block.transform.position.z);
                for (int above_row = row + 1; above_row < rows; above_row++)
                {
                    if(gridHeight[row, col] > gridHeight[above_row, col])
                    {
                        gridHeight[above_row, col] += 1;
                        block = blockGrid[above_row, col];
                        block.transform.position = new Vector3(block.transform.position.x, gridHeight[above_row, col] * zOffset, block.transform.position.z);
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
                int col = Mathf.FloorToInt(object_pos.x / xOffset);
                int row = Mathf.FloorToInt(object_pos.z / yOffset);
                GameObject block = blockGrid[row, col];
                gridHeight[row, col] -= 1;
                block.transform.position = new Vector3(block.transform.position.x, gridHeight[row, col] * zOffset, block.transform.position.z);
                for (int below_row = row - 1; below_row > -1; below_row--)
                {
                    if (gridHeight[row, col] < gridHeight[below_row, col])
                    {
                        gridHeight[below_row, col] -= 1;
                        block = blockGrid[below_row, col];
                        block.transform.position = new Vector3(block.transform.position.x, gridHeight[below_row, col] * zOffset, block.transform.position.z);
                    }
                }
            }
        }

    }
}
