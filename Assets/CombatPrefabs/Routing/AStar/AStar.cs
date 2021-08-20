using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class AStar : ScriptableObject
{
    public int finalGoalIdx;

    //Character To Move
    FighterClass characterInfo;

    //Info Grids
    AStarNode[,] routeMap;

    //Goal Info
    List<Vector2Int> goalCoordinates;

    /*
    private struct AStarNode
    {
        public float g;
        public float h;
        public float totalCost;
        public bool expanded;
        public Vector2 coordinates;
        public Vector2 parent;
        public FighterClass.CharacterPosition move;
    }
    */

    public (Vector2Int, FighterClass.CharacterPosition, bool) GetNextTile(
        FighterClass character,
        Vector2Int StartPosition,
        List<Vector2Int> goalCoordinates
        )
    {
        this.characterInfo = character;
        
        this.goalCoordinates = goalCoordinates;

        return NextMove(StartPosition);
    }

    public void Debug(
        FighterClass character,
        Vector2Int StartPosition,
        List<Vector2Int> goalCoordinates
        )
    {
        this.characterInfo = character;
        
        this.goalCoordinates = goalCoordinates;

        NextMove(StartPosition);

        for (int row = 0; row < CombatExecutor.mapShape.x; row++)
        {
            for (int col = 0; col < CombatExecutor.mapShape.y; col++)
            {
                if (!(routeMap[row, col] is null))
                {
                    GameObject debugText = new GameObject("debugText");
                    debugText.transform.position = CombatExecutor.blockGrid[row, col].transform.position + new Vector3(0, 0.5f, 0);
                    TextMeshPro tx = debugText.AddComponent<TextMeshPro>();
                    tx.text = routeMap[row, col].totalCost.ToString();
                    tx.fontSize = 10;
                    tx.verticalAlignment = VerticalAlignmentOptions.Middle;
                    tx.horizontalAlignment = HorizontalAlignmentOptions.Center;
                }
            }
        }
    }

    public (Vector2Int, FighterClass.CharacterPosition, bool) NextMove(Vector2Int startCoordinate)
    {
        routeMap = new AStarNode[CombatExecutor.mapShape.x, CombatExecutor.mapShape.y];
        List<AStarNode> costList = new List<AStarNode>();

        //Create and Expand first node.
        AStarNode newNode = createRootNode(startCoordinate);
        if(newNode.h == 0)
        {
            return (startCoordinate, FighterClass.CharacterPosition.Ground, true);
        }
        routeMap[(int)startCoordinate.x, (int)startCoordinate.y] = newNode;
        costList.Add(newNode);

        bool solved = false;
        int steps = 0;
        while (!solved)
        {
            steps++;
            //Debug.Log(steps);
            if (steps > 200)
            {
                //Debug.Log(costList.Count);
                break;
            }
            AStarNode bestNode = costList[0];
            if (bestNode.h == 0)
            {
                solved = true;
                return getNextMove(bestNode);
            }
            costList.RemoveAt(0);
            //Expand best node.
            List<AStarNode> newNodes = expandNode(bestNode);
            //Add nodes based on cost.
            foreach (AStarNode node in newNodes)
            {
                if (costList.Count == 0)
                {
                    costList.Add(node);
                } else if (node.totalCost >= costList[costList.Count-1].totalCost)
                {
                    costList.Add(node);
                } else
                {
                    for (int costIdx = 0; costIdx < costList.Count; costIdx++)
                    {
                        if (node.totalCost < costList[costIdx].totalCost)
                        {
                            costList.Insert(costIdx, node);
                            break;
                        }
                    }
                }
            }
            if(costList.Count == 0)
            {
                break;
            }
        }
        return (startCoordinate, FighterClass.CharacterPosition.Ground, false);
    }

    private (Vector2Int, FighterClass.CharacterPosition, bool) getNextMove(AStarNode bestNode)
    {
        AStarNode node = bestNode;
        AStarNode prevNode = null;
        while (node.parent != new Vector2Int(-1, -1))
        {
            prevNode = node;
            node = routeMap[(int)node.parent.x, (int)node.parent.y];
        }
        return (prevNode.coordinates, prevNode.move, false);
    }

    private List<AStarNode> expandNode(AStarNode bestNode)
    {
        bestNode.expanded = true;
        List<AStarNode> newNodes = new List<AStarNode>();
        Vector2Int coordinates = bestNode.coordinates;

        Vector2Int up = coordinates + new Vector2Int(0, 1);
        Vector2Int bigUp = coordinates + new Vector2Int(0, characterInfo.TileSize.y);
        if(checkIfExpand(coordinates, up))
        {
            newNodes.Add(createNewNode(bestNode, up));
        } else if (checkIfExpand(coordinates, bigUp))
        {
            newNodes.Add(createNewNode(bestNode, bigUp));
        }
        Vector2Int down = coordinates + new Vector2Int(0, -1);
        Vector2Int bigDown = coordinates + new Vector2Int(0, -characterInfo.TileSize.y);
        if (checkIfExpand(coordinates, down))
        {
            newNodes.Add(createNewNode(bestNode, down));
        }
        else if (checkIfExpand(coordinates, bigDown))
        {
            newNodes.Add(createNewNode(bestNode, bigDown));
        }
        Vector2Int right = coordinates + new Vector2Int(1, 0);
        Vector2Int bigRight = coordinates + new Vector2Int(characterInfo.TileSize.x, 0);
        if (checkIfExpand(coordinates, right))
        {
            newNodes.Add(createNewNode(bestNode, right));
        }
        else if (checkIfExpand(coordinates, bigRight))
        {
            newNodes.Add(createNewNode(bestNode, bigRight));
        }
        Vector2Int left = coordinates + new Vector2Int(-1, 0);
        Vector2Int bigLeft = coordinates + new Vector2Int(-characterInfo.TileSize.x, 0);
        if (checkIfExpand(coordinates, left))
        {
            newNodes.Add(createNewNode(bestNode, left));
        }
        else if (checkIfExpand(coordinates, bigLeft))
        {
            newNodes.Add(createNewNode(bestNode, bigLeft));
        }
        return newNodes;
    }

    private bool checkIfExpand(Vector2Int from, Vector2Int to)
    {
        List<Vector2Int> potentialTiles = characterInfo.PotentialGridOccupation(to);
        if (!BattleMapProcesses.isThisListOnGrid(potentialTiles)) return false;

        if (!CombatExecutor.LevelFloor(to, characterInfo.TileSize)) return false;

        if (!(routeMap[(int)to.x, (int)to.y] is null)) return false;

        if (!BattleMapProcesses.isTileEmpty(potentialTiles, characterInfo.gameObject)) return false;

        BlockTemplate blockInfo = CombatExecutor.blockGrid[(int)to.x, (int)to.y].GetComponent<BlockTemplate>();
        if (!((characterInfo.CanWalk && blockInfo.Walkable) ||
            (characterInfo.CanFly && blockInfo.Flyable) ||
            (characterInfo.CanSwim && blockInfo.Swimable)))
        {
            return false;
        }
        int heightDifference = CombatExecutor.gridHeight[(int)to.x, (int)to.y] - CombatExecutor.gridHeight[(int)from.x, (int)from.y];
        if (heightDifference > characterInfo.MaxJumpHeight)
        {
            return false;
        }
        return true;
    }

    private AStarNode createNewNode(AStarNode bestNode, Vector2Int newCoordinate)
    {
        AStarNode newNode = ScriptableObject.CreateInstance<AStarNode>();
        BlockTemplate blockInfo = CombatExecutor.blockGrid[(int)newCoordinate.x, (int)newCoordinate.y].GetComponent<BlockTemplate>();

        float cheapestMove = 10000;
        if (characterInfo.CanWalk)
        {
            if (blockInfo.WalkCost < cheapestMove)
            {
                cheapestMove = blockInfo.WalkCost;
            }
        }
        if (characterInfo.CanSwim)
        {
            if (blockInfo.SwimCost < cheapestMove)
            {
                cheapestMove = blockInfo.SwimCost;
            }
        }
        if (characterInfo.CanFly)
        {
            if (blockInfo.FlyCost < cheapestMove)
            {
                cheapestMove = blockInfo.FlyCost;
            }
        }
        
        newNode.g = bestNode.g + cheapestMove;
        float closestGoal = 10000;
        foreach (Vector2Int goal in goalCoordinates)
        {
            float distance = Mathf.Abs(goal.x - newCoordinate.x) + Mathf.Abs(goal.y - newCoordinate.y);
            if (distance < closestGoal)
            {
                closestGoal = distance;
            }
            foreach(Vector2Int extPos in characterInfo.PotentialGridOccupation(newCoordinate))
            {
                distance = Mathf.Abs(goal.x - extPos.x) + Mathf.Abs(goal.y - extPos.y);
                if (distance < closestGoal)
                {
                    closestGoal = distance;
                }

            }
        }

        newNode.h = closestGoal;
        newNode.totalCost = newNode.g + newNode.h;
        //Debug.Log(newNode.totalCost);
        newNode.expanded = false;
        newNode.coordinates = newCoordinate;
        newNode.parent = bestNode.coordinates;

        routeMap[(int)newCoordinate.x, (int)newCoordinate.y] = newNode;

        return newNode;
    }

    private AStarNode createRootNode(Vector2Int newCoordinate)
    {
        AStarNode newNode = ScriptableObject.CreateInstance<AStarNode>();
        BlockTemplate blockInfo = CombatExecutor.blockGrid[(int)newCoordinate.x, (int)newCoordinate.y].GetComponent<BlockTemplate>();

        float cheapestMove = 10000;
        if (characterInfo.CanWalk)
        {
            if (blockInfo.WalkCost < cheapestMove)
            {
                cheapestMove = blockInfo.WalkCost;
                newNode.move = FighterClass.CharacterPosition.Ground;
            }
        }
        if (characterInfo.CanSwim)
        {
            if (blockInfo.SwimCost < cheapestMove)
            {
                cheapestMove = blockInfo.SwimCost;
                newNode.move = FighterClass.CharacterPosition.Water;
            }
        }
        if (characterInfo.CanFly)
        {
            if (blockInfo.FlyCost < cheapestMove)
            {
                cheapestMove = blockInfo.FlyCost;
                newNode.move = FighterClass.CharacterPosition.Air;
            }
        }

        newNode.g = cheapestMove;
        float closestGoal = 10000;
        for (int goalIdx = 0; goalIdx < goalCoordinates.Count; goalIdx++)
        {
            Vector2Int goal = goalCoordinates[goalIdx];
            float distance = Mathf.Abs(goal.x - newCoordinate.x) + Mathf.Abs(goal.y - newCoordinate.y); //Mathf.Sqrt(Mathf.Pow((goal.x - newCoordinate.x), 2) + Mathf.Pow((goal.y - newCoordinate.y), 2));
            if (distance < closestGoal)
            {
                closestGoal = distance;
                if (distance == 0)
                {
                    finalGoalIdx = goalIdx;
                }
            }
            foreach (Vector2Int extPos in characterInfo.PotentialGridOccupation(newCoordinate))
            {
                distance = Mathf.Abs(goal.x - extPos.x) + Mathf.Abs(goal.y - extPos.y);
                if (distance < closestGoal)
                {
                    closestGoal = distance;
                    if (distance == 0)
                    {
                        finalGoalIdx = goalIdx;
                    }
                }

            }
        }

        newNode.h = closestGoal;
        newNode.totalCost = newNode.g + newNode.h;
        newNode.expanded = false;
        newNode.coordinates = newCoordinate;
        newNode.parent = new Vector2Int(-1, -1);
        return newNode;
    }
}
