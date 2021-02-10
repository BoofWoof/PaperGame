using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpToLocation : CutSceneClass
{
    public float heightOverHighestCharacter = 2;
    public Vector3 endPosition;
    public float speed = 1f;
    private Vector3 startPosition;
    private float highestCharacterY;

    private float amp;
    private float d;
    private float height;
    // Start is called before the first frame update
    void Start()
    {
    }

    override public bool Activate()
    {
        startPosition = parent.transform.position;
        //if(startPosition.x > endPosition)
        float dist = Vector2.Distance(new Vector2(startPosition.x, startPosition.z), new Vector2(endPosition.x, endPosition.z));
        speed = dist * speed;
        float y2 = endPosition.y;
        float y1 = startPosition.y;

        if (y2 > y1)
        {
            highestCharacterY = y2;
        }
        else
        {
            highestCharacterY = y1;
        }

        height = highestCharacterY + heightOverHighestCharacter;

        float a = (y2 - y1);
        if (a == 0)
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
        if (parent.GetComponent<Animator>() != null)
        {
            parent.GetComponent<Animator>().SetTrigger("Jump");
        }
        active = true;
        return true;
    }

    // Update is called once per frame
    override public bool Update()
    {
        if (active)
        {
            //Move Horizontally
            parent.transform.position = Vector3.MoveTowards(parent.transform.position, new Vector3(endPosition.x, parent.transform.position.y, endPosition.z), speed * Time.deltaTime);
            //Update Vertical
            float distFromStart = Vector2.Distance(new Vector2(startPosition.x, startPosition.z), new Vector2(parent.transform.position.x, parent.transform.position.z));
            float newHeight = -amp * (distFromStart - d) * (distFromStart - d) + height;
            parent.transform.position = new Vector3(parent.transform.position.x, newHeight, parent.transform.position.z);

            if (Vector2.Distance(new Vector2(parent.transform.position.x, parent.transform.position.z), new Vector2(endPosition.x, endPosition.z)) <= speed * Time.deltaTime)
            {
                parent.transform.position = endPosition;
                if (parent.GetComponent<Animator>() != null)
                {
                    parent.GetComponent<Animator>().SetTrigger("Stop");
                }
                return true;
            }
        }
        return false;
    }
}
