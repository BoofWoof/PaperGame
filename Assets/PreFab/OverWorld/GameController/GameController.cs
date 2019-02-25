using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum gameModeOptions {Mobile, Cutscene, MobileCutscene};
    public static gameModeOptions gameMode = gameModeOptions.Mobile;
    public static GameObject Player;

    public void Start()
    {
    }
}


