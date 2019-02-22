using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS ONE DOES A BASIC ATTACK
public class MoveTest1Script : MoveClass
{
    public GameObject dialogueCutscene;

    void Start()
    {
        targetMode = targetModeTypes.Enemies;
    }
    public override void effect()
    {
        if (friendlySource)
        {
            enemyList[targetID].GetComponent<FighterClass>().attackEffect(power, FighterClass.attackType.Normal, FighterClass.statusEffects.None, friendlyList[sourceID]);
            friendlyList[sourceID].GetComponent<FighterClass>().cutsceneTasks.Add(dialogueCutscene);
        }
        else
        {
            enemyList[targetID].GetComponent<FighterClass>().attackEffect(power, FighterClass.attackType.Normal, FighterClass.statusEffects.None, enemyList[sourceID]);
        }
        print("You hit um!");
    }
}
