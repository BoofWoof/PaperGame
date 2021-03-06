﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceController : MonoBehaviour
{
    public NodeLinkData choices;
    public TMP_FontAsset fontChoice;

    private GameObject choiceDialogue;
    private TextMeshPro myChoice;

    private Vector3 startLocation;

    void Start()
    {

    }

    public void Initiate()
    {
        startLocation = transform.position;

        choiceDialogue = new GameObject("Choice Dialogue");
        choiceDialogue.transform.SetParent(this.transform);

        myChoice = choiceDialogue.AddComponent<TextMeshPro>();
        myChoice.font = fontChoice;
        myChoice.fontSize = 0.5f;
        myChoice.enableVertexGradient = true;
        myChoice.alignment = TextAlignmentOptions.Center;
        myChoice.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 0.02f);

        RectTransform wrapAreaChoice = choiceDialogue.GetComponent<RectTransform>();
        wrapAreaChoice.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0.65f);
        myChoice.ForceMeshUpdate();

        myChoice.text = choices.PortName;
    }

    public void Select()
    {
        myChoice.text = ">" + myChoice.text + "<";
        transform.position = startLocation + new Vector3(0, 0, -0.05f);
    }
    public void Deselect()
    {
        myChoice.text = myChoice.text.Substring(1, myChoice.text.Length - 2);
        transform.position = startLocation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
