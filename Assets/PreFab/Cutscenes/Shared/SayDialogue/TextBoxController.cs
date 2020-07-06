using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxController : MonoBehaviour
{
    public TextAsset textfile;
    public TextMeshPro myText;

    //LineToDisplay
    private string[] textLines;
    private int currentLine = 0;
    GameObject dialogue;

    //SlowDisplay
    public float TextDisplaySpeed = 0.2f;
    private float delayCount = 0.0f;
    private string displayedText = null;
    private string displayedTextFull = null;
    private int stringLen = 0;
    private int stringDisp = 0;

    //Test Display
    private float edge = 0f;
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
        myText.transform.position = new Vector3(this.transform.position.x-edge, this.transform.position.y+edge*0.5f, this.transform.position.z-0.1f);

        RectTransform wrapArea = dialogue.GetComponent<RectTransform>();
        wrapArea.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 1.60f);
        myText.ForceMeshUpdate();

        displayedTextFull = textLines[currentLine];
        processLine(displayedTextFull);
        displayedTextFull = processLine(displayedTextFull);
        stringDisp = displayedTextFull.Length;
    }

    // Update is called once per frame
    void Update()
    {
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

        if ((Input.GetButtonDown("Fire1"))&&(stringDisp == stringLen)&&(currentLine+1<textLines.Length))
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
        if ((Input.GetButtonDown("Fire1")) && (stringDisp == stringLen) && (currentLine + 1 == textLines.Length))
        {
            Destroy(gameObject);
        }
    }

    void AnimateString()
    {
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
