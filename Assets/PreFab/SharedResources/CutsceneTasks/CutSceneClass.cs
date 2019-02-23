using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneClass : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

    public void cutsceneDone()
    {
        if (transform.parent.GetComponent<FighterClass>() != null) {
            sceneLists.cutScenesPlaying--;
            Destroy(gameObject);
        }
    }
}
