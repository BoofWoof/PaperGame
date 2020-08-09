using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneClass : ScriptableObject
{
    public bool active = false;
    public GameObject parent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public virtual bool Activate()
    {
        active = true;
        return false;
    }

    public virtual bool Update()
    {
        return false;
    }
}
