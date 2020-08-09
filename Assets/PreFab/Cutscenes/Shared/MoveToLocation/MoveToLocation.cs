using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLocation : CutSceneClass
{
    public Vector3 endPosition;
    public float speed = 3;
    // Start is called before the first frame update
    private void Start()
    {
    }

    override public bool Activate()
    {
        if (parent.GetComponent<Animator>() != null)
        {
            parent.GetComponent<Animator>().SetTrigger("Go");
        }
        active = true;
        return true;
    }

    // Update is called once per frame
    override public bool Update()
    {
        if (active){
            if (Vector3.Distance(parent.transform.position, endPosition) < speed * Time.deltaTime)
            {
                parent.transform.position = endPosition;
                if (parent.GetComponent<Animator>() != null)
                {
                    parent.GetComponent<Animator>().SetTrigger("Stop");
                }
                return true;
            }
            parent.transform.position = Vector3.MoveTowards(parent.transform.position, endPosition, speed * Time.deltaTime);
        }
        return false;
    }
}
