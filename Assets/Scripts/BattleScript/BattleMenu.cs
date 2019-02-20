using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMenu : MonoBehaviour
{
    private List<GameObject> MenuTiles = new List<GameObject>();
    private int menuItemCount = 0;
    private float rotation;
    public GameObject Tile1 = null;
    public GameObject Tile2 = null;
    public GameObject Tile3 = null;
    public GameObject Tile4 = null;
    public GameObject Tile5 = null;
    public GameObject Tile6 = null;
    public GameObject Tile7 = null;

    //Rotation Over Time
    public float rotationSpeed = 360.0f;
    private float rotationGoal = 0;
    private float rotationDone = 0;

    //Tile Selectiong
    private int selectedTile = 0;

    //CombatController
    [HideInInspector] public GameObject comControl = null;

    // Start is called before the first frame update
    void Start()
    {
        if (Tile1 != null)
        {
            MenuTiles.Add(Tile1);
        }
        if (Tile2 != null)
        {
            MenuTiles.Add(Tile2);
        }
        if (Tile3 != null)
        {
            MenuTiles.Add(Tile3);
        }
        if (Tile4 != null)
        {
            MenuTiles.Add(Tile4);
        }
        if (Tile5 != null)
        {
            MenuTiles.Add(Tile5);
        }
        if (Tile6 != null)
        {
            MenuTiles.Add(Tile6);
        }
        if (Tile7 != null)
        {
            MenuTiles.Add(Tile7);
        }
        menuItemCount = MenuTiles.Count;
        rotation = 360 / menuItemCount;
        Vector3 spawnLocation = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
        foreach (GameObject Tile in MenuTiles)
        {
            GameObject newTile = Instantiate<GameObject>(Tile, spawnLocation, Quaternion.identity);
            newTile.transform.SetParent(transform);
            transform.Rotate(0, 0, rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (rotationGoal == 0)
        {
            float horizontalInput = Input.GetAxis("Vertical");
            if (horizontalInput > 0)
            {
                rotationGoal = rotation;
                selectedTile++;
            }
            if (horizontalInput < 0)
            {
                rotationGoal = -rotation;
                selectedTile--;
            }
        }
        if(selectedTile < 0)
        {
            selectedTile = menuItemCount + selectedTile;
        }
        if (selectedTile >= menuItemCount)
        {
            selectedTile = selectedTile - menuItemCount;
        }
        float rotationAmount = 0;
        if(rotationGoal > 0)
        {
            rotationAmount = rotationSpeed * Time.deltaTime;
        }
        if (rotationGoal < 0)
        {
            rotationAmount = -rotationSpeed * Time.deltaTime;
        }
        rotationDone = rotationDone + rotationAmount;
        transform.Rotate(0, 0, rotationAmount);
        if(rotationDone* rotationDone > rotationGoal * rotationGoal)
        {
            transform.eulerAngles = new Vector3(0, 0, selectedTile * rotation);
            rotationGoal = 0;
            rotationDone = 0;
        }

    }
}
