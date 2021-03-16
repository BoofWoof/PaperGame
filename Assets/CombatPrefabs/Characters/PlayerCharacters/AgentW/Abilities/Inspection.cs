using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspection : moveTemplate
{
    public override void Activate(List<GameObject> targets) 
    {
        base.Activate(targets);
        CutsceneDeconstruct complexCutscene = ScriptableObject.CreateInstance<CutsceneDeconstruct>();
        GameDataTracker.combatExecutor.cutsceneDeconstruct = complexCutscene;
        GameDataTracker.combatExecutor.FocusOnCharacter(character.GetComponent<FighterClass>().pos);
        complexCutscene.Deconstruct(targets[0].GetComponent<FighterClass>().inspectionInfo, character.GetComponent<FighterClass>().name, character);
    }
}
