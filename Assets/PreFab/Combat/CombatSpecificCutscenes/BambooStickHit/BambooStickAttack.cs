using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BambooStickAttack : CutSceneClass
{
    public int amount;
    public FighterClass.attackType type;
    public FighterClass.statusEffects effects;
    public FighterClass.attackLocation location;
    public GameObject source;
    public GameObject damageTarget;

    public float heightOverHighestCharacter;
    public Vector3 endPosition;
    public float speed;

    private Vector3 startPosition;
    private float highestCharacterY;
    private float amp;
    private float d;
    private float height;

    private bool attemptMade = false;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.parent.position;
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
        if (transform.parent.GetComponent<Animator>() != null)
        {
            transform.parent.GetComponent<Animator>().SetTrigger("Jump");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(new Vector2(transform.parent.transform.position.x, transform.parent.transform.position.z), new Vector2(endPosition.x, endPosition.z)) <= speed * Time.deltaTime)
        {
            transform.parent.position = endPosition;
            if (transform.parent.GetComponent<Animator>() != null)
            {
                transform.parent.GetComponent<Animator>().SetTrigger("Stop");
            }
            damageTarget.GetComponent<FighterClass>().attackEffect(amount, type, effects, location, source);
            cutsceneDone();
        }

        //Move Horizontally
        transform.parent.position = Vector3.MoveTowards(transform.parent.position, new Vector3(endPosition.x, transform.parent.position.y, endPosition.z), speed * Time.deltaTime);
        //Update Vertical
        float distFromStart = Vector2.Distance(new Vector2(startPosition.x, startPosition.z), new Vector2(transform.parent.position.x, transform.parent.position.z));
        float newHeight = -amp * (distFromStart - d) * (distFromStart - d) + height;
        transform.parent.position = new Vector3(transform.parent.position.x, newHeight, transform.parent.position.z);

        if (Input.GetButtonDown("Fire1") && (attemptMade == false))
        {
            attemptMade = true;
            if(Vector2.Distance(new Vector2(transform.parent.position.x, transform.parent.position.z), new Vector2(endPosition.x, endPosition.z))<0.08*speed)
            {
                amount = amount * 2;
            }
        }
    }
}