using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
Things to do when adding new trigger.
Make new UI.
Make info saver.
Update viewer.
Update updater.
*/

public class CutsceneTriggerMenu : MenuBaseScript
{
    public GameObject FileBrowser;

    public List<GridObject> TargetCharacters;
    public int TriggerLimit;

    public string CutscenePath = "";
    public GameObject FilePathText;

    public TMP_InputField LabelInput;

    public void FindCutscene()
    {
        GameObject AddCharactersMenu = Instantiate(FileBrowser);
        AddCharactersMenu.GetComponent<FileBrowserScript>().SourceMenu = gameObject;

        gameObject.SetActive(false);
    }

    public void UpdateCutscenePath(string pathName)
    {
        CutscenePath = pathName;
        FilePathText.GetComponent<TextMeshProUGUI>().SetText(CutscenePath);
    }
}
