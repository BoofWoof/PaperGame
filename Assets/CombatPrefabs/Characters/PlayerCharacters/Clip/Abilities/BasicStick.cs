using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicStick : moveTemplate
{
    GameObject target;

    public override void Activate(List<GameObject> targets)
    {
        base.Activate(targets);

        target = targets[0];

        BasicStickCutscene basicStick = ScriptableObject.CreateInstance<BasicStickCutscene>();
        basicStick.target = target;
        basicStick.power = character.GetComponent<FighterClass>().Power;
        CutsceneController.addCutsceneEvent(basicStick, character, true, GameDataTracker.gameModeOptions.Cutscene);
    }
}

public class BasicStickCutscene : CutSceneClass
{
    private int phase = 0;
    private float count = 0.0f;
    public int power;
    public GameObject target;
    override public bool Activate()
    {
        return true;
    }

    // Update is called once per frame
    override public bool Update()
    {
        if (phase == 0)
        {
            if (Input.GetAxis("Horizontal") < -0.3f)
            {
                parent.GetComponent<FighterClass>().animator.SetTrigger("HoldStickHit");
                phase++;
            }
        }
        if (phase == 1)
        {
            count += Time.deltaTime;
            Debug.Log(count);
            if (Input.GetAxis("Horizontal") > -0.1f || count >= 2.0f)
            {
                if (count > 1.5f && count < 2.0f)
                {
                    target.GetComponent<FighterClass>().postBufferAttackEffect(power*2, FighterClass.attackType.Normal, FighterClass.statusEffects.None, FighterClass.attackLocation.Ground, parent);
                } else
                {
                    target.GetComponent<FighterClass>().postBufferAttackEffect(power, FighterClass.attackType.Normal, FighterClass.statusEffects.None, FighterClass.attackLocation.Ground, parent);
                }
                parent.GetComponent<FighterClass>().animator.SetTrigger("StickHit");
                phase++;
                count = 0;
            }
        }
        if (phase == 2)
        {
            count += Time.deltaTime;
            if (count >= 2.0f)
            {
                return true;
            }
        }
        return false;
    }
}
