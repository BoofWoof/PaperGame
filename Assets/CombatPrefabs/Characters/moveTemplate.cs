using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveTemplate : MonoBehaviour
{
    public enum TargetType
    {
        None,
        Self,
        Clip,
        Partner,
        Allies,
        Enemies,
        Flying,
        Submerged,
        Ground,
        Tile,
        Object
    }

    public enum TargetShape
    {
        Global,
        Diamond,
        Square,
        Cross,
        X
    }

    public enum TargetQuantity
    {
        None,
        Single,
        Multiple,
        All,
        Random
    }

    [Header("Attack Info")]
    public string name;
    public string combatDescription;
    public GameObject character;
    public List<GameObject> target;
    public int moveIndex;

    [Header("Attack Targeting")]
    public TargetType targetType = TargetType.None;
    public TargetQuantity targetQuantity = TargetQuantity.None;
    public TargetShape targetShape = TargetShape.Global;

    [Header("Target Shape Info")]
    public int MaxRange = 4;
    public int MinRange = 2;

    [Header("Multiple Target Info")]
    public int targetCount = 1;

    public virtual void Activate(List<GameObject> targets)
    {
        
    }

    public List<Vector2> findGoals(
        List<GameObject> targets,
        int rows,
        int cols
        )
    {
        if (targetShape == TargetShape.Square)
        {
            return findGoalsSquare(
                targets,
                rows,
                cols
                );
        }
        return new List<Vector2>();
    }

    public List<Vector2> findGoalsSquare(
        List<GameObject> targets,
        int rows,
        int cols
        )
    {
        List<Vector2> goalList = new List<Vector2>();
        foreach(GameObject targetObject in targets)
        {
            Vector2 targetPos = targetObject.GetComponent<FighterClass>().pos;
            for (int row = -MaxRange; row <= MaxRange; row++)
            {
                for (int col = -MaxRange; col <= MaxRange; col++)
                {
                    if (Mathf.Abs(row) >= MinRange)
                    {
                        Vector2 goal = targetPos + new Vector2(row, col);
                        if (!goalList.Contains(goal))
                        {
                            goalList.Add(goal);
                        }
                    }
                }
            }
        }
        return goalList;
    }
}
