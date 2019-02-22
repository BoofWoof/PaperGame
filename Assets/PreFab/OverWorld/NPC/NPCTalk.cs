using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalk : MonoBehaviour
{
    public TextAsset textfile;
    public GameObject Character;
    public GameObject TextObject;
    private GameObject textbox = null;
    public float textspeed = 0.03f;
    private bool textBoxExist = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")&&(Vector3.Distance(Character.transform.position, transform.position)<1)&&(GameController.gameMode=="Mobile"))
        {
            textbox = (GameObject)Instantiate(TextObject, new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.identity);
            textbox.transform.SetParent(this.transform);
            textbox.GetComponent<TextBoxController>().textfile = textfile;
            textbox.GetComponent<TextBoxController>().TextDisplaySpeed = textspeed;
            textBoxExist = true;
        }
        if ((Vector3.Distance(Character.transform.position, transform.position) > 3) && (textbox != null))
        {
            print("Test");
            GameController.gameMode = "Mobile";
            Destroy(textbox);
            textBoxExist = false;
        }
    }
}
