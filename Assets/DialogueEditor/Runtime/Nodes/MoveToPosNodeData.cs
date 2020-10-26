using System;
using UnityEngine;

[Serializable]
public class MoveToPosNodeData
{
    public string Guid;
    public string TargetObject;
    public string ReferenceObject;
    public Vector3 PosOffset;
    public bool Wait;
    public Vector2 Position;
}
