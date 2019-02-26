using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneClass : MonoBehaviour
{
    private void Awake()
    {
        gameObject.SetActive(false);
        //This currently isn't working because of ordering things.  The new struct setup should fix that.
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    public void cutsceneDone()
    {
        OverworldController.CutscenesPlaying--;
        CombatController.cutScenesPlaying--;
        Destroy(gameObject);
    }
}
