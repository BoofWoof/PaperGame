using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject characterMenuFirstFocus;

    private void OnEnable()
    {
        //EventSystem.current.SetSelectedGameObject(null);
        //EventSystem.current.SetSelectedGameObject(characterMenuFirstFocus);
    }

    public void SaveGame()
    {
        GameDataTracker.Save();
    }

    public void ReturnToMenu()
    {
        GameDataTracker.deadEnemyIDs.Clear();
        GameDataTracker.previousArea = null;
        OverworldController.gameMode = OverworldController.gameModeOptions.Mobile;

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
