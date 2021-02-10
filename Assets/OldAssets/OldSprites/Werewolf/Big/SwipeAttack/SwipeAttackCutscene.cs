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
    }

    override public bool Activate()
    {
        positionHome = parent.GetComponent<FighterClass>().HomePosition;
        positionDistance = damageTarget.transform.position - parent.transform.position;
        active = true;
        return true;
    }

    override public bool Update()
    {
        if (active)
        {
            if (attackPhase == 0)
            {
                if (parent.GetComponent<Animator>() != null)
                {
                    parent.GetComponent<Animator>().SetTrigger("AttackSlicePrep");
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
                    if (parent.GetComponent<Animator>() != null)
                    {
                        parent.GetComponent<Animator>().SetTrigger("AttackSlice");
                    }
                    attackPhase++;
                }
                if (timeCount >= 3)
                {
                    if (parent.GetComponent<Animator>() != null)
                    {
                        parent.GetComponent<Animator>().SetTrigger("AttackSlice");
                    }
                    attackPhase++;
                }
            }
            else if (attackPhase == 2)
            {
                parent.transform.position = Vector3.MoveTowards(parent.transform.position, damageTarget.transform.position, speed * Time.deltaTime);
                if (Vector3.Distance(parent.transform.position, damageTarget.transform.position) < speed * Time.deltaTime)
                {
                    parent.transform.position = damageTarget.transform.position;
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
                parent.transform.position = Vector3.MoveTowards(parent.transform.position, parent.transform.position +positionDistance, speed * Time.deltaTime);
                if (parent.transform.position.x > 10)
                {
                    parent.transform.position = new Vector3(-10, positionHome.y, positionHome.z);
                    attackPhase++;
                }
            }
            else if (attackPhase == 5)
            {
                parent.transform.position = Vector3.MoveTowards(parent.transform.position, positionHome, speed * Time.deltaTime);
                if (Vector3.Distance(parent.transform.position, positionHome) < speed * Time.deltaTime)
                {
                    if (parent.GetComponent<Animator>() != null)
                    {
                        parent.GetComponent<Animator>().SetTrigger("Stop");
                    }
                    parent.transform.position = positionHome;
                    attackPhase++;
                }
            }
            else if (attackPhase == 6)
            {
                return true;
            }

        }
        return false;
    }
}
