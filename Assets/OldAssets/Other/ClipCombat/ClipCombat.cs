using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipCombat : FriendlyScript
{
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        PlayerData pd = GameDataTracker.playerData;
        HPMax = pd.maxHealth;
        HP = pd.health;
    }

    public void updateGameStats()
    {
        PlayerData pd = GameDataTracker.playerData;
        pd.health = HP;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
