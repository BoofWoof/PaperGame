using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickUpItem : MonoBehaviour
{
    public int ItemID;

    private double instanceID;
    private string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        if (!GameDataTracker.playerData.GatheredItemsDictionary.ContainsKey(sceneName))
        {
            GameDataTracker.playerData.GatheredItemsDictionary.Add(sceneName, new List<double>());
        }

        instanceID = (1000 * transform.position.x) + transform.position.y + (.001 * transform.position.z);
        if(GameDataTracker.playerData.GatheredItemsDictionary[sceneName].IndexOf(instanceID) != -1)
        {
            Destroy(gameObject);
        }
        SpriteRenderer SR = gameObject.GetComponent<SpriteRenderer>();
        SR.sprite = ItemMapping.imageMap[ItemID];
    }

    private void OnTriggerEnter(Collider trig)
    {
        if (trig.CompareTag("Player") && OverworldController.gameMode == OverworldController.gameModeOptions.Mobile)
        {
            GameDataTracker.AddItem(ItemID);
            GameDataTracker.playerData.GatheredItemsDictionary[sceneName].Add(instanceID);
            Destroy(gameObject);
        }
    }
}
