using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveResponseSocket : ResponseCutscene
{
    override public void ResponseSocket(string response)
    {
        if (response == "Save")
        {
            GameDataTracker.playerData.sceneName = SceneManager.GetActiveScene().name;
            GameDataTracker.playerData.savePosition = GetComponent<SavePointScript>().SpawnPoint.transform.position;
            GameDataTracker.Save();
            return;
        }
        if (response == "Load")
        {
            GameDataTracker.Load();
            SceneManager.LoadScene(GameDataTracker.playerData.sceneName, LoadSceneMode.Single);
            return;
        }
    }
}
