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
            Load();
        }
        else if (DataTracker != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
    }

    public void ConvertInventoryListToSaveData()
    {

    }
    public void ConvertBadgeListToSaveData()
    {

    }
    public void ConvertChallengeListToSaveData()
    {

    }

    public static void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savedata.bof";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, playerData);
        stream.Close();
    }

    public static void Load()
    {
        string path = Application.persistentDataPath + "/savedata.bof";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            playerData = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
        } else
        {
            //Debug.LogError("Save file not found in " + path);
            print("Save file not found in " + path);
        }
    }
}

//THESE ARE THE OBJECTS I EXPECT WE'LL NEED====================
[Serializable]
public class PlayerData
{
    //Statsaves
    public int maxHealth = 10;
    public int health = 10;
    public int maxAP = 10;
    public int ap = 10;
    public int defenseModifier = 0;
    public int attackModifier = 0;

    //Eventsaves
    public List<string> events = new List<string>();

    //Companion Unlock
    public int CompanionMaxHealth = 10;
    public int CompanionDefenseModifier = 0;
    public int CompanionAttackModifier = 0;
    public bool Werewolf = false;
    public int WerewolfHealth = 10;
    public bool Fish = false;
    public int FishHealth = 10;
    public bool Android = false;
    public int AndroidHealth = 10;
    public bool Fae = false;
    public int FaeHealth = 10;
    public bool Artist = false;
    public int ArtistHealth = 10;

    //Equipment -- Every badge is equipped to an ID.
    public bool[] UnlockedEquipmentID = new bool[128];
    public bool[] EquipedEquipmentID = new bool[128];

    //Challenges -- Every challenge has a unique ID.
    public bool[] UnlockedChallengeID = new bool[128];
    public bool[] EquipedChallengeID = new bool[128];
    public bool[] CompletedChallengeID = new bool[128];

    //ItemsList -- Every Item has an ID.  A value can appear on the list multiple times.
    public List<int> Inventory = new List<int>();
    public int InventorySize = 10;
}

[Serializable]
public class objectUsage
{
    public string objectName = "ObjectBoi";
    public bool objectUsed = true;
}
