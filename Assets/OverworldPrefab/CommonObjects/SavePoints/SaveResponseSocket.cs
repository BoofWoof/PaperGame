using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveResponseSocket : ResponseCutscene
{
    override public void ResponseSocket(string response)
    {
        if (response == "Save")
        {
            GameDataTracker.Save();
            return;
        }
        if (response == "Load")
        {
            GameDataTracker.Load();
            return;
        }
    }
}
