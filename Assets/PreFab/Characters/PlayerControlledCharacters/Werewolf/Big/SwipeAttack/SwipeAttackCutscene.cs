using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeAttackCutscene : CutSceneClass
{
    public int amount;
    public FighterClass.attackType type;
    public FighterClass.statusEffects effects;
    public FighterClass.attackLocation location;
    public GameObject source;
    public GameObject damageTarget;

    private int attackPhase = 0;
    private float timeCount = 0;
    private bool attemptMade = false;

    private Vector3 positionDistance;
    private Vector3 positionHome;
    private float speed = 12.0f;

    private void Start()
    {
        positionHome = transform.parent.GetComponent<FighterClass>().HomePosition;
        positionDistance = damageTarget.transform.position - transform.parent.position;
    }

    private void Update()
    {
        if (attackPhase == 0)
        {
            if (transform.parent.GetComponent<Animator>() != null)
            {
                transform.parent.GetComponent<Animator>().SetTrigger("AttackSlicePrep");
            }
            attackPhase++;
        }
        if (attackPhase == 1)
        {
            timeCount = timeCount + Time.deltaTime;
            if (Input.GetButtonUp("Fire1") && attemptMade == false)
            {
                attemptMade = true;
                if (timeCount > 1.75)
                {
                    amount = amount * 2;
                }
                if (transform.parent.GetComponent<Animator>() != null)
                {
                    print("hi");
                    transform.parent.GetComponent<Animator>().SetTrigger("AttackSlice");
                }
                attackPhase++;
            }
            if (timeCount >= 3)
            {
                if (transform.parent.GetComponent<Animator>() != null)
                {
                    print("hi");
                    transform.parent.GetComponent<Animator>().SetTrigger("AttackSlice");
                }
                attackPhase++;
            }
        }
        else if (attackPhase == 2)
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, damageTarget.transform.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.parent.position, damageTarget.transform.position) < speed * Time.deltaTime)
            {
                transform.parent.position = damageTarget.transform.position;
                attackPhase++;
            }
        }
        else if (attackPhase == 3)
        {
            damageTarget.GetComponent<FighterClass>().attackEffect(amount, type, effects, location, source);
            attackPhase++;
        }
        else if (attackPhase == 4)
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, transform.parent.position+positionDistance, speed * Time.deltaTime);
            if (transform.parent.position.x > 10)
            {
                transform.parent.position = new Vector3(-10, positionHome.y, positionHome.z);
                attackPhase++;
            }
        }
        else if (attackPhase == 5)
        {
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, positionHome, speed * Time.deltaTime);
            if (Vector3.Distance(transform.parent.position, positionHome) < speed * Time.deltaTime)
            {
                if (transform.parent.GetComponent<Animator>() != null)
                {
                    transform.parent.GetComponent<Animator>().SetTrigger("Stop");
                }
                transform.parent.position = positionHome;
                attackPhase++;
            }
        }
        else if (attackPhase == 6)
        {
            cutsceneDone();
        }
    }
}
