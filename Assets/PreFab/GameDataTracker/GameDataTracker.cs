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

    //Records that a chest was opened.======================================
    public bool usedCheck(string ObjectName)
    {
        objectUsage chest = (objectUsage)Load("Used" + ObjectName);
        if (chest == null)
        {
            return (false);
        } else
        {
            return (true);
        }
    }
    public void useObject(string ObjectName)
    {
        objectUsage newChest = new objectUsage();
        newChest.objectName = ObjectName;
        Save("Used" + ObjectName, newChest);
    }
    //=====================================================================

    public static void Save(string Filename, object SaveObject)
    {
        if(Directory.Exists(Application.persistentDataPath + "/" + saveFileName + "/" + SceneManager.GetActiveScene().name) == false)
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/" + saveFileName + "/" + SceneManager.GetActiveScene().name);
        }
        if(File.Exists(Application.persistentDataPath + "/" + saveFileName + "/" + SceneManager.GetActiveScene().name + "/" + Filename))
        {
            File.Delete(Application.persistentDataPath + "/" + saveFileName + "/" + SceneManager.GetActiveScene().name + "/" + Filename);
        }
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + saveFileName + "/" + SceneManager.GetActiveScene().name + "/" + Filename);
        bf.Serialize(file, SaveObject);
        file.Close();
    }

    public static object Load(string Filename)
    {
        BinaryFormatter bf = new BinaryFormatter();
        try
        {
            FileStream file = File.Open(Application.persistentDataPath + "/" + saveFileName + "/" + SceneManager.GetActiveScene().name + "/" + Filename, FileMode.Open);
            object data = bf.Deserialize(file) as object;
            file.Close();
            return (data);
        }
        catch (System.Exception e)
        {
            return (null);
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
