using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode : ScriptableObject
{
    public float g;
    public float h;
    public float totalCost;
    public bool expanded;
    public Vector2Int coordinates;
    public Vector2Int parent;
    public FighterClass.CharacterPosition move;
}
