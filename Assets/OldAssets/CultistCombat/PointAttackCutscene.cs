using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAttackCutscene : CutSceneClass
{
    public int amount;
    public FighterClass.attackType type;
    public FighterClass.statusEffects effects;
    public FighterClass.attackLocation location;
    public GameObject source;

    private bool attemptMade = false;

    private GameObject camera;

    private int directionRand;
    // 0 Left
    // 1 Right
    // 2 Down
    private int attackStep = 0;

    private float angle;

    void Start()
    {
    }

    override public bool Activate()
    {
        camera = CombatController.gameControllerAccess.GetComponent<CombatController>().trackingCamera;
        directionRand = Random.Range(0, 3);
        angle = 180 * Mathf.Atan((source.transform.position.x - camera.transform.position.x) / (source.transform.position.z - camera.transform.position.z)) / Mathf.PI;
        active = true;
        return true;
    }

    // Update is called once per frame
    override public bool Update()
    {
        if (active)
        {
            if(attackStep == 0)
            {
                //float angle = Vector3.Angle(camera.transform.position, new Vector3(source.transform.position.x, camera.transform.position.y, source.transform.position.z));
                source.transform.rotation = Quaternion.RotateTowards(source.transform.rotation, Quaternion.Euler(0, angle-90, 0), 100 * Time.deltaTime);
                if ((source.transform.rotation.eulerAngles.y >= angle +270 - 0.1) && (source.transform.rotation.eulerAngles.y <= angle +270 + 0.1))
                {
                    if (directionRand == 0)
                    {
                        source.GetComponent<Animator>().SetTrigger("Left");
                    }
                    if (directionRand == 1)
                    {
                        source.GetComponent<Animator>().SetTrigger("Right");
                    }
                    if (directionRand == 2)
                    {
                        source.GetComponent<Animator>().SetTrigger("Down");
                    }
                    source.transform.rotation = Quaternion.Euler(0, angle - 90 + 180, 0);
                    attackStep++;
                }
            }
            else if(attackStep == 1)
            {
                source.transform.rotation = Quaternion.RotateTowards(source.transform.rotation, Quaternion.Euler(0, 0, 0), 100 * Time.deltaTime);
                if (Input.GetButton("Fire1") && (attemptMade == false)) {
                    float horiz = Input.GetAxis("Horizontal");
                    float vert = Input.GetAxis("Vertical");
                    attemptMade = true;
                    if ((directionRand == 0) && (horiz > .25))
                    {
                        amount = amount - 1;
                    }
                    if ((directionRand == 1) && (horiz < -.25))
                    {
                        amount = amount - 1;
                    }
                    if ((directionRand == 2) && (vert > .25))
                    {
                        amount = amount - 1;
                    }
                }
                if (source.transform.rotation.eulerAngles.y == 0)
                {
                    attackStep++;
                }
            }
            else if (attackStep == 2)
            {
                attackStep++;
            }
            else if (attackStep == 3)
            {
                attackStep++;
            }
            else if (attackStep == 4)
            {
                parent.GetComponent<FighterClass>().attackEffect(amount, type, effects, location, source);
                source.GetComponent<Animator>().SetTrigger("Stop");
                return true;
            }
        }
        return false;
    }
}
