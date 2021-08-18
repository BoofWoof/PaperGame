using System;
using UnityEngine;

[Serializable]
public class ExtraInfo
{
    public string name;

    public int selfLimit;
    public ExtraInfoTarget[] selfInfo;

    public int targetLimit;
    public ExtraInfoTarget[] targetInfo;

    public bool changeStringLength;
    public string[] extraStrings;

    public bool changeValueLength;
    public int[] extraValues;

    public string trigger;
    public string[] triggerOptions;
}
