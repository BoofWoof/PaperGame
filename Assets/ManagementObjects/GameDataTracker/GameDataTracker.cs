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

    public static string saveFileName = null;

    //Current Mode
    public enum gameModeOptions { Mobile, Cutscene, MobileCutscene, DialogueReady, Paused };
    public static gameModeOptions gameMode = gameModeOptions.Mobile;
    public static gameModeOptions gameModePre = gameModeOptions.Mobile;

    //Super Important Data
    public static string previousArea;
    public static int lastTransition = 0;
    public static bool transitioning = false;

    //CombatInfo
    public static Vector3 combatStartPosition;
    public static List<float> deadEnemyIDs = new List<float>();
    public static bool lastAreaWasCombat = false;

    //Flags
    public static Dictionary<string, bool> boolFlags = new Dictionary<string, bool>();
    public static Dictionary<string, string> stringFlags = new Dictionary<string, string>();

    //CombatTracking
    public static CombatExecutor combatExecutor;
    public static CombatContainer combatScene;
    public static CutsceneTrigger cutsceneTrigger;

    //CharacterInfo
    public static List<Character> CharacterList = new List<Character>();
    //public static List<Character> EnemyList;

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
        spawnLastTransitionObject();
    }

    static public void spawnLastTransitionObject()
    {
        GameObject transitionObject = Instantiate(SceneTransferMapping.sceneTransitionMap[lastTransition]);
    }

    static public void clearCharacterList()
    {
        CharacterList = new List<Character>();
    }

    static public void GameOver()
    {
        deadEnemyIDs = new List<float>();
        lastAreaWasCombat = false;
        previousArea = null;
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
        string path = Application.persistentDataPath + "/" + saveFileName + ".bof";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, playerData);
        stream.Close();
    }

    public static void Load()
    {
        string path = Application.persistentDataPath + "/" + saveFileName + ".bof";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            playerData = formatter.Deserialize(stream) as PlayerData;
            stream.Close();
        } else
        {
            //Debug.LogError("Save file not found in " + path);
            print("Creating new save data.");
            PlayerData playerData = new PlayerData();
        }
    }

    public void LateUpdate()
    {
        CutsceneController.Update();
    }

    public static void DeleteFile(string deleteFileName)
    {
        playerData = new PlayerData();
        saveFileName = deleteFileName;
        Save();
    }

    public static void AddItem(int id)
    {
        playerData.Inventory.Add(id);
    }

    public static void AddBadge(int id)
    {
        playerData.UnlockedEquipmentID[id] = true;
    }

    public static void ChangeHealth(int amount)
    {
        playerData.health += amount;
        if (playerData.health > playerData.maxHealth)
        {
            playerData.health = playerData.maxHealth;
        }
    }
    
    public static Character findCharacterByName(string Name, List<Character> charList)
    {
        Character foundCharacter = new Character();
        foundCharacter.CharacterName = "NoNamesLikeThat";
        foundCharacter.CharacterObject = null;
        foundCharacter.dialogueHeight = -1;
        foundCharacter.uniqueSceneID = -1;
        foreach (Character charItem in charList)
        {
            if (charItem.CharacterName == Name)
            {
                foundCharacter = charItem;
                return (foundCharacter);
            }
        }
        return (foundCharacter);
    }
    public static Character findCharacterUniqueSceneID(int ID, List<Character> charList)
    {
        Character foundCharacter = new Character();
        foundCharacter.CharacterName = "NoNamesLikeThat";
        foundCharacter.CharacterObject = null;
        foundCharacter.dialogueHeight = -1;
        foundCharacter.uniqueSceneID = -1;
        foreach (Character charItem in charList)
        {
            if (charItem.uniqueSceneID == ID)
            {
                foundCharacter = charItem;
                return (foundCharacter);
            }
        }
        return (foundCharacter);
    }

}

public class Character
{
    public GameObject CharacterObject;
    public string CharacterName;
    public float dialogueHeight;
    public float uniqueSceneID;
}

//THESE ARE THE OBJECTS I EXPECT WE'LL NEED====================
[Serializable]
public class PlayerData
{
    //Metaknowledge
    public string fileName = "New Game";

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

    //One Time Collectibles Tracker
    public Dictionary<string, List<double>> GatheredItemsDictionary = new Dictionary<string, List<double>>();

    //Equipment -- Every badge is equipped to an ID.
    public int MaxBadgePoints = 3;
    public int CurrentBadgePoints = 3;

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
