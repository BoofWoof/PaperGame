using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenesCutscene : CutSceneClass
{
    public string nextSceneName;
    void Start()
    {
        GameDataTracker.saveScene();
        GameDataTracker.loadScene(nextSceneName);
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
        cutsceneDone();
    }
}
