using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HeightHighlighter : CutSceneClass
{
    public float heightChange;
    public List<Vector2Int> targetPos;

    private List<GameObject> targetObjects;

    private float time = 0;
    private float execution_time = 1f;
    private int phase = 0;

    override public bool Activate()
    {
        targetObjects = CombatExecutor.GetAllObjectExcept(targetPos);
        return true;
    }

    override public bool Deactivate()
    {
        phase += 1;
        return true;
    }

    // Update is called once per frame
    override public bool Update()
    {
        if(phase == 0)
        {
            for (int i = 0; i < targetObjects.Count; i++)
            {
                GameObject targetObject = targetObjects[i];
                if (targetObject != null) targetObject.transform.position -= new Vector3(0, heightChange * Time.deltaTime / execution_time, 0);
            }
            time += Time.deltaTime;
            if(time > execution_time)
            {
                time = 0;
                phase += 1;
            }
        }
        if (phase == 2)
        {
            for (int i = 0; i < targetObjects.Count; i++)
            {
                GameObject targetObject = targetObjects[i];
                if (targetObject != null) targetObject.transform.position += new Vector3(0, heightChange * Time.deltaTime / execution_time, 0);
            }
            time += Time.deltaTime;
            if (time > execution_time)
            {
                time = 0;
                phase += 1;
            }
        }
        if (phase == 3) {
            return true;
        }
        return false;
    }
}
