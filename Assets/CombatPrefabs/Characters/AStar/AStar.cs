using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : ScriptableObject
{
    //Character To Move
    FighterClass characterInfo;

    //Info Grids
    GameObject[,] blockGrid;
    GameObject[,] characterGrid;
    GameObject[,] objectGrid;
    int[,] tileHeight;
    AStarNode[,] routeMap;

    //Goal Info
    List<Vector2> goalCoordinates;

    int rows;
    int cols;

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

    public (Vector2, FighterClass.CharacterPosition) GetNextTile(
        FighterClass character,
        GameObject[,] blockGrid,
        GameObject[,] characterGrid,
        GameObject[,] objectGrid,
        int[,] tileHeight,
        Vector2 StartPosition,
        List<Vector2> goalCoordinates
        )
    {
        this.characterInfo = character;

        this.blockGrid = blockGrid;
        this.characterGrid = characterGrid;
        this.objectGrid = objectGrid;
        this.tileHeight = tileHeight;
        this.goalCoordinates = goalCoordinates;

        this.rows = blockGrid.GetLength(0);
        this.cols = blockGrid.GetLength(1);

        return NextMove(StartPosition);
    }
    
    public (Vector2, FighterClass.CharacterPosition) NextMove(Vector2 startCoordinate)
    {
        routeMap = new AStarNode[rows, cols];
        List<AStarNode> costList = new List<AStarNode>();

        //Create and Expand first node.
        AStarNode newNode = createRootNode(startCoordinate);
        routeMap[(int)startCoordinate.x, (int)startCoordinate.y] = newNode;
        costList.Add(newNode);

        bool solved = false;
        int steps = 0;
        while (!solved)
        {
            steps++;
            Debug.Log(steps);
            if (steps > 100)
            {
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
                addNodeToRouteMap(node);
                if (costList.Count == 0)
                {
                    costList.Add(node);
                } else if (node.totalCost > costList[costList.Count-1].totalCost)
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
        }
        return (Vector2.zero, FighterClass.CharacterPosition.Ground);
    }

    private (Vector2, FighterClass.CharacterPosition) getNextMove(AStarNode bestNode)
    {
        AStarNode node = bestNode;
        AStarNode prevNode = new AStarNode();
        while (node.parent != new Vector2(-1, -1))
        {
            prevNode = node;
            node = routeMap[(int)node.parent.x, (int)node.parent.y];
        }
        return (prevNode.coordinates, prevNode.move);
    }

    private void addNodeToRouteMap(AStarNode node)
    {
        Vector2 coordinate = node.coordinates;
        if (routeMap[(int)coordinate.x, (int)coordinate.y].g == 0)
        {
            routeMap[(int)coordinate.x, (int)coordinate.y] = node;
        } else
        {
            if(node.totalCost < routeMap[(int)coordinate.x, (int)coordinate.y].totalCost)
            {
                routeMap[(int)coordinate.x, (int)coordinate.y] = node;
            }
        }
    }

    private List<AStarNode> expandNode(AStarNode bestNode)
    {
        List<AStarNode> newNodes = new List<AStarNode>();
        Vector2 coordinates = bestNode.coordinates;

        Vector2 up = coordinates + new Vector2(1, 0);
        if(checkIfExpand(coordinates, up))
        {
            newNodes.Add(createNewNode(bestNode, up));
        }
        Vector2 down = coordinates + new Vector2(-1, 0);
        if (checkIfExpand(coordinates, down))
        {
            newNodes.Add(createNewNode(bestNode, down));
        }
        Vector2 right = coordinates + new Vector2(0, 1);
        if (checkIfExpand(coordinates, right))
        {
            newNodes.Add(createNewNode(bestNode, right));
        }
        Vector2 left = coordinates + new Vector2(0, -1);
        if (checkIfExpand(coordinates, left))
        {
            newNodes.Add(createNewNode(bestNode, left));
        }
        return newNodes;
    }

    private AStarNode createNewNode(AStarNode bestNode, Vector2 newCoordinate)
    {
        bestNode.expanded = true;

        AStarNode newNode = new AStarNode();
        BlockTemplate blockInfo = blockGrid[(int)newCoordinate.x, (int)newCoordinate.y].GetComponent<BlockTemplate>();

        float cheapestMove = 1000;
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
        float closestGoal = 1000;
        foreach (Vector2 goal in goalCoordinates)
        {
            float distance = Mathf.Sqrt(Mathf.Pow((goal.x - newCoordinate.x), 2) + Mathf.Pow((goal.y - newCoordinate.y), 2));
            if (distance < closestGoal)
            {
                closestGoal = distance;
            }
        }

        newNode.h = closestGoal;
        newNode.totalCost = newNode.g + newNode.h;
        newNode.expanded = false;
        newNode.coordinates = newCoordinate;
        newNode.parent = bestNode.coordinates;
        return newNode;
    }

    private AStarNode createRootNode(Vector2 newCoordinate)
    {
        AStarNode newNode = new AStarNode();
        BlockTemplate blockInfo = blockGrid[(int)newCoordinate.x, (int)newCoordinate.y].GetComponent<BlockTemplate>();

        float cheapestMove = 1000;
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
        float closestGoal = 1000;
        foreach (Vector2 goal in goalCoordinates)
        {
            float distance = Mathf.Sqrt(Mathf.Pow((goal.x - newCoordinate.x), 2) + Mathf.Pow((goal.y - newCoordinate.y), 2));
            if (distance < closestGoal)
            {
                closestGoal = distance;
            }
        }

        newNode.h = closestGoal;
        newNode.totalCost = newNode.g + newNode.h;
        newNode.expanded = false;
        newNode.coordinates = newCoordinate;
        newNode.parent = new Vector2(-1, -1);
        return newNode;
    }

    private bool checkIfExpand(Vector2 from, Vector2 to)
    {
        if(to.y < 0 || to.y > cols - 1 || to.x < 0 || to.x > rows - 1)
        {
            return false;
        }
        if(routeMap[(int)to.x, (int)to.y].expanded)
        {
            return false;
        }
        bool isGoal = false;
        foreach (Vector2 goal in goalCoordinates)
        {
            if(goal == to)
            {
                isGoal = true;
            }
        }
        if(!(characterGrid[(int)to.x, (int)to.y] is null) && !isGoal)
        {
            return false;
        }
        BlockTemplate blockInfo = blockGrid[(int)to.x, (int)to.y].GetComponent<BlockTemplate>();
        if (!((characterInfo.CanWalk && blockInfo.Walkable) ||
            (characterInfo.CanFly && blockInfo.Flyable) ||
            (characterInfo.CanSwim && blockInfo.Swimable)))
        {
            return false;
        }
        int heightDifference = tileHeight[(int)to.x, (int)to.y] - tileHeight[(int)from.x, (int)from.y];
        if (heightDifference > characterInfo.MaxJumpHeight)
        {
            return false;
        }
        return true;
    }
}
