using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfoScript : MonoBehaviour
{
    public string ObjectName = "Name Me";
    public string ObjectNote = "";
    public float ObjectHeight = 1f;
    public float ObjectWidth = 1f;
    public float TextHeightOverObject = 0.5f;
    public bool FlatObject = true;

    public AudioClip[] LetterNoises;
    public int[] LetterNoisesPerList;

    public float GetDialogueHeightOverSpeaker()
    {
        return TextHeightOverObject + ObjectHeight;
    }
}
