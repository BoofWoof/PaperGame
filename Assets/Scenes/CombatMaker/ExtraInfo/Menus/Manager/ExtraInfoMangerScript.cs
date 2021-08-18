using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExtraInfoMangerScript : MonoBehaviour
{
    public GameObject managerExtraInfoContent;
    private float minContentHeight;

    public GameObject extraInfoEditorPrefab;
    public GameObject extraInfoEditorButton;

    public GridCrafter menuSource;
    private List<GameObject> extraInfoEditButtonList = new List<GameObject>();

    public void Start()
    {
        minContentHeight = managerExtraInfoContent.GetComponent<RectTransform>().sizeDelta.y;
        UpdateMenuOptions();
    }

    public void CreateNewTileHeightChanger()
    {
        ExtraInfo newExtraInfo = new ExtraInfo();
        newExtraInfo.name = "UnnamedTileHeightChanger";
        newExtraInfo.extraValues = new int[1];
        newExtraInfo.changeValueLength = false;
        newExtraInfo.extraStrings = new string[] {"HeightChanger"};
        newExtraInfo.changeStringLength = false;
        newExtraInfo.triggerOptions = new string[] {"TriggerOnce", "RepeatTriggers"};
        newExtraInfo.selfLimit = -1;
        newExtraInfo.targetLimit = -1;
        menuSource.extraInfoList.Add(newExtraInfo);
        UpdateMenuOptions();
    }

    private void UpdateMenuOptions()
    {
        ClearEditButtons();
        float contentHeight = 100 + 56 * menuSource.extraInfoList.Count;
        if (contentHeight > minContentHeight)
        {
            managerExtraInfoContent.GetComponent<RectTransform>().sizeDelta = new Vector2(managerExtraInfoContent.GetComponent<RectTransform>().sizeDelta.x, contentHeight);
        }
        int extInfoIdx = 0;
        foreach (ExtraInfo extInfo in menuSource.extraInfoList)
        {
            GameObject extInfoEditButton = Instantiate(extraInfoEditorButton, managerExtraInfoContent.transform.position + new Vector3(345, -55 * (0.6f + extInfoIdx), 0), Quaternion.identity);
            extInfoEditButton.transform.parent = managerExtraInfoContent.transform;
            extInfoEditButton.transform.Find("ButtonName").GetComponent<TextMeshProUGUI>().text = extInfo.name;
            int buttonIdx = extInfoIdx;
            extInfoEditButton.GetComponent<Button>().onClick.AddListener(() => { GenerateExtraInfoEditor(buttonIdx); });
            extInfoEditButton.transform.Find("DeleteButton").GetComponent<Button>().onClick.AddListener(() => {DeleteExtraAtIndex(buttonIdx);});
            extInfoIdx += 1;
            extraInfoEditButtonList.Add(extInfoEditButton);
        }
    }

    public void DeleteExtraAtIndex(int deletionIndex)
    {
        Debug.Log(deletionIndex);
        menuSource.extraInfoList.RemoveAt(deletionIndex);
        UpdateMenuOptions();
    }

    private void ClearEditButtons()
    {
        foreach (GameObject button in extraInfoEditButtonList)
        {
            Destroy(button);
        }
        extraInfoEditButtonList = new List<GameObject>();
    }

    public void CloseMenu()
    {
        menuSource.extraInfoManagerMenu = null;
        Destroy(gameObject);
    }

    public void GenerateExtraInfoEditor(int extraInfoIdx)
    {
        GameObject extraInfoEditorMenu = Instantiate(extraInfoEditorPrefab);
        extraInfoEditorMenu.GetComponent<ExtraInfoEditorScript>().extraInfoManagerSource = this;
        gameObject.SetActive(false);
    }
}
