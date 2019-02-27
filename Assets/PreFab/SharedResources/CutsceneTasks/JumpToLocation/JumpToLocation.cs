using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpToLocation : CutSceneClass
{
    public float heightOverHighestCharacter;
    public Vector3 endPosition;
    public float speed;
    private Vector3 startPosition;
    private float highestCharacterY;

    private float amp;
    private float d;
    private float height;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.parent.position;
        //if(startPosition.x > endPosition)
        float dist = Vector2.Distance(new Vector2(startPosition.x, startPosition.z), new Vector2(endPosition.x, endPosition.z));
        speed = dist*speed;
        float y2 = endPosition.y;
        float y1 = startPosition.y;

        if(y2 > y1)
        {
            highestCharacterY = y2;
        } else
        {
            highestCharacterY = y1;
        }

        height = highestCharacterY + heightOverHighestCharacter;

        float a = (y2 - y1);
        if(a == 0)
        {
            a = 0.00001f;
        }
        float b = (2 * dist * y1 - 2 * dist * height);
        float c = (-dist * dist * y1 + dist * dist * height);

        d = (-b - Mathf.Sqrt(b * b - 4 * a * c)) / (2 * a);

        amp = -(y1 - height) / (d * d);
        

        //THIS IS JUST SOME HELPFUL TEST CODE TO MAKE SURE THE EQUATION WORKS
        /*
        float x1 = 0.0f;
        float x2 = 3.0f;
        float y1 = 2.0f;
        float y2 = 5.0f;
        float height = 7.0f;

        float a = (y2 - y1);
        float b = (2 * (x2 - x1) * y1 - 2 * (x2 - x1) * height);
        float c = (-(x2 - x1) * (x2 - x1) * y1 + (x2 - x1) * (x2 - x1) * height);

        print(a);
        print(b);
        print(c);

        float d = (-b - Mathf.Sqrt(b*b-4*a*c)) / (2 * a);

        print(d);

        float amp = -(y1 - height) / (x1 * x1 - 2 * x1 * d + d * d);

        */
        if (transform.parent.GetComponent<Animator>() != null)
        {
            transform.parent.GetComponent<Animator>().SetTrigger("Jump");
        }
    }

    // Update is called once per frame
    void Update()
    {

        //Move Horizontally
        transform.parent.position = Vector3.MoveTowards(transform.parent.position, new Vector3(endPosition.x, transform.parent.position.y, endPosition.z), speed * Time.deltaTime);
        //Update Vertical
        float distFromStart = Vector2.Distance(new Vector2(startPosition.x, startPosition.z), new Vector2(transform.parent.position.x, transform.parent.position.z));
        float newHeight = -amp * (distFromStart - d) * (distFromStart - d) + height;
        transform.parent.position = new Vector3(transform.parent.position.x, newHeight, transform.parent.position.z);

        if (Vector2.Distance(new Vector2(transform.parent.transform.position.x, transform.parent.transform.position.z), new Vector2(endPosition.x, endPosition.z)) <= speed * Time.deltaTime)
        {
            transform.parent.position = endPosition;
            if (transform.parent.GetComponent<Animator>() != null)
            {
                transform.parent.GetComponent<Animator>().SetTrigger("Stop");
            }
            cutsceneDone();
        }
    }
}
