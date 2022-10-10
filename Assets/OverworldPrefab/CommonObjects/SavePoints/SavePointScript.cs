using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointScript : DestructionScript
{
    public DialogueContainer dialogue;

    public ObjectInfoScript ObjectInfo;
    public GameObject SpawnPoint;

    public void Start()
    {
        Character thisNPCCharacter = new Character();
        thisNPCCharacter.CharacterObject = gameObject;
        thisNPCCharacter.ObjectInfo = ObjectInfo;
        thisNPCCharacter.uniqueSceneID = GetInstanceID();
        GameDataTracker.CharacterList.Add(thisNPCCharacter);
    }

    public override void BreakObject()
    {
        CutsceneDeconstruct complexCutscene = ScriptableObject.CreateInstance<CutsceneDeconstruct>();
        complexCutscene.Deconstruct(dialogue, ObjectInfo.ObjectName, gameObject);
    }
}
