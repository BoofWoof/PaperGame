using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CutsceneTriggerMenu : MonoBehaviour
{
    public GameObject SourceMenu;
    public GameObject FileBrowser;

    public List<GridObject> TargetCharacters;

    public string CutscenePath = "";
    public GameObject FilePathText;

    private void Start()
    {
        transform.SetParent(SourceMenu.transform.parent);
        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    public void Close()
    {
        SourceMenu.SetActive(true);
        Destroy(gameObject);
    }

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
