﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTriggerCutscene : CutSceneClass
{
    public string TriggerName;
    public string TargetName;
    
    override public bool Activate()
    {
        Animator animatorStorage;
        if (string.IsNullOrEmpty(TargetName))
        {
            animatorStorage = parent.GetComponent<Animator>();
        } else
        {
            animatorStorage = OverworldController.findCharacterByName(TargetName, OverworldController.CharacterList).CharacterObject.GetComponent<Animator>();
        }
        animatorStorage.SetTrigger(TriggerName);
        return false;
    }
}
