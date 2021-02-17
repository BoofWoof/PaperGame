using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode : ScriptableObject
{
    public float g;
    public float h;
    public float totalCost;
    public bool expanded;
    public Vector2 coordinates;
    public Vector2 parent;
    public FighterClass.CharacterPosition move;
}
