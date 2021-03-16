using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

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
        X,
        None
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
    [Header("Attack Indicator")]
    public Material rangeIndicator;
    private List<GameObject> decalProjectors = new List<GameObject>();

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
                        if (isThisOnTheGrid(goal, rows, cols) && !goalList.Contains(goal))
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
                        if (isThisOnTheGrid(goal, rows, cols) && !goalList.Contains(goal))
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
                Vector2 newGoalVector;
                newGoalVector = targetPos + new Vector2(idx, 0);
                if (isThisOnTheGrid(newGoalVector, rows, cols) && !goalList.Contains(newGoalVector))
                {
                    goalList.Add(newGoalVector);
                    objectList.Add(targetObject);
                }
                newGoalVector = targetPos + new Vector2(-idx, 0);
                if (isThisOnTheGrid(newGoalVector, rows, cols) && !goalList.Contains(newGoalVector))
                {
                    goalList.Add(newGoalVector);
                    objectList.Add(targetObject);
                }
                newGoalVector = targetPos + new Vector2(0, idx);
                if (isThisOnTheGrid(newGoalVector, rows, cols) && !goalList.Contains(newGoalVector))
                {
                    goalList.Add(newGoalVector);
                    objectList.Add(targetObject);
                }
                newGoalVector = targetPos + new Vector2(0, -idx);
                if (isThisOnTheGrid(newGoalVector, rows, cols) && !goalList.Contains(newGoalVector))
                {
                    goalList.Add(newGoalVector);
                    objectList.Add(targetObject);
                }
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
                Vector2 newGoalVector;
                newGoalVector = targetPos + new Vector2(idx, idx);
                if (isThisOnTheGrid(newGoalVector, rows, cols) && !goalList.Contains(newGoalVector))
                {
                    goalList.Add(newGoalVector);
                    objectList.Add(targetObject);
                }
                newGoalVector = targetPos + new Vector2(-idx, -idx);
                if (isThisOnTheGrid(newGoalVector, rows, cols) && !goalList.Contains(newGoalVector))
                {
                    goalList.Add(newGoalVector);
                    objectList.Add(targetObject);
                }
                newGoalVector = targetPos + new Vector2(-idx, idx);
                if (isThisOnTheGrid(newGoalVector, rows, cols) && !goalList.Contains(newGoalVector))
                {
                    goalList.Add(newGoalVector);
                    objectList.Add(targetObject);
                }
                newGoalVector = targetPos + new Vector2(idx, -idx);
                if (isThisOnTheGrid(newGoalVector, rows, cols) && !goalList.Contains(newGoalVector))
                {
                    goalList.Add(newGoalVector);
                    objectList.Add(targetObject);
                }
            }
        }
        return (goalList, objectList);
    }

    private bool isThisOnTheGrid(Vector2 pos, int rows, int cols)
    {
        if (pos.x >= 0 && pos.x < rows && pos.y >= 0 && pos.y < cols)
        {
            return true;
        }
        return false;
    }

    public virtual List<GameObject> targetFilter(List<GameObject> potentialTargets)
    {
        if (targetShape == TargetShape.Square)
        {
            return squareFilter(
                potentialTargets
                );
        }
        if (targetShape == TargetShape.Diamond)
        {
            return diamondFilter(
                potentialTargets
                );
        }
        if (targetShape == TargetShape.Cross)
        {
            return crossFilter(
                potentialTargets
                );
        }
        if (targetShape == TargetShape.X)
        {
            return xFilter(
                potentialTargets
                );
        }

        return potentialTargets;
    }

    private List<GameObject> xFilter(List<GameObject> targets)
    {
        for (int idx = targets.Count - 1; idx >= 0; idx--)
        {
            Vector2 selfPos = character.GetComponent<FighterClass>().pos;
            Vector2 pos = (targets[idx].GetComponent<FighterClass>().pos - selfPos);
            if (Mathf.Abs(pos.x) != Mathf.Abs(pos.y))
            {
                targets.RemoveAt(idx);
                continue;
            }
            if (Mathf.Abs(pos.x) > MaxRange ||
                Mathf.Abs(pos.y) > MaxRange ||
                Mathf.Abs(pos.x) < MinRange ||
                Mathf.Abs(pos.y) < MinRange)
            {
                targets.RemoveAt(idx);
            }
        }
        return targets;
    }

    private List<GameObject> crossFilter(List<GameObject> targets)
    {
        for (int idx = targets.Count - 1; idx >= 0; idx--)
        {
            Vector2 selfPos = character.GetComponent<FighterClass>().pos;
            Vector2 pos = (targets[idx].GetComponent<FighterClass>().pos - selfPos);
            if(pos.x != 0 && pos.y != 0)
            {
                targets.RemoveAt(idx);
                continue;
            }
            if (Mathf.Abs(pos.x) > MaxRange ||
                Mathf.Abs(pos.y) > MaxRange ||
                Mathf.Abs(pos.x) < MinRange ||
                Mathf.Abs(pos.y) < MinRange)
            {
                targets.RemoveAt(idx);
            }
        }
        return targets;
    }

    private List<GameObject> diamondFilter(List<GameObject> targets)
    {
        for (int idx = targets.Count - 1; idx >= 0; idx--)
        {
            Vector2 selfPos = character.GetComponent<FighterClass>().pos;
            Vector2 pos = (targets[idx].GetComponent<FighterClass>().pos - selfPos);
            if (Mathf.Abs(pos.x) + Mathf.Abs(pos.y) > MaxRange ||
                Mathf.Abs(pos.x) + Mathf.Abs(pos.y) < MinRange)
            {
                targets.RemoveAt(idx);
            }
        }
        return targets;
    }

    private List<GameObject> squareFilter(List<GameObject> targets){
        for(int idx = targets.Count-1; idx >= 0; idx--)
        {
            Vector2 selfPos = character.GetComponent<FighterClass>().pos;
            Vector2 pos = (targets[idx].GetComponent<FighterClass>().pos - selfPos);
            if (Mathf.Abs(pos.x) > MaxRange ||
                Mathf.Abs(pos.y) > MaxRange ||
                Mathf.Abs(pos.x) < MinRange ||
                Mathf.Abs(pos.y) < MinRange)
            {
                targets.RemoveAt(idx);
            }
        }
        return targets;
    }

    public virtual void displayRange()
    {
        CombatExecutor ce = GameDataTracker.combatExecutor;
        GameObject[,] blockGrid = ce.blockGrid;
        int rows = ce.rows;
        int cols = ce.cols;
        if (rangeIndicator != null)
        {
            List<GameObject> characterTarget = new List<GameObject>();
            characterTarget.Add(character);
            List<Vector2> possibleTargets;
            if (targetShape == TargetShape.Square)
            {
                possibleTargets = findGoalsSquare(
                    characterTarget,
                    rows,
                    cols
                    ).Item1;
            }
            else if (targetShape == TargetShape.Diamond)
            {
                possibleTargets = findGoalsDiamond(
                    characterTarget,
                    rows,
                    cols
                    ).Item1;
            }
            else if (targetShape == TargetShape.Cross)
            {
                possibleTargets = findGoalsCross(
                    characterTarget,
                    rows,
                    cols
                    ).Item1;
            }
            else if (targetShape == TargetShape.X)
            {
                possibleTargets = findGoalsX(
                    characterTarget,
                    rows,
                    cols
                    ).Item1;
            }
            else
            {
                possibleTargets = new List<Vector2>();
            }
            foreach(Vector2 possibleTarget in possibleTargets)
            {
                GameObject newProjector = new GameObject("Projector");
                DecalProjector projector = newProjector.AddComponent<DecalProjector>();
                projector.material = rangeIndicator;
                newProjector.transform.localRotation = Quaternion.Euler(90, 0, 0);
                newProjector.transform.position = blockGrid[(int)possibleTarget.x, (int)possibleTarget.y].transform.position;
                decalProjectors.Add(newProjector);
            }
        }
    }

    public virtual void hideRange()
    {
        for (int idx = decalProjectors.Count - 1; idx >= 0; idx--)
        {
            Destroy(decalProjectors[idx]);
            decalProjectors.RemoveAt(idx);
        }
    }
}
