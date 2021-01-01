using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public int ItemID;
    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer SR = gameObject.GetComponent<SpriteRenderer>();
        SR.sprite = ItemMapping.imageMap[ItemID];
    }

    private void OnTriggerEnter(Collider trig)
    {
        if (trig.CompareTag("Player") && OverworldController.gameMode == OverworldController.gameModeOptions.Mobile)
        {
            GameDataTracker.AddItem(ItemID);
            Destroy(gameObject);
        }
    }
}
