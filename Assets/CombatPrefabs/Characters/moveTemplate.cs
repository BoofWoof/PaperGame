using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public enum TargetQuantity
    {
        None,
        Single,
        Multiple,
        All,
        Random
    }

    public string name;
    public GameObject character;
    public List<GameObject> target;
    public int moveIndex;
    public TargetType targetType = TargetType.None;
    public TargetQuantity targetQuantity = TargetQuantity.None;
    public int targetCount = 1;
    public string combatDescription;

    public virtual void Activate(List<GameObject> targets)
    {
        
    }
}
