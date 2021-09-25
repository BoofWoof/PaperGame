using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldAssetBase : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    virtual public void MobileUpdate()
    {

    }

    virtual public void CutsceneUpdate()
    {

    }

    virtual public void MobileCutsceneUpdate()
    {

    }

    virtual public void PausedUpdate()
    {

    }

    public enum gameModeOptions { Mobile, Cutscene, MobileCutscene, DialogueReady, Paused, AbilityFreeze };
}
