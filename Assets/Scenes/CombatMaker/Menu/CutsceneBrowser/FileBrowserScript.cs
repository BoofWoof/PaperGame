using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileBrowserScript : MonoBehaviour
{
    public GameObject SourceMenu;

    public GameObject ContentScreen;
    public GameObject SelectableItemPrefab;
    private List<GameObject> SelectableItems = new List<GameObject>();
    private string BaseFilePath;
    private string FilePathLayer = "";

    private void Start()    
    {
        BaseFilePath = Application.dataPath + "/DialogueEditor/Resources";
        transform.SetParent(SourceMenu.transform.parent);
        GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        UpdateList();
    }

    public void Close()
    {
        SourceMenu.SetActive(true);
        Destroy(gameObject);
    }

    public void SelectOption(string fileName)
    {
        if (FilePathLayer.Length > 0)
        {
            SourceMenu.GetComponent<CutsceneTriggerMenu>().UpdateCutscenePath(FilePathLayer + "/" + fileName);
        }
        else
        {
            SourceMenu.GetComponent<CutsceneTriggerMenu>().UpdateCutscenePath(fileName);
        }
        Close();
    }

    public void SelectDirectory(string directoryName)
    {
        ClearList();
        if (FilePathLayer.Length > 0)
        {
            FilePathLayer += "/" + directoryName;
        }
        else
        {
            FilePathLayer = directoryName;
        }
        UpdateList();
    }

    public void MoveUpDirectory()
    {
        if(FilePathLayer.LastIndexOf("/") >= 0)
        {
            FilePathLayer = FilePathLayer.Substring(0, FilePathLayer.LastIndexOf("/"));
        } else
        {
            FilePathLayer = "";
        }
        UpdateList();
    }

    private void ClearList()
    {
        for (int idx = 0; idx < SelectableItems.Count; idx++)
        {
            Destroy(SelectableItems[idx]);
        }
        SelectableItems = new List<GameObject>();
}

    private void UpdateList()
    {
        DirectoryInfo info = new DirectoryInfo(BaseFilePath + "/" + FilePathLayer);
        FileInfo[] fileInfos = info.GetFiles();
        foreach (FileInfo fileInfo in fileInfos)
        {
            string fileName = fileInfo.Name;
            string fileEnding = fileName.Substring(fileName.LastIndexOf("."), fileName.Length - fileName.LastIndexOf("."));
            if (fileEnding != ".asset") continue;
            fileName = fileName.Substring(0, fileName.LastIndexOf("."));
            DialogueContainer dialogueContainer;
            if (FilePathLayer.Length > 0) {
                dialogueContainer = Resources.Load<DialogueContainer>(FilePathLayer + "/" + fileName);
            } else
            {
                dialogueContainer = Resources.Load<DialogueContainer>(fileName);
            }
            GameObject fileInfoUI = Instantiate(SelectableItemPrefab);
            fileInfoUI.GetComponent<FileInfoScript>().SourceMenu = gameObject;
            fileInfoUI.GetComponent<FileInfoScript>().SetLabels(fileName, dialogueContainer.Note, false);

            fileInfoUI.transform.SetParent(ContentScreen.transform);

            SelectableItems.Add(fileInfoUI);
        }

        DirectoryInfo[] directoryInfos = info.GetDirectories();
        foreach (DirectoryInfo directoryInfo in directoryInfos)
        {
            string fileName = directoryInfo.Name;
            GameObject fileInfoUI = Instantiate(SelectableItemPrefab);
            fileInfoUI.GetComponent<FileInfoScript>().SourceMenu = gameObject;
            fileInfoUI.GetComponent<FileInfoScript>().SetLabels(fileName, "Folder", true);

            fileInfoUI.transform.SetParent(ContentScreen.transform);

            SelectableItems.Add(fileInfoUI);
        }

        float contentHeight = 100 * SelectableItems.Count;
        ContentScreen.GetComponent<RectTransform>().sizeDelta = new Vector2(1240, contentHeight);
        for(int idx = 0; idx < SelectableItems.Count; idx++)
        {
            SelectableItems[idx].GetComponent<RectTransform>().anchoredPosition = 
                new Vector3(0, idx*100f - contentHeight/2f + 50, 0);
        }
    }
}
