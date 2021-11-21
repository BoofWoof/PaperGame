using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneClass : ScriptableObject
{
    public bool active = false;
    public GameObject parent;
    internal FighterClass target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Return true if it needs to get called again 
    public virtual bool Activate()
    {
        active = true;
        return false;
    }

    // Return true if it needs to go through deactivation work.
    public virtual bool Deactivate()
    {
        return false;
    }

    // Return True When Done
    public virtual bool Update()
    {
        return false;
    }

}
