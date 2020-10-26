using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;

public class MoveToPosNode : NodeTemplate
{
    public string TargetObject;
    public string ReferenceObject;
    public Vector3 PosOffset;
    public bool Wait;
}
