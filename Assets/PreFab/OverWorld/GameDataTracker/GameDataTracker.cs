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
    public static string saveFileName = "SaveFile1";

    //Super Important Data
    public static string previousArea;
    //CombatInfo
    public static Vector3 combatStartPosition;
    public static List<int> deadEnemyIDs = new List<int>();
    public static bool lastAreaWasCombat = false;
    
    void Awake()
    {
        //MAKES THIS OBJECT PERSIST IN EVERY SCENE
        if (DataTracker == null)
        {
            DontDestroyOnLoad(gameObject);
            DataTracker = this;
        }
        else if (DataTracker != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    }

    

    public static void Save()
    {
        if(Directory.Exists(Application.persistentDataPath + "/" + saveFileName) == false)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + saveFileName);
        }
        if(File.Exists(Application.persistentDataPath + "/" + saveFileName + "/" + saveFileName))
        {
            File.Delete(Application.persistentDataPath + "/" + saveFileName + "/" + saveFileName);
        }

        DataTracker.gameObject.GetComponent<GameDataSplitter>().startSave();

        string jsonString = JsonUtility.ToJson(DataTracker.gameObject.GetComponent<GameDataSplitter>());
        using (StreamWriter streamWriter = File.CreateText(Application.persistentDataPath + "/" + saveFileName + "/"+ saveFileName))
        {
            streamWriter.Write(jsonString);
        }

        
    }

    public static void Load()
    {
        try
        {
            StreamReader streamReader = File.OpenText(Application.persistentDataPath + "/" + saveFileName);
            JsonUtility.FromJsonOverwrite(streamReader.ReadToEnd(), DataTracker.gameObject.GetComponent<GameDataSplitter>());
            DataTracker.gameObject.GetComponent<GameDataSplitter>().loadSave();
        }
        catch (System.Exception e)
        {
            return;
        }
    }
}

//THESE ARE THE OBJECTS I EXPECT WE'LL NEED====================
[Serializable]
public class PlayerData
{
    public int maxHealth = 10;
    public int health = 10;
}

[Serializable]
public class objectUsage
{
    public string objectName = "ObjectBoi";
    public bool objectUsed = true;
}
