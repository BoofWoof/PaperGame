using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMenu : MonoBehaviour
{
    //Menu
    private bool menuEnabled = true;

    private int menuItemCount = 0;
    private float rotation;

    //Rotation Over Time
    public float rotationSpeed = 360.0f;
    private float rotationGoal = 0;
    private float rotationDone = 0;

    //Tile Selectiong
    private int selectedTile = 0;

    //CombatController
    [HideInInspector] public GameObject comControl = null;
    [HideInInspector] public List<GameObject> MenuTiles;
    [HideInInspector] public List<GameObject> InstantiatedTiles = new List<GameObject>();

    //Data
    [HideInInspector] public int myID;
    [HideInInspector] public List<GameObject> friendlyList;
    [HideInInspector] public List<GameObject> enemyList;
    [HideInInspector] public bool friendly = false;

    // Start is called before the first frame update
    void Start()
    {
        menuItemCount = MenuTiles.Count;
        rotation = 360 / menuItemCount;
        Vector3 spawnLocation = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
        GameObject newTile;
        foreach (GameObject Tile in MenuTiles)
        {
            newTile = Instantiate<GameObject>(Tile, spawnLocation, Quaternion.identity);
            newTile.transform.SetParent(transform);
            transform.Rotate(0, 0, rotation);
            newTile.GetComponent<MoveClass>().friendlyList = friendlyList;
            newTile.GetComponent<MoveClass>().enemyList = enemyList;
            newTile.GetComponent<MoveClass>().myID = myID;
            newTile.GetComponent<MoveClass>().friendly = friendly;
            InstantiatedTiles.Add(newTile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (menuEnabled)
        {
            if (rotationGoal == 0)
            {
                if (Input.GetButtonDown("Fire1"))
                {
                    selectTile();
                }
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
            if (selectedTile < 0)
            {
                selectedTile = menuItemCount + selectedTile;
            }
            if (selectedTile >= menuItemCount)
            {
                selectedTile = selectedTile - menuItemCount;
            }
            float rotationAmount = 0;
            if (rotationGoal > 0)
            {
                rotationAmount = rotationSpeed * Time.deltaTime;
            }
            if (rotationGoal < 0)
            {
                rotationAmount = -rotationSpeed * Time.deltaTime;
            }
            rotationDone = rotationDone + rotationAmount;
            transform.Rotate(0, 0, rotationAmount);
            if (rotationDone * rotationDone > rotationGoal * rotationGoal)
            {
                transform.eulerAngles = new Vector3(0, 0, selectedTile * rotation);
                rotationGoal = 0;
                rotationDone = 0;
            }
        }
    }
    private void selectTile()
    {
        InstantiatedTiles[selectedTile].GetComponent<MoveClass>().select();
    }

    public void enableMenu()
    {
        menuEnabled = true;
        transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
    }
    public void disableMenu()
    {
        menuEnabled = false;
        transform.position = new Vector3(transform.position.x, transform.position.y - 10, transform.position.z);
    }

}
