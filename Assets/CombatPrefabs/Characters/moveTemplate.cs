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
    //public List<GameObject> target;
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

    [HideInInspector] public bool activated = false;

    public virtual void Activate(List<GameObject> targets)
    {
        activated = true;
    }

    public (List<Vector2>, List<GameObject>) findGoals(
        List<GameObject> targets,
        int rows,
        int cols
        )
    {
        //There is some inefficiency here.
        //Some goals can be placed out of bounds.
        //+ and x can also have duplicated goals.
        //MAKE THIS INTO A DICTIONARY PROBABLY
        if (targetShape == TargetShape.Square)
        {
            return findGoalsSquare(
                targets,
                rows,
                cols
                );
        }
        if (targetShape == TargetShape.Diamond)
        {
            return findGoalsDiamond(
                targets,
                rows,
                cols
                );
        }
        if (targetShape == TargetShape.Cross)
        {
            return findGoalsCross(
                targets,
                rows,
                cols
                );
        }
        if (targetShape == TargetShape.X)
        {
            return findGoalsX(
                targets,
                rows,
                cols
                );
        }
        return (new List<Vector2>(), new List<GameObject>());
    }

    public (List<Vector2>, List<GameObject>) findGoalsSquare(
        List<GameObject> targets,
        int rows,
        int cols
        )
    {
        List<Vector2> goalList = new List<Vector2>();
        List<GameObject> objectList = new List<GameObject>();
        foreach (GameObject targetObject in targets)
        {
            Vector2 targetPos = targetObject.GetComponent<FighterClass>().pos;
            for (int row = -MaxRange; row <= MaxRange; row++)
            {
                for (int col = -MaxRange; col <= MaxRange; col++)
                {
                    if (Mathf.Abs(row) >= MinRange || Mathf.Abs(col) >= MinRange)
                    {
                        Vector2 goal = targetPos + new Vector2(row, col);
                        if (!goalList.Contains(goal))
                        {
                            goalList.Add(goal);
                            objectList.Add(targetObject);
                        }
                    }
                }
            }
        }
        return (goalList, objectList);
    }

    public (List<Vector2>, List<GameObject>) findGoalsDiamond(
        List<GameObject> targets,
        int rows,
        int cols
        )
    {
        List<Vector2> goalList = new List<Vector2>();
        List<GameObject> objectList = new List<GameObject>();
        foreach (GameObject targetObject in targets)
        {
            Vector2 targetPos = targetObject.GetComponent<FighterClass>().pos;
            for (int row = -MaxRange; row <= MaxRange; row++)
            {
                for (int col = -MaxRange + Mathf.Abs(row); col <= MaxRange - Mathf.Abs(row); col++)
                {
                    if (Mathf.Abs(row) + Mathf.Abs(col) >= MinRange)
                    {
                        Vector2 goal = targetPos + new Vector2(row, col);
                        if (!goalList.Contains(goal))
                        {
                            goalList.Add(goal);
                            objectList.Add(targetObject);
                        }
                    }
                }
            }
        }
        return (goalList, objectList);
    }

    public (List<Vector2>, List<GameObject>) findGoalsCross(
        List<GameObject> targets,
        int rows,
        int cols
        )
    {
        List<Vector2> goalList = new List<Vector2>();
        List<GameObject> objectList = new List<GameObject>();
        foreach (GameObject targetObject in targets)
        {
            Vector2 targetPos = targetObject.GetComponent<FighterClass>().pos;
            for (int idx = MinRange; idx <= MaxRange; idx++)
            {
                goalList.Add(targetPos + new Vector2(idx, 0));
                objectList.Add(targetObject);
                goalList.Add(targetPos + new Vector2(-idx, 0));
                objectList.Add(targetObject);
                goalList.Add(targetPos + new Vector2(0, idx));
                objectList.Add(targetObject);
                goalList.Add(targetPos + new Vector2(0, -idx));
                objectList.Add(targetObject);
            }
        }
        return (goalList, objectList);
    }

    public (List<Vector2>, List<GameObject>) findGoalsX(
        List<GameObject> targets,
        int rows,
        int cols
        )
    {
        List<Vector2> goalList = new List<Vector2>();
        List<GameObject> objectList = new List<GameObject>();
        foreach (GameObject targetObject in targets)
        {
            Vector2 targetPos = targetObject.GetComponent<FighterClass>().pos;
            for (int idx = MinRange; idx <= MaxRange; idx++)
            {
                goalList.Add(targetPos + new Vector2(idx, idx));
                objectList.Add(targetObject);
                goalList.Add(targetPos + new Vector2(-idx, idx));
                objectList.Add(targetObject);
                goalList.Add(targetPos + new Vector2(-idx, -idx));
                objectList.Add(targetObject);
                goalList.Add(targetPos + new Vector2(idx, -idx));
                objectList.Add(targetObject);
            }
        }
        return (goalList, objectList);
    }
}
