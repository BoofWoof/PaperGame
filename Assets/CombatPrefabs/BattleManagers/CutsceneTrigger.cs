using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneTrigger : MonoBehaviour
{
    public void onCombatStart()
    {
        GameObject target = GameDataTracker.combatExecutor.Clip;
        FighterClass targetInfo = target.GetComponent<FighterClass>();
        SayDialogue dialogueCutscene = ScriptableObject.CreateInstance<SayDialogue>();
        TextAsset textAsset = new TextAsset("Test test hello.");
        dialogueCutscene.inputText = textAsset;
        
        dialogueCutscene.heightOverSpeaker = targetInfo.CharacterHeight + 0.5f;
        dialogueCutscene.speakerName = targetInfo.name;
        CutsceneController.addCutsceneEvent(dialogueCutscene, target, true, GameDataTracker.cutsceneModeOptions.Cutscene);
    }

    public void onTurnStart(int turn, TurnManager.turnPhases turnPhase)
    {

    }


}
