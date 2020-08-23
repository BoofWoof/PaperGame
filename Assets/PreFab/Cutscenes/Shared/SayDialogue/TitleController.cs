using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleController : MonoBehaviour
{
    public TMP_FontAsset fontChoice;
    public string characterName;

    private GameObject titleBox;
    private TextMeshPro myName;

    // Start is called before the first frame update
    void Start()
    {
        titleBox = new GameObject("Choice Dialogue");
        titleBox.transform.SetParent(this.transform);

        myName = titleBox.AddComponent<TextMeshPro>();
        myName.font = fontChoice;
        myName.fontSize = 0.7f;
        myName.enableVertexGradient = true;
        myName.alignment = TextAlignmentOptions.Center;
        myName.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 0.02f);

        RectTransform wrapAreaChoice = titleBox.GetComponent<RectTransform>();
        wrapAreaChoice.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0.8f);
        myName.ForceMeshUpdate();

        myName.text = characterName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
