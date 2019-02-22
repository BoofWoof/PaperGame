using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMenu : MonoBehaviour
{
    //CAN THE MENU INTERACT-----------------
    private bool menuEnabled = true;
    //-------------------------------------

    //MENU STATS--------------------------
    private int menuItemCount = 0;
    private int selectedTile = 0;
    //------------------------------------

    //ROTATION OVER TIME VARIABLES--------
    private float rotation;
    public float rotationSpeed = 360.0f;
    private float rotationGoal = 0;
    private float rotationDone = 0;
    //------------------------------------

    //MENU TILES INPUT AND INSTANTIATION-----------------------------------------------------
    [HideInInspector] public List<GameObject> MenuTiles;
    [HideInInspector] public List<GameObject> InstantiatedTiles = new List<GameObject>();
    //--------------------------------------------------------------------------------------

    //MENU CREATOR IDENTIFICATION--------------------------
    [HideInInspector] public int sourceID;
    [HideInInspector] public bool friendlySource = false;
    //-----------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        //HOW MUCH DO WE NEED TO ROTATE TO GET TO A NEW MENU ITEM--
        menuItemCount = MenuTiles.Count;
        rotation = 360 / menuItemCount;
        //---------------------------------------------------------

        //SPAWNS ALL THE TILES FOR THE MENU-------------------------------------------------------------------------
        Vector3 spawnLocation = new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z);
        GameObject newTile;
        foreach (GameObject Tile in MenuTiles)
        {
            newTile = Instantiate<GameObject>(Tile, spawnLocation, Quaternion.identity);
            newTile.transform.SetParent(transform);
            transform.Rotate(0, 0, rotation);
            newTile.GetComponent<MoveClass>().sourceID = sourceID;
            newTile.GetComponent<MoveClass>().friendlySource = friendlySource;
            InstantiatedTiles.Add(newTile);
        }
        //-----------------------------------------------------------------------------------------------------------
    }

    // Update is called once per frame
    void Update()
    {
        if (menuEnabled)
        {
            //UNSER INPUT BASED MENU CHANGING-----------------------
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
            //------------------------------------------------------

            //MAKES SURE SELECTED TILE IS IN RANGE------------------
            if (selectedTile < 0)
            {
                selectedTile = menuItemCount + selectedTile;
            }
            if (selectedTile >= menuItemCount)
            {
                selectedTile = selectedTile - menuItemCount;
            }
            //------------------------------------------------------

            //HOW MUCH TO ROTATE THE MENU-----------------------------------------------
            float rotationAmount = 0;
            if (rotationGoal > 0)
            {
                rotationAmount = rotationSpeed * Time.deltaTime;
            }
            if (rotationGoal < 0)
            {
                rotationAmount = -rotationSpeed * Time.deltaTime;
            }
            //---------------------------------------------------------------------------

            //ROTATE AND RECORD THE AMOUNT OF ROTATION----------------
            rotationDone = rotationDone + rotationAmount;
            transform.Rotate(0, 0, rotationAmount);
            //-------------------------------------------------------

            //MAKES SURE THE ROTATION DOES NOT OVERSHOOT ITS GOAL----------------------
            if (rotationDone * rotationDone > rotationGoal * rotationGoal)
            {
                transform.eulerAngles = new Vector3(0, 0, selectedTile * rotation);
                rotationGoal = 0;
                rotationDone = 0;
            }
            //--------------------------------------------------------------------------
        }
    }

    private void selectTile()
    {
        //ACTIVATES THE SELECTED TILE----------------------------------------
        InstantiatedTiles[selectedTile].GetComponent<MoveClass>().select();
        //-------------------------------------------------------------------
    }

    //ENABLES AND DISABLES THE MENU----------------------------------------------------------------------------------
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
    //---------------------------------------------------------------------------------------------------------------
}
