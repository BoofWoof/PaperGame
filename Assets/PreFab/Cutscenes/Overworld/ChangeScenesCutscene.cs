using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScenesCutscene : CutSceneClass
{
    public string nextSceneName;
    void Start()
    {
    }

    override public bool Activate()
    {
        GameDataTracker.previousArea = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
        return false;
    }
}
