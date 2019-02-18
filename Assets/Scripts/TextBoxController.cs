using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxController : MonoBehaviour
{
    public TextAsset textfile;
    public TextMesh myText;

    //LineToDisplay
    private string[] textLines;
    private int currentLine = 0;
    GameObject dialogue;

    //Display Cutting
    private int lineLength = 0;
    private int lastPlacement = 0;
    private int maxLineLength = 26;

    //SlowDisplay
    public float TextDisplaySpeed = 0.2f;
    private float delayCount = 0.0f;
    private string displayedText = null;
    private string displayedTextFull = null;
    private int stringLen = 0;
    private int stringDisp = 0;

    //Test Display
    private float edge = 0f;
    public Font fontChoice;
    public Material fontMaterialChoice;

    // Start is called before the first frame update
    void Start()
    {
        GameController.gameMode = "Dialogue";
        textLines = textfile.text.Split('\n');

        dialogue = new GameObject("TextboxDialogue");
        dialogue.transform.SetParent(this.transform);

        myText = dialogue.AddComponent<TextMesh>();
        myText.font = fontChoice;
        dialogue.GetComponent<MeshRenderer>().material = fontMaterialChoice;
        myText.fontSize = 30;
        myText.characterSize = 0.04f;
        myText.anchor = TextAnchor.MiddleCenter;
        myText.alignment = TextAlignment.Center;
        myText.transform.position = new Vector3(this.transform.position.x-edge, this.transform.position.y+edge*0.5f, this.transform.position.z);
        //myText.color = new Color(255, 255, 255);

        displayedTextFull = textLines[currentLine];
        stringDisp = displayedTextFull.Length;
    }

    // Update is called once per frame
    void Update()
    {
        //Display Text Slowly Start-----------------------------------------------------
        if (stringLen < stringDisp)
        {
            if (delayCount < TextDisplaySpeed)
            {
                delayCount = delayCount + Time.deltaTime;
            }
            else
            {
                //Add to line length.
                lineLength++;
                //Reset Delay Count
                delayCount = 0;
                //Add To String
                displayedText = displayedText + displayedTextFull[stringLen];
                stringLen++;
                //AddNewLine
                float width = dialogue.GetComponent<MeshRenderer>().bounds.size.x;
                //if (lineLength == maxLineLength)
                //{
                //    displayedText = occurenceReplace(displayedText);
                //}
                if (width > 1.8)
                {
                    displayedText = occurenceReplace(displayedText);
                }
                //DISPLAY THE TEXT
                myText.text = displayedText;
            }
        }
        //Display Text Slowly End-----------------------------------------------------

        if ((Input.GetButtonDown("Fire1"))&&(stringDisp == stringLen)&&(currentLine+1<textLines.Length))
        {
            //Moves To Next Line
            currentLine++;
            displayedTextFull = textLines[currentLine];
            //Resets Slow Reveal
            displayedText = null;
            //RESET COUNTS
            stringLen = 0;
            stringDisp = displayedTextFull.Length;
            //DISPLAY THE TEXT
            myText.text = displayedText;
            //Resets Line Length
            lineLength = 0;
            lastPlacement = 0;
        }
        if ((Input.GetButtonDown("Fire1")) && (stringDisp == stringLen) && (currentLine + 1 == textLines.Length))
        {
            GameController.gameMode = "Mobile";
            GetComponentInParent<NPCTalk>().settextBoxNotExist();
            Destroy(gameObject);
        }
    }

    private string occurenceReplace(string inputString)
    {
        int placement = inputString.LastIndexOf(" ");
        inputString = inputString.Remove(placement,1).Insert(placement, "\n");
        return (inputString);
    }

    private float GetWidth(TextMesh mesh)
    {
        float width = 0;
        foreach (char symbol in mesh.text)
        {
            CharacterInfo info;
            if (mesh.font.GetCharacterInfo(symbol, out info, mesh.fontSize, mesh.fontStyle))
            {
                width += info.advance;
            }
        }
        return width * mesh.characterSize * 0.1f;
    }

    public void destroySelf()
    {
        Destroy(gameObject);
    }
}
