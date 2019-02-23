using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SayDialogue : CutSceneClass
{
    public GameObject textBox;
    private GameObject spawnedTextBox;
    public TextAsset inputText;
    public float heightOverSpeaker;

    private bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        spawnedTextBox = Instantiate<GameObject>(textBox, new Vector3(transform.parent.position.x, transform.parent.position.y + heightOverSpeaker, transform.parent.position.z), Quaternion.identity);
        spawnedTextBox.GetComponent<TextBoxController>().textfile = inputText;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnedTextBox == null)
        {
            cutsceneDone();
        }
    }
}
