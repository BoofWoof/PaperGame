using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxController : MonoBehaviour
{
    GameControls controls;

    public TextAsset textfile;
    public List<NodeLinkData> choices;
    private TextMeshPro myText;
    public string speakerName;

    //Choices
    public CutsceneDeconstruct scriptSource;
    private List<GameObject> choiceBoxes = new List<GameObject>();
    private Vector2 activeChoice = new Vector2(0, 0);
    private float choiceTime = 0;

    //LineToDisplay
    private string[] textLines;
    private int currentLine = 0;
    private GameObject dialogue;

    //SlowDisplay
    public float TextDisplaySpeed = 0.2f;
    private float delayCount = 0.0f;
    private string displayedText = null;
    private string displayedTextFull = null;
    private int stringLen = 0;
    private int stringDisp = 0;

    //Test Display
    public TMP_FontAsset fontChoice;

    //AnimateCounts
    int updateCount = 0;

    //Text Modifiers
    private enum textModifiers
    {
        Rainbow,
        Shaky
    }
    private struct ModifiedText
    {
        public ModifiedText(textModifiers modification, int start, int end = -1)
        {
            START = start;
            END = end;
            MODIFICATION = modification;
        }

        public int START { get; }
        public int END { get; set; }
        public bool ENDSET()
        {
            if (END == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public textModifiers MODIFICATION { get; }
    }
    private List<ModifiedText> modifierList = new List<ModifiedText>();

    private void Awake()
    {
        controls = new GameControls();
    }

    private void OnEnable()
    {
        controls.OverworldControls.Enable();
    }

    private void OnDisable()
    {
        controls.OverworldControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        textLines = textfile.text.Split('\n');

        dialogue = new GameObject("TextboxDialogue");
        dialogue.transform.SetParent(this.transform);

        myText = dialogue.AddComponent<TextMeshPro>();
        myText.font = fontChoice;
        myText.fontSize = 1;
        myText.enableVertexGradient = true;
        myText.alignment = TextAlignmentOptions.Center;
        myText.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z-0.1f);

        RectTransform wrapArea = dialogue.GetComponent<RectTransform>();
        wrapArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1.60f);
        myText.ForceMeshUpdate();

        displayedTextFull = textLines[currentLine];
        processLine(displayedTextFull);
        displayedTextFull = processLine(displayedTextFull);
        stringDisp = displayedTextFull.Length;

        if (!string.IsNullOrEmpty(speakerName))
        {
            GameObject titlebox = Resources.Load<GameObject>("TextName");
            GameObject titleSpawn = Instantiate<GameObject>(titlebox, new Vector3(transform.position.x - 0.6f, transform.position.y + 0.5f, transform.position.z - 0.05f), Quaternion.identity);
            titleSpawn.GetComponent<TitleController>().characterName = speakerName;
            titleSpawn.transform.SetParent(transform);
        }
    }

    private void MoveSelector(int x, int y)
    {
        choiceBoxes[(int)activeChoice.x + (int)activeChoice.y * 2].GetComponent<ChoiceController>().Deselect();
        activeChoice += new Vector2(x, y);
        choiceTime = 0;
        activeChoice.x = activeChoice.x % 2;
        activeChoice.y = activeChoice.y % 2;
        if(activeChoice.x < 0)
        {
            activeChoice.x += 2;
        }
        if (activeChoice.y < 0)
        {
            activeChoice.y += 2;
        }
        if (choiceBoxes.Count == 2)
        {
            activeChoice.y = 0;
        }
        if (choiceBoxes.Count == 3 && activeChoice.y == 1)
        {
            activeChoice.x = 0;
        }
        print(activeChoice);
        choiceBoxes[(int)activeChoice.x + (int)activeChoice.y * 2].GetComponent<ChoiceController>().Select();
    }

    // Update is called once per frame
    void Update()
    {
        if (choices.Count >= 2 && stringDisp == stringLen)
        {
            if (choiceTime < 0.2)
            {
                choiceTime += Time.deltaTime;
            }
            else
            {
                Vector2 thumbstick_values = controls.OverworldControls.Movement.ReadValue<Vector2>();
                float moveHorizontal = thumbstick_values[0];
                float moveVertical = thumbstick_values[1];
                if (moveHorizontal > 0.1)
                {
                    MoveSelector(1, 0);
                }
                if (moveHorizontal < -0.1)
                {
                    MoveSelector(-1, 0);
                }
                if (moveVertical > 0.1)
                {
                    MoveSelector(0, 1);
                }
                if (moveVertical < -0.1)
                {
                    MoveSelector(0, -1);
                }
            }
        }


        updateCount++;
        updateCount %= 500;
        //Display Text Slowly Start-----------------------------------------------------
        if (stringLen < stringDisp)
        {
            if (delayCount < TextDisplaySpeed)
            {
                delayCount = delayCount + Time.deltaTime;
            }
            else
            {
                //Reset Delay Count
                delayCount = 0;
                //Add To String
                displayedText = displayedText + displayedTextFull[stringLen];
                stringLen++;
                //DISPLAY THE TEXT
                myText.text = displayedText;
            }
        }
        //Display Text Slowly End-----------------------------------------------------

        AnimateString();



        if (choices != null && choiceBoxes.Count == 0 && stringDisp == stringLen)
        {
            if (choices.Count >= 2)
            {
                GameObject choicebox = Resources.Load<GameObject>("TextChoice");
                GameObject choiceSpawn = Instantiate<GameObject>(choicebox, new Vector3(transform.position.x - 0.45f, transform.position.y - 0.5f, transform.position.z - 0.05f), Quaternion.identity);
                choiceSpawn.GetComponent<ChoiceController>().choices = choices[0];
                choiceSpawn.GetComponent<ChoiceController>().Initiate();
                choiceSpawn.GetComponent<ChoiceController>().Select();
                choiceSpawn.transform.SetParent(transform);
                choiceBoxes.Add(choiceSpawn);

                choiceSpawn = Instantiate<GameObject>(choicebox, new Vector3(transform.position.x + 0.45f, transform.position.y - 0.5f, transform.position.z - 0.05f), Quaternion.identity);
                choiceSpawn.GetComponent<ChoiceController>().choices = choices[1];
                choiceSpawn.GetComponent<ChoiceController>().Initiate();
                choiceSpawn.transform.SetParent(transform);
                choiceBoxes.Add(choiceSpawn);
            }
        }
        if ((controls.OverworldControls.MainAction.triggered)&&(stringDisp == stringLen)&&(currentLine+1<textLines.Length))
        {
            //Moves To Next Line
            currentLine++;
            displayedTextFull = textLines[currentLine];
            displayedTextFull = processLine(displayedTextFull);
            //Resets Slow Reveal
            displayedText = null;
            //RESET COUNTS
            stringLen = 0;
            stringDisp = displayedTextFull.Length;
            //DISPLAY THE TEXT
            myText.text = displayedText;
        }
        if ((controls.OverworldControls.MainAction.triggered) && (stringDisp == stringLen) && (currentLine + 1 == textLines.Length))
        {
            if (choices.Count > 0)
            {
                if (choices.Count == 1)
                {
                    scriptSource.nextGUID = choices[0].TargetNodeGuid;
                    scriptSource.Update();
                } else
                {
                    scriptSource.nextGUID = choices[(int)activeChoice.x + (int)activeChoice.y * 2].TargetNodeGuid;
                    scriptSource.Update();
                }
            }
            Destroy(gameObject);
        }


    }

    void AnimateString()
    {
        myText.ForceMeshUpdate();

        Color32[] newVertexColors;
        TMP_TextInfo textinfo = myText.textInfo;
        int vertexIndex;

        int materialindex = textinfo.characterInfo[0].materialReferenceIndex;
        newVertexColors = textinfo.meshInfo[materialindex].colors32;
        Vector3[] vertexList = textinfo.meshInfo[materialindex].vertices;

        //Modify Text---------------------------
        foreach (ModifiedText textmodifier in modifierList)
        {
            if (stringLen >= textmodifier.START)
            {
                for (int i = stringLen; i <= textmodifier.END; i++)
                {
                    if (stringLen < stringDisp)
                    {
                        //Add To String
                        displayedText = displayedText + displayedTextFull[stringLen];
                        stringLen++;
                        //DISPLAY THE TEXT
                        myText.text = displayedText;
                    }
                }
                if (textmodifier.MODIFICATION == textModifiers.Rainbow)
                {
                    myText.ForceMeshUpdate();
                    newVertexColors = textinfo.meshInfo[materialindex].colors32;
                    for (int i = textmodifier.START; i < textmodifier.END; i++)
                    {
                        if (displayedTextFull[i] != ' ')
                        { 
                            vertexIndex = myText.textInfo.characterInfo[i].vertexIndex;
                            float colorUpdate = (updateCount + i * 30) % 500;
                            Color32 c = Color.HSVToRGB(colorUpdate / 500, 1f, 0.8f);
                            newVertexColors[vertexIndex + 0] = c;
                            newVertexColors[vertexIndex + 1] = c;
                            newVertexColors[vertexIndex + 2] = c;
                            newVertexColors[vertexIndex + 3] = c;
                        }
                    }
                }

                if (textmodifier.MODIFICATION == textModifiers.Shaky)
                {
                    for (int i = textmodifier.START; i < textmodifier.END; i++)
                    {
                        if (displayedTextFull[i] != ' ')
                        {
                            vertexIndex = myText.textInfo.characterInfo[i].vertexIndex;
                            float shakeUpdate = (updateCount + i * 30) % 500;
                            vertexList[vertexIndex + 0] += new Vector3(0f, 0f, Mathf.Sin(shakeUpdate * 6.283f / 500) * 0.05f);
                            vertexList[vertexIndex + 1] += new Vector3(0f, 0f, Mathf.Sin(shakeUpdate * 6.283f / 500) * 0.05f);
                            vertexList[vertexIndex + 2] += new Vector3(0f, 0f, Mathf.Sin(shakeUpdate * 6.283f / 500) * 0.05f);
                            vertexList[vertexIndex + 3] += new Vector3(0f, 0f, Mathf.Sin(shakeUpdate * 6.283f / 500) * 0.05f);
                        }
                    }
                }
            }
        }
        myText.UpdateVertexData();
        //Modify Text End-----------------------
    }

    string processLine(string FullText)
    {
        modifierList = new List<ModifiedText>();
        textModifiers modifier = textModifiers.Rainbow;
        char[] charsToTrim = { '/', '[', ']' };
        string modifierName = "";
        for (int i = 0; i < FullText.Length; i++)
        {
            if (FullText[i] == '[')
            {
                int endIndex = FullText.IndexOf(']');
                modifierName = FullText.Substring(i, endIndex - i + 1);
                FullText = FullText.Replace(modifierName, "");
                //Identify Modifier Type
                switch (modifierName.Trim(charsToTrim).ToLower())
                {
                    case "rainbow":
                        modifier = textModifiers.Rainbow;
                        break;
                    case "shaky":
                        modifier = textModifiers.Shaky;
                        break;
                    default:
                        print("Improper textbox syntax.");
                        break;
                }
                if (modifierName[1] != '/')
                {
                    modifierList.Add(new ModifiedText(modifier, i));
                }
                else
                {
                    //Identify Modifier Type
                    for (int j=0; j<modifierList.Count; j++)
                    {
                        ModifiedText modifierItem = modifierList[j];
                        if (modifierItem.MODIFICATION == modifier)
                        {
                            if (modifierItem.ENDSET() == false)
                            {
                                modifierItem.END = i;
                                modifierList[j] = modifierItem;
                            }
                        }
                    }
                }
                i--;
            }
        }
        return (FullText);
    }

    public void destroySelf()
    {
        Destroy(gameObject);
    }
}
