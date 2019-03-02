using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToLocation : CutSceneClass
{
    public Vector3 endPosition;
    public float speed;
    // Start is called before the first frame update
    private void Start()
    {
        if (transform.parent.GetComponent<Animator>() != null)
        {
            transform.parent.GetComponent<Animator>().SetTrigger("Go");
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.parent.transform.position, endPosition) < speed * Time.deltaTime)
        {
            transform.parent.transform.position = endPosition;
            if (transform.parent.GetComponent<Animator>() != null)
            {
                transform.parent.GetComponent<Animator>().SetTrigger("Stop");
            }
            cutsceneDone();
        }
        transform.parent.transform.position = Vector3.MoveTowards(transform.parent.transform.position, endPosition, speed * Time.deltaTime);
    }
}
