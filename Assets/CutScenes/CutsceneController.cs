using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneController
{
    private static List<CutSceneEvent> CutsceneQueue = new List<CutSceneEvent>();
    private static List<CutSceneEvent> CutsceneActive = new List<CutSceneEvent>();
    public static int CutscenesPlaying = 0;

    //WAYS TO ADD CUTSCENE EVENTS-----------------------------------------------------------------------
    public static void addCutsceneEvent(CutSceneClass CutsceneEventInput, GameObject CutsceneTargetInput, bool WaitInput, OverworldController.gameModeOptions GameModeInput, GameObject CameraFocusInput, Vector3 CameraOffsetInput)
    {
        CutSceneEvent newEvent = new CutSceneEvent();
        newEvent.CutsceneEvent = CutsceneEventInput;
        newEvent.CutsceneTarget = CutsceneTargetInput;
        newEvent.Wait = WaitInput;
        newEvent.GameMode = GameModeInput;
        newEvent.CameraFocus = CameraFocusInput;
        newEvent.CameraOffset = CameraOffsetInput;
        CutsceneQueue.Add(newEvent);
    }

    public static void addCutsceneEvent(CutSceneClass CutsceneEventInput, GameObject CutsceneTargetInput, bool WaitInput, OverworldController.gameModeOptions GameModeInput)
    {
        CutSceneEvent newEvent = new CutSceneEvent();
        newEvent.CutsceneEvent = CutsceneEventInput;
        newEvent.CutsceneTarget = CutsceneTargetInput;
        newEvent.Wait = WaitInput;
        newEvent.GameMode = GameModeInput;
        newEvent.CameraFocus = null;
        newEvent.CameraOffset = Vector3.zero;
        CutsceneQueue.Add(newEvent);
    }
    public static void addCutsceneEventFront(CutSceneClass CutsceneEventInput, GameObject CutsceneTargetInput, bool WaitInput, OverworldController.gameModeOptions GameModeInput, GameObject CameraFocusInput, Vector3 CameraOffsetInput)
    {
        CutSceneEvent newEvent = new CutSceneEvent();
        newEvent.CutsceneEvent = CutsceneEventInput;
        newEvent.CutsceneTarget = CutsceneTargetInput;
        newEvent.Wait = WaitInput;
        newEvent.GameMode = GameModeInput;
        newEvent.CameraFocus = CameraFocusInput;
        newEvent.CameraOffset = CameraOffsetInput;
        CutsceneQueue.Insert(0, newEvent);
    }

    public static void addCutsceneEventFront(CutSceneClass CutsceneEventInput, GameObject CutsceneTargetInput, bool WaitInput, OverworldController.gameModeOptions GameModeInput)
    {
        CutSceneEvent newEvent = new CutSceneEvent();
        newEvent.CutsceneEvent = CutsceneEventInput;
        newEvent.CutsceneTarget = CutsceneTargetInput;
        newEvent.Wait = WaitInput;
        newEvent.GameMode = GameModeInput;
        newEvent.CameraFocus = null;
        newEvent.CameraOffset = Vector3.zero;
        CutsceneQueue.Insert(0, newEvent);
    }

    public static void Update()
    {
        //Debug.Log(CutsceneQueue.Count);
        if (CutscenesPlaying == 0 && CutsceneQueue.Count > 0)
        {
            Debug.Log(CutsceneQueue.Count);
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
                OverworldController.gameMode = eventInitiation.GameMode;
                if (keep) {
                    CutsceneActive.Add(eventInitiation);
                    CutscenesPlaying++;
                }
                CutsceneQueue.Remove(eventInitiation);
            }
        }
        if (CutscenesPlaying == 0 && CutsceneQueue.Count == 0 && (OverworldController.gameMode == OverworldController.gameModeOptions.MobileCutscene || OverworldController.gameMode == OverworldController.gameModeOptions.Cutscene))
        {
            OverworldController.gameMode = OverworldController.gameModeOptions.Mobile;
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
}
