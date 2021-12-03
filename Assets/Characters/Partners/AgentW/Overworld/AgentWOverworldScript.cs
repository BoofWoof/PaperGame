using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentWOverworldScript : PartnerBaseScript
{
    public override void UseAbility()
    {
        DialogueContainer AreaInfo = OverworldController.AreaInfo;
        if (AreaInfo != null)
        {
            CutsceneDeconstruct complexCutscene = ScriptableObject.CreateInstance<CutsceneDeconstruct>();
            complexCutscene.Deconstruct(AreaInfo, GetComponent<FriendlyNPCClass>().ObjectInfo.ObjectName, gameObject);
        } else
        {
            Debug.Log("No info for this area.");
        }
    }
}
