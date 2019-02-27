using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;

public class GameDataTracker : MonoBehaviour
{
    public static GameDataTracker DataTracker;
    public static PlayerData playerData = new PlayerData();
    public static SceneInfo currentScene = new SceneInfo();
    public static string saveFileName = "SaveFile1";
    
    void Awake()
    {
        //MAKES THIS OBJECT PERSIST IN EVERY SCENE
        if (DataTracker == null)
        {
            DontDestroyOnLoad(gameObject);
            DataTracker = this;
            LoadAll(); //The game was just loaded and needs this to be loaded.
        }
        else if (DataTracker != this)
        {
            Destroy(gameObject);
        }
    }

    //SAVE ALL IMPORTANT PARTS OF A SCENE===========NEEDS WORK===========
    public static void saveScene()
    {
        currentScene.sceneName = SceneManager.GetActiveScene().name;
        Save(currentScene.sceneName, currentScene);
    }
    //==================================================
    //SAVE ALL IMPORTANT PARTS OF A SCENE===========NEEDS WORK===========
    public static void loadScene(string nextSceneName)
    {
        currentScene = (SceneInfo)Load(nextSceneName);
    }
    //==================================================





    public void LoadAll()
    {

    }

    public static void Save(string Filename, object SaveObject)
    {
        if(Directory.Exists(Application.persistentDataPath + "/" + saveFileName) == false)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + saveFileName);
        }
        if(File.Exists(Application.persistentDataPath + "/" + saveFileName + "/" + Filename))
        {
            File.Delete(Application.persistentDataPath + "/" + saveFileName + "/" + Filename);
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + saveFileName + "/" + Filename);
        bf.Serialize(file, SaveObject);
        file.Close();
    }

    public static object Load(string Filename)
    {
        BinaryFormatter bf = new BinaryFormatter();
        try
        {
            FileStream file = File.Open(Application.persistentDataPath + "/" + saveFileName + "/" + Filename, FileMode.Open);
            object data = bf.Deserialize(file) as object;
            file.Close();
            return (data);
        }
        catch (System.Exception e)
        {
            print("NoSceneSavedToLoad");
            return (null);
        }
    }
}

//THESE ARE THE OBJECTS I EXPECT WE'LL NEED====================
[Serializable]
public class PlayerData
{
    public int health = 0;
}

[Serializable]
public class Playerinventory
{
}

[Serializable]
public class SceneInfo
{
    public string sceneName = "YouDidn'tNameThisScene";
    public List<GameObject> FriendlyNPCs;
    public List<GameObject> enemyList;
    public List<GameObject> interactableObjects;
}

[Serializable]
public class StoryFlags
{
    public bool SomethingHappened = false;
}
//==============================================================
