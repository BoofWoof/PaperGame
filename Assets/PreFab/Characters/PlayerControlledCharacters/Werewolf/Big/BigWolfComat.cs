using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWolfComat : FriendlyScript
{
    // Start is called before the first frame update
    void Start()
    {
        HP = GameDataTracker.playerData.WerewolfHealth;
    }

    // Update is called once per frame
    void Update()
    {
        GameDataTracker.playerData.WerewolfHealth = HP;
    }
}
