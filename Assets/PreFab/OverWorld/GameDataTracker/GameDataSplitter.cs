using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


[Serializable]
public class GameDataSplitter : MonoBehaviour
{
    public PlayerInventory playerInventory;

    public PythonDict globalGameFlags;

    public Dictionary<string, Badge> badgeMap;
    public Dictionary<string, Item> itemMap;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startSave()
    {
        globalGameFlags["playerPos"] = new PythonDict(OverworldController.Player.transform.position);
        globalGameFlags["playerHP"] = new PythonDict(GameDataTracker.playerData.health);
        globalGameFlags["playerMaxHP"] = new PythonDict(GameDataTracker.playerData.maxHealth);
        globalGameFlags["playerInventorySize"] = new PythonDict(playerInventory.maxSize);
        globalGameFlags["playerInventory"] = new PythonDict(playerInventory.GetSaveString());
    }

    public void loadSave()
    {
        OverworldController.Player.transform.position = globalGameFlags["playerPos"].getPos();
        GameDataTracker.playerData.health = globalGameFlags["playerHP"].getVal();
        GameDataTracker.playerData.maxHealth = globalGameFlags["playerMaxHP"].getVal();
        playerInventory = new PlayerInventory();
        playerInventory.maxSize = globalGameFlags["playerInventorySize"].getVal();
        playerInventory.LoadSaveString(globalGameFlags["playerInventory"]);
    }

    public void setDefaults()
    {
        playerInventory = new PlayerInventory();
        globalGameFlags = new PythonDict();
    }


    [Serializable]
    public class PythonDict
    {
        /* Public properties and conversions */
        public PythonDict this[String index]
        {
            get
            {
                return this.dict_[index];
            }
            set
            {
                this.dict_[index] = value;
            }
        }

        public static implicit operator PythonDict(String value)
        {
            return new PythonDict(value);
        }

        public static implicit operator String(PythonDict value)
        {
            return value.str_;
        }

        /* Public methods */
        public PythonDict()
        {
            this.dict_ = new Dictionary<String, PythonDict>();
        }

        public PythonDict(String value)
        {
            this.str_ = value;
        }

        public PythonDict(Vector3 value)
        {
            this.pos_ = value;
        }

        public PythonDict(bool value)
        {
            this.flag_ = value;
        }

        public PythonDict(int value)
        {
            this.val_ = value;
        }

        /// Getters
        public Vector3 getPos()
        {
            return pos_;
        }

        public string getStr()
        {
            return str_;
        }

        public int getVal()
        {
            return val_;
        }

        public bool isString()
        {
            return (this.str_ != null);
        }

        /* Private fields */
        Dictionary<String, PythonDict> dict_ = null;
        String str_;
        Vector3 pos_;
        bool flag_;
        int val_;
    }
}
