using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public string name = "No Name";
    [HideInInspector] public Vector2Int pos;
    [HideInInspector] public Vector2Int prevPos;
    [HideInInspector] public List<Vector2Int> extra_pos = new List<Vector2Int>();
    [HideInInspector] public Vector2Int TileSize = new Vector2Int(1, 1);
    [HideInInspector] public GameObject[,] ContainingGrid = null;
    [HideInInspector] public int objectID;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void MoveObject(Vector2Int end_pos)
    {
        RemoveObjectFromGrid();
        AddObjectToGrid(end_pos);
    }

    public void RemoveObjectFromGrid()
    {
        prevPos = pos;
        ContainingGrid[pos.x, pos.y] = null;
        foreach(Vector2Int ext_pos in extra_pos)
        {
            ContainingGrid[ext_pos.x, ext_pos.y] = null;
        }
    }

    public void AddObjectToGrid(Vector2Int target_pos)
    {
        ContainingGrid[target_pos.x, target_pos.y] = gameObject;
        pos = target_pos;
        extra_pos = new List<Vector2Int>();
        for (int x = 0; x < TileSize.x; x++)
        {
            for (int y = 0; y < TileSize.y; y++)
            {
                if (x != 0 || y != 0)
                {
                    ContainingGrid[target_pos.x + x, target_pos.y + y] = gameObject;
                    extra_pos.Add(new Vector2Int(target_pos.x + x, target_pos.y + y));
                }
            }
        }
    }

    public void DestoryObject()
    {
        RemoveObjectFromGrid();
        Destroy(gameObject);
    }

    public List<Vector2Int> PotentialGridOccupation(Vector2Int target_pos)
    {
        List<Vector2Int> potentialGridOccupation = new List<Vector2Int>();
        for (int x = 0; x < TileSize.x; x++)
        {
            for (int y = 0; y < TileSize.y; y++)
            {
                potentialGridOccupation.Add(new Vector2Int(target_pos.x + x, target_pos.y + y));
            }
        }
        return potentialGridOccupation;
    }
    
    public List<Vector2Int> currentGridOccupation()
    {
        return PotentialGridOccupation(pos);
    }
}
