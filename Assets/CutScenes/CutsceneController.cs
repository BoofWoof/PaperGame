using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CutSceneEvent
{
    public CutSceneClass CutsceneEvent;
    public GameObject CutsceneTarget;
    public bool Wait;
    public GameObject CameraFocus;
    public Vector3 CameraOffset;
    public GameDataTracker.cutsceneModeOptions CutsceneMode;
}

public class CutsceneController
{
    private static List<CutSceneEvent> CutsceneQueue = new List<CutSceneEvent>();
    private static List<CutSceneEvent> CutsceneActive = new List<CutSceneEvent>();
    public static int CutscenesPlaying = 0;

    //WAYS TO ADD CUTSCENE EVENTS-----------------------------------------------------------------------
    public static void addCutsceneEvent(CutSceneClass CutsceneEventInput, GameObject CutsceneTargetInput, bool WaitInput, GameDataTracker.cutsceneModeOptions CutsceneModeInput, GameObject CameraFocusInput, Vector3 CameraOffsetInput)
    {
        CutSceneEvent newEvent = new CutSceneEvent();
        newEvent.CutsceneEvent = CutsceneEventInput;
        newEvent.CutsceneTarget = CutsceneTargetInput;
        newEvent.Wait = WaitInput;
        newEvent.CutsceneMode = CutsceneModeInput;
        newEvent.CameraFocus = CameraFocusInput;
        newEvent.CameraOffset = CameraOffsetInput;
        CutsceneQueue.Add(newEvent);
    }

    public static void addCutsceneEvent(CutSceneClass CutsceneEventInput, GameObject CutsceneTargetInput, bool WaitInput, GameDataTracker.cutsceneModeOptions CutsceneModeInput)
    {
        CutSceneEvent newEvent = new CutSceneEvent();
        newEvent.CutsceneEvent = CutsceneEventInput;
        newEvent.CutsceneTarget = CutsceneTargetInput;
        newEvent.Wait = WaitInput;
        newEvent.CutsceneMode = CutsceneModeInput;
        newEvent.CameraFocus = null;
        newEvent.CameraOffset = Vector3.zero;
        CutsceneQueue.Add(newEvent);
    }
    public static void addCutsceneEventFront(CutSceneClass CutsceneEventInput, GameObject CutsceneTargetInput, bool WaitInput, GameDataTracker.cutsceneModeOptions CutsceneModeInput, GameObject CameraFocusInput, Vector3 CameraOffsetInput)
    {
        CutSceneEvent newEvent = new CutSceneEvent();
        newEvent.CutsceneEvent = CutsceneEventInput;
        newEvent.CutsceneTarget = CutsceneTargetInput;
        newEvent.Wait = WaitInput;
        newEvent.CutsceneMode = CutsceneModeInput;
        newEvent.CameraFocus = CameraFocusInput;
        newEvent.CameraOffset = CameraOffsetInput;
        CutsceneQueue.Insert(0, newEvent);
    }

    public static void addCutsceneEventFront(CutSceneClass CutsceneEventInput, GameObject CutsceneTargetInput, bool WaitInput, GameDataTracker.cutsceneModeOptions CutsceneModeInput)
    {
        CutSceneEvent newEvent = new CutSceneEvent();
        newEvent.CutsceneEvent = CutsceneEventInput;
        newEvent.CutsceneTarget = CutsceneTargetInput;
        newEvent.Wait = WaitInput;
        newEvent.CutsceneMode = CutsceneModeInput;
        newEvent.CameraFocus = null;
        newEvent.CameraOffset = Vector3.zero;
        CutsceneQueue.Insert(0, newEvent);
    }

    public static void Update()
    {
        if (CutscenesPlaying == 0 && CutsceneQueue.Count > 0)
        {
            bool keepPlaying = true;
            while (keepPlaying)
            {
                CutSceneEvent eventInitiation = CutsceneQueue[0];
                eventInitiation.CutsceneEvent.parent = eventInitiation.CutsceneTarget;
                bool keep = eventInitiation.CutsceneEvent.Activate();
                if (eventInitiation.Wait == true || CutsceneQueue.Count == 1)
                {
                    keepPlaying = false;
                }
                GameDataTracker.cutsceneMode = eventInitiation.CutsceneMode;
                if (keep) {
                    CutsceneActive.Add(eventInitiation);
                    CutscenesPlaying++;
                }
                CutsceneQueue.Remove(eventInitiation);
            }
        }
        if (CutscenesPlaying == 0 && CutsceneQueue.Count == 0 && GameDataTracker.cutsceneMode != GameDataTracker.cutsceneModeOptions.Mobile && !GameDataTracker.transitioning)
        {
            GameDataTracker.cutsceneMode = GameDataTracker.cutsceneModeOptions.Mobile;
        }
        for (int i = CutsceneActive.Count; i > 0; i--)
        {
            CutSceneEvent cutscene = CutsceneActive[i - 1];
            bool delete = cutscene.CutsceneEvent.Update();
            if (delete)
            {
                CutsceneActive.Remove(cutscene);
                CutscenesPlaying--;
            }
        }
    }

    public static bool noCutscenes()
    {
        return CutscenesPlaying == 0 && CutsceneQueue.Count == 0;
    }
}
