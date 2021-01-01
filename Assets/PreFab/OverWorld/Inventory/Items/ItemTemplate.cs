using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTemplate : ScriptableObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OverWorldUse()
    {
        Debug.Log("OverworldUseNotImplemented");
    }

    public virtual void CombatUse()
    {
        Debug.Log("CombatUseNotImplemented");
    }

    public virtual void CombatThrow()
    {
        Debug.Log("CombatThrowNotImplemented");
    }
}
