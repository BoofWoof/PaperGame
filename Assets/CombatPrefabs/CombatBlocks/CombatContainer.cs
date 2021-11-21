using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CombatContainer : ScriptableObject
{
    public int[] blockGrid;
    public int[] characterGrid;
    public int[] objectGrid;
    public int[] gridHeight;
    public Vector2Int mapShape;
    public bool puzzleMode = false;
    public bool doublePuzzleMode = false;
    public bool turnTie = false;
}
