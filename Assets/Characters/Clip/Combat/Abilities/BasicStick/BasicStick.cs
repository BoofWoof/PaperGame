using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicStick : moveTemplate
{
    GameObject target;
    public GameObject stickTimer;

    public override void Activate(List<GameObject> targets)
    {
        base.Activate(targets);

        target = targets[0];

        BasicStickCutscene basicStick = ScriptableObject.CreateInstance<BasicStickCutscene>();
        basicStick.target = target;
        basicStick.power = character.GetComponent<FighterClass>().Power;
        basicStick.stickTimerPrefab = stickTimer;
        CutsceneController.addCutsceneEvent(basicStick, character, true, GameDataTracker.cutsceneModeOptions.Cutscene);
    }
}

public class BasicStickCutscene : CutSceneClass
{
    private int phase = 0;
    private float count = 0.0f;
    public int power;
    public GameObject target;
    public GameObject stickTimerPrefab;
    private GameObject stickTimer;
    private GameControls controls;

    private HeightHighlighter heightCutscene;

    override public bool Activate()
    {
        controls = new GameControls();
        List<Vector2Int> minigameBlocks = parent.GetComponent<GridObject>().currentGridOccupation();
        minigameBlocks.AddRange(target.GetComponent<GridObject>().currentGridOccupation());
        heightCutscene = ScriptableObject.CreateInstance<HeightHighlighter>();
        heightCutscene.targetPos = minigameBlocks;
        heightCutscene.heightChange = 2f;
        heightCutscene.Activate();
        controls.CombatControls.Enable();
        return true;
    }

    // Update is called once per frame
    override public bool Update()
    {
        if (heightCutscene.Update()) return true;
        float horizontalPosition = controls.CombatControls.Movement_Clip.ReadValue<Vector2>()[0];
        if (phase == 0)
        {
            if (stickTimer == null)
            {
                GameDataTracker.combatExecutor.FocusOnCharacter(parent.GetComponent<GridObject>().pos);
                stickTimer = Instantiate(stickTimerPrefab, parent.transform.position + new Vector3(-1f, 1.3f, 0f), Quaternion.identity);
                stickTimer.transform.localScale = new Vector3(2, 2, 1);
                stickTimer.GetComponent<Animator>().speed = 0;
            }
            if (horizontalPosition < -0.3f)
            {
                stickTimer.GetComponent<Animator>().speed = 2;
                parent.GetComponent<FighterClass>().animator.SetTrigger("HoldStickHit");
                phase++;
            }
        }
        if (phase == 1)
        {
            count += Time.deltaTime;
            if (horizontalPosition > -0.1f || count >= 1.1f)
            {
                if (count > 0.9f && count < 1.1f)
                {
                    target.GetComponent<FighterClass>().postBufferAttackEffect(power*2, FighterClass.attackType.Normal, FighterClass.statusEffects.None, FighterClass.attackLocation.Ground, parent);
                } else
                {
                    target.GetComponent<FighterClass>().postBufferAttackEffect(power, FighterClass.attackType.Normal, FighterClass.statusEffects.None, FighterClass.attackLocation.Ground, parent);
                }
                parent.GetComponent<FighterClass>().animator.SetTrigger("InstantStickHit");
                phase++;
                count = 0;
                Destroy(stickTimer);
                heightCutscene.Deactivate();
            }
        }
        if (phase == 2)
        {
            count += Time.deltaTime;
            if (count >= 1.0f)
            {
                GameDataTracker.combatExecutor.SetCameraToWorld();
            }
        }
        return false;
    }
}
