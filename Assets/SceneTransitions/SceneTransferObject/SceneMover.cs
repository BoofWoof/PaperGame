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
        if (trig.CompareTag("Player") && GameDataTracker.cutsceneMode == GameDataTracker.cutsceneModeOptions.Mobile)
        {
            GameDataTracker.deadEnemyIDs.Clear();

            PlayerTravelDirection m = ScriptableObject.CreateInstance<PlayerTravelDirection>();
            m.endPosition = transform.position;
            m.travelDirection = exitDirection;
            CutsceneController.addCutsceneEvent(m, OverworldController.Player, true, GameDataTracker.cutsceneModeOptions.Cutscene);

            ChangeScenesCutscene s = ScriptableObject.CreateInstance<ChangeScenesCutscene>();
            s.nextSceneName = sceneName;
            CutsceneController.addCutsceneEvent(s, OverworldController.Player, true, GameDataTracker.cutsceneModeOptions.Cutscene);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
