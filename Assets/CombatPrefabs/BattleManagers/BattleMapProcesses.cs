using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMapProcesses
{


    public static List<Vector2> FindNearestTileNoCharacter(Vector2 currentPos, int closestDist, CombatExecutor combatExecutor)
    {
        List<Vector2> targetOptions = new List<Vector2>();
        //Top Side
        for (int col = -closestDist; col <= closestDist; col++)
        {
            Vector2 newPos = new Vector2(currentPos.x - closestDist, currentPos.y + col);
            if (isThisOnTheGrid(newPos, combatExecutor))
            {
                if (combatExecutor.characterGrid[(int)newPos.x, (int)newPos.y] is null && isObjectPassable(newPos, combatExecutor))
                {
                    targetOptions.Add(newPos);
                }
            }
        }
        for (int col = -closestDist; col <= closestDist; col++)
        {
            Vector2 newPos = new Vector2(currentPos.x + closestDist, currentPos.y + col);
            if (isThisOnTheGrid(newPos, combatExecutor))
            {
                if (combatExecutor.characterGrid[(int)newPos.x, (int)newPos.y] is null && isObjectPassable(newPos, combatExecutor))
                {
                    targetOptions.Add(newPos);
                }
            }
        }
        for (int row = -closestDist + 1; row < closestDist; row++)
        {
            Vector2 newPos = new Vector2(currentPos.x + row, currentPos.y - closestDist);
            if (isThisOnTheGrid(newPos, combatExecutor))
            {
                if (combatExecutor.characterGrid[(int)newPos.x, (int)newPos.y] is null && isObjectPassable(newPos, combatExecutor))
                {
                    targetOptions.Add(newPos);
                }
            }
        }
        for (int row = -closestDist + 1; row < closestDist; row++)
        {
            Vector2 newPos = new Vector2(currentPos.x + row, currentPos.y + closestDist);
            if (isThisOnTheGrid(newPos, combatExecutor))
            {
                if (combatExecutor.characterGrid[(int)newPos.x, (int)newPos.y] is null && isObjectPassable(newPos, combatExecutor))
                {
                    targetOptions.Add(newPos);
                }
            }
        }
        if (targetOptions.Count == 0)
        {
            return FindNearestTileNoCharacter(currentPos, closestDist + 1, combatExecutor);
        }
        return targetOptions;
    }

    public static bool isObjectPassable(Vector2 pos, CombatExecutor combatExecutor)
    {
        if (!(combatExecutor.objectGrid[(int)pos.x, (int)pos.y] is null))
        {
            if (!combatExecutor.objectGrid[(int)pos.x, (int)pos.y].GetComponent<ObjectTemplate>().Passable)
            {
                return false;
            }
        }
        return true;
    }

    public static bool isThisOnTheGrid(Vector2 pos, CombatExecutor combatExecutor)
    {
        if (pos.x >= 0 && pos.x < combatExecutor.rows && pos.y >= 0 && pos.y < combatExecutor.cols)
        {
            return true;
        }
        return false;
    }
}
