using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapProcesses
{
    public static List<Vector2Int> FindNearestTileNoCharacter(Vector2Int currentPos, int closestDist, GameObject targetObject)
    {
        List<Vector2Int> targetOptions = new List<Vector2Int>();
        List<Vector2Int> potentialOccupiedTiles;
        //Top Side
        for (int y = -closestDist; y <= closestDist; y++)
        {
            Vector2Int newPos = new Vector2Int(currentPos.x - closestDist, currentPos.y + y);
            potentialOccupiedTiles = targetObject.GetComponent<GridObject>().PotentialGridOccupation(newPos);
            if (isThisListOnGrid(potentialOccupiedTiles))
            {
                if(isTileEmpty(potentialOccupiedTiles, targetObject))
                {
                    targetOptions.Add(newPos);
                }
            }
        }
        for (int y = -closestDist; y <= closestDist; y++)
        {
            Vector2Int newPos = new Vector2Int(currentPos.x + closestDist, currentPos.y + y);
            potentialOccupiedTiles = targetObject.GetComponent<GridObject>().PotentialGridOccupation(newPos);
            if (isThisListOnGrid(potentialOccupiedTiles))
            {
                if (isTileEmpty(potentialOccupiedTiles, targetObject))
                {
                    targetOptions.Add(newPos);
                }
            }
        }
        for (int x = -closestDist + 1; x < closestDist; x++)
        {
            Vector2Int newPos = new Vector2Int(currentPos.x + x, currentPos.y - closestDist);
            potentialOccupiedTiles = targetObject.GetComponent<GridObject>().PotentialGridOccupation(newPos);
            if (isThisListOnGrid(potentialOccupiedTiles))
            {
                if (isTileEmpty(potentialOccupiedTiles, targetObject))
                {
                    targetOptions.Add(newPos);
                }
            }
        }
        for (int x = -closestDist + 1; x < closestDist; x++)
        {
            Vector2Int newPos = new Vector2Int(currentPos.x + x, currentPos.y + closestDist);
            potentialOccupiedTiles = targetObject.GetComponent<GridObject>().PotentialGridOccupation(newPos);
            if (isThisListOnGrid(potentialOccupiedTiles))
            {
                if (isTileEmpty(potentialOccupiedTiles, targetObject))
                {
                    targetOptions.Add(newPos);
                }
            }
        }
        if (targetOptions.Count == 0)
        {
            return FindNearestTileNoCharacter(currentPos, closestDist + 1, targetObject);
        }
        return targetOptions;
    }

    public static bool isObjectPassable(Vector2Int pos)
    {
        if (!(CombatExecutor.objectGrid[(int)pos.x, (int)pos.y] is null))
        {
            if (!CombatExecutor.objectGrid[(int)pos.x, (int)pos.y].GetComponent<CombatObject>().Passable)
            {
                return false;
            }
        }
        return true;
    }

    public static bool isTileEmpty(List<Vector2Int> positions, GameObject checkingObject)
    {
        foreach(Vector2Int pos in positions)
        {
            if (!isObjectPassable(pos) || (CombatExecutor.characterGrid[pos.x, pos.y] != null && CombatExecutor.characterGrid[pos.x, pos.y] != checkingObject))
            {
                return false;
            }
        }
        return true;
    }

    public static bool CanIMoveToTile(Vector2Int pos, CombatObject traveler)
    {
        int height_difference = CombatExecutor.gridHeight[pos.x, pos.y] - CombatExecutor.gridHeight[traveler.pos.x, traveler.pos.y];
        if (height_difference > traveler.MaxJumpHeight || !isTravelTypeCompatible(pos, traveler))
        {
            return false;
        }
        return true;
    }

    public static bool isTravelTypeCompatible(Vector2Int pos, CombatObject traveler)
    {
        BlockTemplate block_info = CombatExecutor.blockGrid[pos.x, pos.y].GetComponent<BlockTemplate>();
        if ((block_info.Walkable && traveler.CanWalk) || (block_info.Swimable && traveler.CanSwim) || (block_info.Flyable && traveler.CanFly))
        {
            return true;
        }
        return false;
    }

    public static bool isThisOnTheGrid(Vector2Int pos)
    {
        if (pos.x >= 0 && pos.x < CombatExecutor.mapShape.x && pos.y >= 0 && pos.y < CombatExecutor.mapShape.y)
        {
            return true;
        }
        return false;
    }

    public static bool isThisListOnGrid(List<Vector2Int> posList)
    {
        foreach(Vector2Int pos in posList)
        {
            if (!isThisOnTheGrid(pos)) return false;
        }
        return true;
    }

    public static bool doesObjectOverlapTargets(List<Vector2Int> targetTiles, GridObject objectTarget)
    {
        if (targetTiles.Contains(objectTarget.pos)) return true;
        foreach (Vector2Int pos in objectTarget.extra_pos)
        {
            if (targetTiles.Contains(pos)) return true;
        }
        return false;
    }

    //Assumes that you can only go too far left or too far right.
    public static Vector2Int findNearestTileFullyFitsObject(Vector2Int targetSize, Vector2Int pos)
    {
        Vector2Int mapSize = CombatExecutor.mapShape;
        Vector2Int placeholderPos = pos;
        int horizontalOvershoot = pos.x + targetSize.x - mapSize.x;
        if (horizontalOvershoot > 0)
        {
            placeholderPos.x -= horizontalOvershoot;
        }
        int verticalOvershoot = pos.y + targetSize.y - mapSize.y;
        if (verticalOvershoot > 0)
        {
            placeholderPos.y -= verticalOvershoot;
        }
        return placeholderPos;
    }
}
