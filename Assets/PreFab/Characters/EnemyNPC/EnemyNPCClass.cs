using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNPCClass : MonoBehaviour
{
    //CharactersName
    private GameObject Player;
    public string CharacterName = "NameMeYaDingus";
    public int UniqueSceneID = -1;
    // Start is called before the first frame update
    void Start()
    {
        if (GameDataTracker.deadEnemyIDs.Contains(UniqueSceneID))
        {
            Destroy(gameObject);
        }
        else
        {
            if (UniqueSceneID == -1)
            {
                print("Give " + CharacterName + " an ID ya nerd.");
            }
            Character thisNPCCharacter = new Character();
            thisNPCCharacter.CharacterObject = gameObject;
            thisNPCCharacter.CharacterName = CharacterName;
            thisNPCCharacter.uniqueSceneID = UniqueSceneID;
            OverworldController.EnemyList.Add(thisNPCCharacter);
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

                GameObject sceneChange = new GameObject("LOAD NEW SCENE");
                ChangeScenesCutscene s = sceneChange.AddComponent<ChangeScenesCutscene>();
                s.nextSceneName = "CombatTest";
                OverworldController.addCutsceneEvent(sceneChange, OverworldController.Player, true, OverworldController.gameModeOptions.Cutscene);
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
