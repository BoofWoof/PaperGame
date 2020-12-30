using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    //[Header("Default Starting Scene Name")]
    public string startSceneName;
    
    ///[Header("First Menu Objects")]
    public GameObject FirstMenu;
    public GameObject SaveFile1;
    public GameObject SaveFile2;
    public GameObject SaveFile3;
    public GameObject SaveFile4;
    
    ///[Header("New Game Menu Objects")]
    public GameObject NewGameTextField;

    public GameObject FileNameMenu;
    // Start is called before the first frame update

    void Start()
    {
        UpdateAllFileName();
        FileNameMenu.SetActive(false);
    }

    void UpdateAllFileName()
    {
        UpdateFileName("Save1", SaveFile1);
        UpdateFileName("Save2", SaveFile2);
        UpdateFileName("Save3", SaveFile3);
        UpdateFileName("Save4", SaveFile4);
    }

    void UpdateFileName(string fileName, GameObject textUI)
    {
        GameDataTracker.saveFileName = fileName;
        GameDataTracker.Load();
        textUI.GetComponent<Text>().text = GameDataTracker.playerData.fileName;
    }

    public void CloseSubMenus()
    {
        FileNameMenu.SetActive(false);
        FirstMenu.SetActive(true);
    }

    public void StartGame(string saveFileName)
    {
        GameDataTracker.saveFileName = saveFileName;
        GameDataTracker.Load();
        if (GameDataTracker.playerData.fileName == "New Game")
        {
            FirstMenu.SetActive(false);
            FileNameMenu.SetActive(true);
        } else
        {
            SceneManager.LoadScene(startSceneName, LoadSceneMode.Single);
        }
    }

    public void StartNewGame()
    {
        GameDataTracker.playerData.fileName = NewGameTextField.GetComponent<TMP_InputField>().text;
        GameDataTracker.Save();
        SceneManager.LoadScene(startSceneName, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void DeleteSave(string deleteFileName)
    {
        GameDataTracker.DeleteFile(deleteFileName);
        UpdateAllFileName();
    }
}
