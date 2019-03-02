using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMover : MonoBehaviour
{
    public enum exitDirectionOptions {up, down, left, right};
    public string sceneName;
    public float halfTriggerHeight = 1f;
    public exitDirectionOptions exitDirection;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter(Collider trig)
    {
        if (trig.CompareTag("Player") && OverworldController.gameMode == OverworldController.gameModeOptions.Mobile)
        {
            GameDataTracker.deadEnemyIDs.Clear();

            GameObject move = new GameObject("MOVE CUTSCENE");
            PlayerTravelDirection m = move.AddComponent<PlayerTravelDirection>();
            m.endPosition = transform.position;
            m.travelDirection = exitDirection;
            OverworldController.addCutsceneEvent(move, OverworldController.Player, true, OverworldController.gameModeOptions.Cutscene);

            GameObject sceneChange = new GameObject("LOAD NEW SCENE");
            ChangeScenesCutscene s = sceneChange.AddComponent<ChangeScenesCutscene>();
            s.nextSceneName = sceneName;
            OverworldController.addCutsceneEvent(sceneChange, OverworldController.Player, true, OverworldController.gameModeOptions.Cutscene);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
