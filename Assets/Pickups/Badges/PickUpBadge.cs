using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PickUpBadge : MonoBehaviour
{
    public int BadgeID;

    private Vector3 startingPos;
    private float phase;

    private double instanceID;
    private string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        phase = Random.Range(-180, 180);

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
        SR.sprite = BadgeMapping.badgeMap[BadgeID].GetComponent<BadgeTemplate>().sprite;
    }

    private void OnTriggerEnter(Collider trig)
    {
        if (trig.CompareTag("Player") && OverworldController.gameMode == OverworldController.gameModeOptions.Mobile)
        {
            GameDataTracker.AddBadge(BadgeID);
            GameDataTracker.playerData.GatheredItemsDictionary[sceneName].Add(instanceID);
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        transform.position = startingPos + new Vector3(0, 0.05f * Mathf.Sin(Time.time * 2 + phase), 0);
    }
}
