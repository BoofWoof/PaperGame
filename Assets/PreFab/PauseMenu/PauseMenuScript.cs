using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    [Header("Character Submenu")]
    public GameObject characterMenu;
    public GameObject characterMenuFirstFocus;

    [Header("Item Submenu")]
    public GameObject itemMenu;
    public GameObject itemMenuFirstFocus;

    private int currentMenuID = 0;
    private List<GameObject> menuList;
    private int maxMenuID = 2;

    void Start()
    {
        menuList = new List<GameObject>()
        {
            characterMenu,
            itemMenu
        };
        itemMenu.SetActive(false);
    }

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Right Bumper"))
        {
            menuList[currentMenuID].SetActive(false);
            currentMenuID += 1;
            if (currentMenuID >= maxMenuID)
            {
                currentMenuID = 0;
            }
            menuList[currentMenuID].SetActive(true);
        }
        if (Input.GetButtonDown("Left Bumper"))
        {
            menuList[currentMenuID].SetActive(false);
            currentMenuID -= 1;
            if (currentMenuID <= -1)
            {
                currentMenuID = maxMenuID - 1;
            };
            menuList[currentMenuID].SetActive(true);
        }
    }
}
