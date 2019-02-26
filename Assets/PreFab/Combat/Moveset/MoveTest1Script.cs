using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//THIS ONE DOES A BASIC ATTACK
public class MoveTest1Script : MoveClass
{
    public GameObject dialogueCutscene;
    public GameObject moveCutscene;
    public GameObject jumpCutscene;
    public GameObject damageCutscene;

    void Start()
    {
        targetMode = targetModeTypes.Enemies;
    }
    public override void effect()
    {
        if (friendlySource)
        {
            //ATTACK CUT SCENE--------------------------------------------------------------------
            GameObject m = Instantiate(moveCutscene, Vector3.zero, Quaternion.identity);
            Vector2 moveDestination = Vector2.MoveTowards(new Vector2(friendlyList[sourceID].CharacterObject.transform.position.x, friendlyList[sourceID].CharacterObject.transform.position.z), new Vector2(enemyList[targetID].CharacterObject.transform.position.x, enemyList[targetID].CharacterObject.transform.position.z), 1f);
            m.GetComponent<MoveToLocation>().endPosition = new Vector3(moveDestination.x, friendlyList[sourceID].CharacterObject.transform.position.y, moveDestination.y);
            CombatController.addCutseenEvent(m, friendlyList[sourceID].CharacterObject, true, friendlyList[sourceID].CharacterObject, new Vector3(1f, 1f, -3.0f));

            GameObject bamboo = new GameObject();
            bamboo.AddComponent<BambooStickAttack>();
            bamboo.GetComponent<BambooStickAttack>().source = friendlyList[sourceID].CharacterObject;
            bamboo.GetComponent<BambooStickAttack>().amount = power;
            bamboo.GetComponent<BambooStickAttack>().type = FighterClass.attackType.Normal;
            bamboo.GetComponent<BambooStickAttack>().effects = FighterClass.statusEffects.None;
            bamboo.GetComponent<BambooStickAttack>().damageTarget = enemyList[targetID].CharacterObject;
            bamboo.GetComponent<BambooStickAttack>().endPosition = enemyList[targetID].CharacterObject.transform.position + new Vector3(-1, 0, 0);
            bamboo.GetComponent<BambooStickAttack>().heightOverHighestCharacter = 2;
            bamboo.GetComponent<BambooStickAttack>().speed = 1;
            CombatController.addCutseenEvent(bamboo, friendlyList[sourceID].CharacterObject, true);
            /*
            GameObject j = Instantiate(jumpCutscene, Vector3.zero, Quaternion.identity);
            j.GetComponent<JumpToLocation>().endPosition = new Vector3(enemyList[targetID].transform.position.x - 1, enemyList[targetID].transform.position.y, enemyList[targetID].transform.position.z);
            sceneLists.addCutseenEvent(j, friendlyList[sourceID], true);

            GameObject d = Instantiate(dialogueCutscene, Vector3.zero, Quaternion.identity);
            sceneLists.addCutseenEvent(d, friendlyList[sourceID], true);

            d = Instantiate(dialogueCutscene, Vector3.zero, Quaternion.identity);
            d.GetComponent<SayDialogue>().inputText = new TextAsset("Plz No Hit\nI am fragile.");
            sceneLists.addCutseenEvent(d, enemyList[targetID], true);

            GameObject a = Instantiate(damageCutscene, Vector3.zero, Quaternion.identity);
            a.GetComponent<DealDamage>().source = friendlyList[sourceID];
            a.GetComponent<DealDamage>().amount = power;
            a.GetComponent<DealDamage>().type = FighterClass.attackType.Normal;
            a.GetComponent<DealDamage>().effects = FighterClass.statusEffects.None;
            sceneLists.addCutseenEvent(a, enemyList[targetID], true, Vector3.zero);
            */

            m = Instantiate(moveCutscene, Vector3.zero, Quaternion.identity);
            moveDestination = Vector2.MoveTowards(new Vector2(enemyList[targetID].CharacterObject.transform.position.x, enemyList[targetID].CharacterObject.transform.position.z), new Vector2(friendlyList[sourceID].CharacterObject.GetComponent<FighterClass>().HomePosition.x, friendlyList[sourceID].CharacterObject.GetComponent<FighterClass>().HomePosition.z), 1.5f);
            m.GetComponent<MoveToLocation>().endPosition = new Vector3(moveDestination.x, enemyList[targetID].CharacterObject.transform.position.y, moveDestination.y);
            CombatController.addCutseenEvent(m, friendlyList[sourceID].CharacterObject, true, friendlyList[sourceID].CharacterObject, Vector3.zero);

            GameObject j = Instantiate(jumpCutscene, Vector3.zero, Quaternion.identity);
            j.GetComponent<JumpToLocation>().endPosition = friendlyList[sourceID].CharacterObject.GetComponent<FighterClass>().HomePosition;
            CombatController.addCutseenEvent(j, friendlyList[sourceID].CharacterObject, true);
            
            
        }
        else
        {
            enemyList[targetID].CharacterObject.GetComponent<FighterClass>().attackEffect(power, FighterClass.attackType.Normal, FighterClass.statusEffects.None, enemyList[sourceID].CharacterObject);
        }
        print("You hit um!");
    }
}
