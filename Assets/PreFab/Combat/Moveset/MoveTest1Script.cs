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
            m.SetActive(false);
            Vector2 moveDestination = Vector2.MoveTowards(new Vector2(friendlyList[sourceID].transform.position.x, friendlyList[sourceID].transform.position.z), new Vector2(enemyList[targetID].transform.position.x, enemyList[targetID].transform.position.z), 1f);
            m.GetComponent<MoveToLocation>().endPosition = new Vector3(moveDestination.x, friendlyList[sourceID].transform.position.y, moveDestination.y);
            sceneLists.addCutseenEvent(m, friendlyList[sourceID], true, new Vector3(0.8f, 0.8f, -3.0f));

            GameObject j = Instantiate(jumpCutscene, Vector3.zero, Quaternion.identity);
            j.SetActive(false);
            j.GetComponent<JumpToLocation>().endPosition = new Vector3(enemyList[targetID].transform.position.x - 1, enemyList[targetID].transform.position.y, enemyList[targetID].transform.position.z);
            sceneLists.addCutseenEvent(j, friendlyList[sourceID], true);

            GameObject a = Instantiate(damageCutscene, Vector3.zero, Quaternion.identity);
            a.SetActive(false);
            a.GetComponent<DealDamage>().source = friendlyList[sourceID];
            a.GetComponent<DealDamage>().amount = power;
            a.GetComponent<DealDamage>().type = FighterClass.attackType.Normal;
            a.GetComponent<DealDamage>().effects = FighterClass.statusEffects.None;
            sceneLists.addCutseenEvent(a, enemyList[targetID], true);

            GameObject d = Instantiate(dialogueCutscene, Vector3.zero, Quaternion.identity);
            d.SetActive(false);
            sceneLists.addCutseenEvent(d, friendlyList[sourceID], true);

            d = Instantiate(dialogueCutscene, Vector3.zero, Quaternion.identity);
            d.SetActive(false);
            d.GetComponent<SayDialogue>().inputText = new TextAsset("Plz No Hit\nI am fragile.");
            sceneLists.addCutseenEvent(d, enemyList[targetID], true);

            m = Instantiate(moveCutscene, Vector3.zero, Quaternion.identity);
            m.SetActive(false);
            moveDestination = Vector2.MoveTowards(new Vector2(enemyList[targetID].transform.position.x, enemyList[targetID].transform.position.z), new Vector2(friendlyList[sourceID].GetComponent<FighterClass>().HomePosition.x, friendlyList[sourceID].GetComponent<FighterClass>().HomePosition.z), 1.5f);
            m.GetComponent<MoveToLocation>().endPosition = new Vector3(moveDestination.x, enemyList[targetID].transform.position.y, moveDestination.y);
            sceneLists.addCutseenEvent(m, friendlyList[sourceID], true);

            j = Instantiate(jumpCutscene, Vector3.zero, Quaternion.identity);
            j.SetActive(false);
            j.GetComponent<JumpToLocation>().endPosition = friendlyList[sourceID].GetComponent<FighterClass>().HomePosition;
            sceneLists.addCutseenEvent(j, friendlyList[sourceID], true);
            /*
            bool wait = false;
            for(int i = 0; i < enemyList.Count; i++)
            {
                d = Instantiate(dialogueCutscene, Vector3.zero, Quaternion.identity);
                d.SetActive(false);
                if (i == enemyList.Count - 1) wait = true;
                sceneLists.addCutseenEvent(d, enemyList[i], wait);
            }
            //------------------------------------------------------------------------------------
            */
        }
        else
        {
            enemyList[targetID].GetComponent<FighterClass>().attackEffect(power, FighterClass.attackType.Normal, FighterClass.statusEffects.None, enemyList[sourceID]);
        }
        print("You hit um!");
    }
}
