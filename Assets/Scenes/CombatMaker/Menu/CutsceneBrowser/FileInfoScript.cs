using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FileInfoScript : MonoBehaviour
{
    private bool IsDirectory;
    private string FileName;
    public GameObject FileNameMesh;
    public GameObject NotesMesh;

    public GameObject SourceMenu;

    public void SetLabels(string fileName, string notes, bool isDirectory)
    {
        FileName = fileName;
        FileNameMesh.GetComponent<TextMeshProUGUI>().SetText(fileName);
        NotesMesh.GetComponent<TextMeshProUGUI>().SetText(notes);
        IsDirectory = isDirectory;
    }

    public void SelectLabel()
    {
        if (IsDirectory)
        {
            SourceMenu.GetComponent<FileBrowserScript>().SelectDirectory(FileName);
        } else
        {
            SourceMenu.GetComponent<FileBrowserScript>().SelectOption(FileName);
        }
    }
}
