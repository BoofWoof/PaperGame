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
        Rainbow
    }
    private struct ModifiedText
    {
        public ModifiedText(int start, int end, textModifiers modification)
        {
            START = start;
            END = end;
            MODIFICATION = modification;
        }

        public int START { get; }
        public int END { get; }
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
        stringDisp = displayedTextFull.Length;

        processLine(displayedTextFull);
        displayedTextFull = processLine(displayedTextFull);
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
        //Modify Text---------------------------
        foreach (ModifiedText textmodifier in modifierList)
        {
            for (int i = textmodifier.START; i < textmodifier.END; i++)
            {
                if (i <= stringLen)
                {
                    if ((stringLen < stringDisp) && (stringLen == i))
                    {
                        //Add To String
                        displayedText = displayedText + displayedTextFull[stringLen];
                        stringLen++;
                        //DISPLAY THE TEXT
                        myText.text = displayedText;
                    }

                    int materialindex = textinfo.characterInfo[i].materialReferenceIndex;
                    newVertexColors = textinfo.meshInfo[materialindex].colors32;
                    int vertexIndex = myText.textInfo.characterInfo[i].vertexIndex;
                    float colorUpdate = (updateCount + i * 100)%500;
                    Color32 c = Color.HSVToRGB(colorUpdate/500, 1f, 0.8f);
                    newVertexColors[vertexIndex + 0] = c;
                    newVertexColors[vertexIndex + 1] = c;
                    newVertexColors[vertexIndex + 2] = c;
                    newVertexColors[vertexIndex + 3] = c;
                    myText.UpdateVertexData();
                }
            }
        }
        //Modify Text End-----------------------
    }

    string processLine(string FullText)
    {
        modifierList = new List<ModifiedText>();
        bool changes = true;
        while (changes == true)
        {
            changes = false;
            int startindex = FullText.IndexOf("[rainbow]");
            if (startindex != -1)
            {
                FullText = FullText.Replace("[rainbow]", "");
                changes = true;
                int endindex = FullText.IndexOf("[/rainbow]");
                if (endindex == -1)
                {
                    endindex = FullText.Length-1;
                }
                else
                {
                    FullText = FullText.Replace("[/rainbow]", "");
                }
                modifierList.Add(new ModifiedText(startindex, endindex, textModifiers.Rainbow));
            }
        }
        return (FullText);
    }

    public void destroySelf()
    {
        Destroy(gameObject);
    }
}
