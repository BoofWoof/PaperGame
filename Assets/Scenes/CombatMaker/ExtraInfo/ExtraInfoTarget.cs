using System;
using UnityEngine;

[Serializable]
public class ExtraInfoTarget
{
    public Vector2Int pos;
    //0 = Tile
    //1 = Character
    //2 = Object
    public int targetType;
}
