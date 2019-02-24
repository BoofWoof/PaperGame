using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class sceneLists
{
    //The important list of all enemies and friendly units.  Will eventually add interactable objects to it.
    public static List<GameObject> enemyList = new List<GameObject>();
    public static List<GameObject> friendList = new List<GameObject>();

    public static List<GameObject> cutsceneEventList = new List<GameObject>();
    public static List<GameObject> cutsceneTarget = new List<GameObject>();
    public static List<bool> waitforNext = new List<bool>();

    public static List<Vector3> offsetList = new List<Vector3>();
    public static List<GameObject> cameraFocusList = new List<GameObject>();


    public static int cutScenesPlaying = 0;
    public static bool newScene = false;

    public static GameObject cameraTrackTarget;
    public static Vector3 cameraOffset;
    public static Vector3 defaultOffset = new Vector3(0, 2.8f, -6.5f);
    public static GameObject defaultFocus;

    public static GameObject gameControllerAccess;

    //ADD CUTSCENE EVENT----------------------------------
    public static void addCutseenEvent(GameObject cutsceneEvent, GameObject target, bool wait, params Vector3[] offset)
    {
        cutsceneEvent.SetActive(false);
        cutsceneEventList.Add(cutsceneEvent);
        cutsceneTarget.Add(target);
        waitforNext.Add(wait);
        newScene = true;
        if (offset.Length > 0)
        {
            if (Vector3.zero == offset[0])
            {
                offsetList.Add(defaultOffset);
                cameraFocusList.Add(defaultFocus);
            } else
            {
                offsetList.Add(offset[0]);
                cameraFocusList.Add(target);
            }
        } else
        {
            offsetList.Add(Vector3.zero);
            cameraFocusList.Add(target);
        }
    }
    //----------------------------------------------------
    //ADD CUTSCENE EVENT IN FRONT----------------------------------
    public static void addCutseenEventFRONT(GameObject cutsceneEvent, GameObject target, bool wait, params Vector3[] offset)
    {
        cutsceneEvent.SetActive(false);
        cutsceneEventList.Insert(0,cutsceneEvent);
        cutsceneTarget.Insert(0, target);
        waitforNext.Insert(0, wait);
        newScene = true;
        if (offset.Length > 0)
        {
            if (Vector3.zero == offset[0])
            {
                offsetList.Insert(0, defaultOffset);
                cameraFocusList.Insert(0, defaultFocus);
            }
            else
            {
                offsetList.Insert(0, offset[0]);
                cameraFocusList.Insert(0, target);
            }
        }
        else
        {
            offsetList.Insert(0, Vector3.zero);
            cameraFocusList.Insert(0, target);
        }
    }
    //----------------------------------------------------
}