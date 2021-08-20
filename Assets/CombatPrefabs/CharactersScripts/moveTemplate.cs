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

    public (List<Vector2Int>, List<GameObject>) findGoals(
        List<GameObject> targets
        )
    {
        //There is some inefficiency here.
        //Some goals can be placed out of bounds.
        //+ and x can also have duplicated goals.
        //MAKE THIS INTO A DICTIONARY PROBABLY
        if (targetShape == TargetShape.Square)
        {
            return findGoalsSquare(
                targets
                );
        }
        if (targetShape == TargetShape.Diamond)
        {
            return findGoalsDiamond(
                targets
                );
        }
        if (targetShape == TargetShape.Cross)
        {
            return findGoalsCross(
                targets
                );
        }
        if (targetShape == TargetShape.X)
        {
            return findGoalsX(
                targets
                );
        }
        return (new List<Vector2Int>(), new List<GameObject>());
    }

    public (List<Vector2Int>, List<GameObject>) findGoalsSquare(
        List<GameObject> targets
        )
    {
        List<Vector2Int> goalList = new List<Vector2Int>();
        List<GameObject> objectList = new List<GameObject>();
        foreach (GameObject targetObject in targets)
        {
            List<Vector2Int> allTargetPos = new List<Vector2Int>();
            allTargetPos.Add(targetObject.GetComponent<FighterClass>().pos);
            allTargetPos.AddRange(targetObject.GetComponent<FighterClass>().extra_pos);
            foreach(Vector2Int targetPos in allTargetPos)
            {
                for (int row = -MaxRange; row <= MaxRange + character.GetComponent<GridObject>().TileSize.x; row++)
                {
                    for (int col = -MaxRange; col <= MaxRange + character.GetComponent<GridObject>().TileSize.y; col++)
                    {
                        if (Mathf.Abs(row) >= MinRange || Mathf.Abs(col) >= MinRange)
                        {
                            Vector2Int goal = targetPos + new Vector2Int(row, col);
                            if (BattleMapProcesses.isThisOnTheGrid(goal) && !goalList.Contains(goal))
                            {
                                goalList.Add(goal);
                                objectList.Add(targetObject);
                            }
                        }
                    }
                }
            }
        }
        return (goalList, objectList);
    }

    public (List<Vector2Int>, List<GameObject>) findGoalsDiamond(
        List<GameObject> targets
        )
    {
        List<Vector2Int> goalList = new List<Vector2Int>();
        List<GameObject> objectList = new List<GameObject>();
        foreach (GameObject targetObject in targets)
        {
            List<Vector2Int> allTargetPos = new List<Vector2Int>();
            allTargetPos.Add(targetObject.GetComponent<FighterClass>().pos);
            allTargetPos.AddRange(targetObject.GetComponent<FighterClass>().extra_pos);
            foreach (Vector2Int targetPos in allTargetPos)
            {
                for (int row = -MaxRange; row <= MaxRange + character.GetComponent<GridObject>().TileSize.x; row++)
                {
                    for (int col = -MaxRange + Mathf.Abs(row); col <= MaxRange - Mathf.Abs(row); col++)
                    {
                        if (Mathf.Abs(row) + Mathf.Abs(col) >= MinRange)
                        {
                            Vector2Int goal = targetPos + new Vector2Int(row, col);
                            if (BattleMapProcesses.isThisOnTheGrid(goal) && !goalList.Contains(goal))
                            {
                                goalList.Add(goal);
                                objectList.Add(targetObject);
                            }
                        }
                    }
                }
            }
        }
        return (goalList, objectList);
    }

    public (List<Vector2Int>, List<GameObject>) findGoalsCross(
        List<GameObject> targets
        )
    {
        List<Vector2Int> goalList = new List<Vector2Int>();
        List<GameObject> objectList = new List<GameObject>();
        foreach (GameObject targetObject in targets)
        {
            List<Vector2Int> allTargetPos = new List<Vector2Int>();
            allTargetPos.Add(targetObject.GetComponent<FighterClass>().pos);
            allTargetPos.AddRange(targetObject.GetComponent<FighterClass>().extra_pos);
            foreach (Vector2Int targetPos in allTargetPos)
            {
                for (int idx = MinRange; idx <= MaxRange; idx++)
                {
                    Vector2Int newGoalVector;
                    newGoalVector = targetPos + new Vector2Int(idx, 0);
                    if (BattleMapProcesses.isThisOnTheGrid(newGoalVector) && !goalList.Contains(newGoalVector))
                    {
                        goalList.Add(newGoalVector);
                        objectList.Add(targetObject);
                    }
                    newGoalVector = targetPos + new Vector2Int(-idx, 0);
                    if (BattleMapProcesses.isThisOnTheGrid(newGoalVector) && !goalList.Contains(newGoalVector))
                    {
                        goalList.Add(newGoalVector);
                        objectList.Add(targetObject);
                    }
                    newGoalVector = targetPos + new Vector2Int(0, idx);
                    if (BattleMapProcesses.isThisOnTheGrid(newGoalVector) && !goalList.Contains(newGoalVector))
                    {
                        goalList.Add(newGoalVector);
                        objectList.Add(targetObject);
                    }
                    newGoalVector = targetPos + new Vector2Int(0, -idx);
                    if (BattleMapProcesses.isThisOnTheGrid(newGoalVector) && !goalList.Contains(newGoalVector))
                    {
                        goalList.Add(newGoalVector);
                        objectList.Add(targetObject);
                    }
                }
            }
        }
        return (goalList, objectList);
    }

    public (List<Vector2Int>, List<GameObject>) findGoalsX(
        List<GameObject> targets
        )
    {
        List<Vector2Int> goalList = new List<Vector2Int>();
        List<GameObject> objectList = new List<GameObject>();
        foreach (GameObject targetObject in targets)
        {
            List<Vector2Int> allTargetPos = new List<Vector2Int>();
            allTargetPos.Add(targetObject.GetComponent<FighterClass>().pos);
            allTargetPos.AddRange(targetObject.GetComponent<FighterClass>().extra_pos);
            foreach (Vector2Int targetPos in allTargetPos)
            {
                for (int idx = MinRange; idx <= MaxRange; idx++)
                {
                    Vector2Int newGoalVector;
                    newGoalVector = targetPos + new Vector2Int(idx, idx);
                    if (BattleMapProcesses.isThisOnTheGrid(newGoalVector) && !goalList.Contains(newGoalVector))
                    {
                        goalList.Add(newGoalVector);
                        objectList.Add(targetObject);
                    }
                    newGoalVector = targetPos + new Vector2Int(-idx, -idx);
                    if (BattleMapProcesses.isThisOnTheGrid(newGoalVector) && !goalList.Contains(newGoalVector))
                    {
                        goalList.Add(newGoalVector);
                        objectList.Add(targetObject);
                    }
                    newGoalVector = targetPos + new Vector2Int(-idx, idx);
                    if (BattleMapProcesses.isThisOnTheGrid(newGoalVector) && !goalList.Contains(newGoalVector))
                    {
                        goalList.Add(newGoalVector);
                        objectList.Add(targetObject);
                    }
                    newGoalVector = targetPos + new Vector2Int(idx, -idx);
                    if (BattleMapProcesses.isThisOnTheGrid(newGoalVector) && !goalList.Contains(newGoalVector))
                    {
                        goalList.Add(newGoalVector);
                        objectList.Add(targetObject);
                    }
                }
            }
        }
        return (goalList, objectList);
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
            if (XFilter(targets[idx].GetComponent<GridObject>()))
            {
                targets.RemoveAt(idx);
            }
        }
        return targets;
    }
    private bool XFilter(GridObject targetObject)
    {
        Vector2 selfPos = character.GetComponent<GridObject>().pos;
        Vector2 pos = (targetObject.pos - selfPos);
        if (Mathf.Abs(pos.x) == Mathf.Abs(pos.y) &&
            Mathf.Abs(pos.x) + Mathf.Abs(pos.y) <= MaxRange &&
            Mathf.Abs(pos.x) + Mathf.Abs(pos.y) >= MinRange) return false;
        foreach (Vector2Int extPos in targetObject.extra_pos)
        {
            pos = (extPos - selfPos);
            if ((pos.x == 0 || pos.y == 0) &&
            Mathf.Abs(pos.x) + Mathf.Abs(pos.y) <= MaxRange &&
            Mathf.Abs(pos.x) + Mathf.Abs(pos.y) >= MinRange) return false;
        }
        return true;
    }

    private List<GameObject> crossFilter(List<GameObject> targets)
    {
        for (int idx = targets.Count - 1; idx >= 0; idx--)
        {
            if (CFilter(targets[idx].GetComponent<GridObject>()))
            {
                targets.RemoveAt(idx);
            }
        }
        return targets;
    }
    private bool CFilter(GridObject targetObject)
    {
        Vector2 selfPos = character.GetComponent<GridObject>().pos;
        Vector2 pos = (targetObject.pos - selfPos);
        if ((pos.x == 0 || pos.y == 0) &&
            Mathf.Abs(pos.x) + Mathf.Abs(pos.y) <= MaxRange &&
            Mathf.Abs(pos.x) + Mathf.Abs(pos.y) >= MinRange) return false;
        foreach (Vector2Int extPos in targetObject.extra_pos)
        {
            pos = (extPos - selfPos);
            if ((pos.x == 0 || pos.y == 0) &&
            Mathf.Abs(pos.x) + Mathf.Abs(pos.y) <= MaxRange &&
            Mathf.Abs(pos.x) + Mathf.Abs(pos.y) >= MinRange) return false;
        }
        return true;
    }

    private List<GameObject> diamondFilter(List<GameObject> targets)
    {
        for (int idx = targets.Count - 1; idx >= 0; idx--)
        {
            if (DFilter(targets[idx].GetComponent<GridObject>()))
            {
                targets.RemoveAt(idx);
            }
        }
        return targets;
    }
    private bool DFilter(GridObject targetObject)
    {
        Vector2 selfPos = character.GetComponent<GridObject>().pos;
        Vector2 pos = (targetObject.pos - selfPos);
        if (Mathf.Abs(pos.x) + Mathf.Abs(pos.y) <= MaxRange &&
            Mathf.Abs(pos.x) + Mathf.Abs(pos.y) >= MinRange) return false;
        foreach(Vector2Int extPos in targetObject.extra_pos)
        {
            pos = (extPos - selfPos);
            if (Mathf.Abs(pos.x) + Mathf.Abs(pos.y) <= MaxRange &&
                Mathf.Abs(pos.x) + Mathf.Abs(pos.y) >= MinRange) return false;
        }
        return true;
    }

    private List<GameObject> squareFilter(List<GameObject> targets){
        for(int idx = targets.Count-1; idx >= 0; idx--)
        {
            Vector2 selfPos = character.GetComponent<FighterClass>().pos;
            Vector2 pos = (targets[idx].GetComponent<FighterClass>().pos - selfPos);
            if (SFilter(targets[idx].GetComponent<GridObject>()))
            {
                targets.RemoveAt(idx);
            }
        }
        return targets;
    }
    private bool SFilter(GridObject targetObject)
    {
        Vector2 selfPos = character.GetComponent<GridObject>().pos;
        Vector2 pos = (targetObject.pos - selfPos);
        if (Mathf.Abs(pos.x) <= MaxRange &&
                Mathf.Abs(pos.y) <= MaxRange &&
                Mathf.Abs(pos.x) >= MinRange &&
                Mathf.Abs(pos.y) >= MinRange) return false;
        foreach (Vector2Int extPos in targetObject.extra_pos)
        {
            pos = (extPos - selfPos);
            if (Mathf.Abs(pos.x) <= MaxRange &&
                Mathf.Abs(pos.y) <= MaxRange &&
                Mathf.Abs(pos.x) >= MinRange &&
                Mathf.Abs(pos.y) >= MinRange) return false;
        }
        return true;
    }

    public virtual void displayRange()
    {
        CombatExecutor ce = GameDataTracker.combatExecutor;
        GameObject[,] blockGrid = CombatExecutor.blockGrid;
        Vector2Int mapShape = CombatExecutor.mapShape;
        if (rangeIndicator != null)
        {
            List<GameObject> characterTarget = new List<GameObject>();
            characterTarget.Add(character);
            List<Vector2Int> possibleTargets;
            if (targetShape == TargetShape.Square)
            {
                possibleTargets = findGoalsSquare(
                    characterTarget
                    ).Item1;
            }
            else if (targetShape == TargetShape.Diamond)
            {
                possibleTargets = findGoalsDiamond(
                    characterTarget
                    ).Item1;
            }
            else if (targetShape == TargetShape.Cross)
            {
                possibleTargets = findGoalsCross(
                    characterTarget
                    ).Item1;
            }
            else if (targetShape == TargetShape.X)
            {
                possibleTargets = findGoalsX(
                    characterTarget
                    ).Item1;
            }
            else
            {
                possibleTargets = new List<Vector2Int>();
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
