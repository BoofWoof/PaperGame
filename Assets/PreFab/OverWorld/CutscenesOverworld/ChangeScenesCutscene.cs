using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenesCutscene : CutSceneClass
{
    public string nextSceneName;
    void Start()
    {
        GameDataTracker.previousArea = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
        cutsceneDone();
    }
}
