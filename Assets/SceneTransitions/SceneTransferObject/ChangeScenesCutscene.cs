using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenesCutscene : CutSceneClass
{
    public string nextSceneName;
    public int transitionType = 0;
    void Start()
    {
    }

    override public bool Activate()
    {
        GameDataTracker.previousArea = SceneManager.GetActiveScene().name;
        GameObject transitionObject = Instantiate(SceneTransferMapping.sceneTransitionMap[transitionType]);
        transitionObject.GetComponent<LevelLoaderScript>().LoadNextLevel(nextSceneName);
        return false;
    }
}
