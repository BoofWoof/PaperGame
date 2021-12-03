using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class EnemyNPCClass : MonoBehaviour
{
    //CharactersName
    private GameObject Player;
    public ObjectInfoScript ObjectInfo;

    [Header("Combat Encounter Info")]
    public CombatContainer combatEncounter;
    public CutsceneTrigger cutsceneTrigger;


    [HideInInspector]public float UniqueSceneID;

    private void Awake()
    {
        UniqueSceneID = (1000f * transform.position.x) + transform.position.y + (.001f * transform.position.z);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameDataTracker.deadEnemyIDs.Contains(UniqueSceneID))
        {
            Destroy(gameObject);
        }
        else
        {
            Character thisNPCCharacter = new Character();
            thisNPCCharacter.CharacterObject = gameObject;
            thisNPCCharacter.ObjectInfo = ObjectInfo;
            thisNPCCharacter.uniqueSceneID = UniqueSceneID;
            GameDataTracker.CharacterList.Add(thisNPCCharacter);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Player != null)
            {
                GameDataTracker.deadEnemyIDs.Add(UniqueSceneID);
                GameDataTracker.combatStartPosition = Player.transform.position;
                GameDataTracker.lastAreaWasCombat = true;

                ChangeScenesCutscene s = ScriptableObject.CreateInstance<ChangeScenesCutscene>();
                GameDataTracker.combatScene = combatEncounter;
                GameDataTracker.cutsceneTrigger = cutsceneTrigger;
                s.nextSceneName = "CombatExecution";
                CutsceneController.addCutsceneEvent(s, OverworldController.Player, true, GameDataTracker.cutsceneModeOptions.Cutscene);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Player == null)
        {
            Player = OverworldController.Player;
        }
    }
}
