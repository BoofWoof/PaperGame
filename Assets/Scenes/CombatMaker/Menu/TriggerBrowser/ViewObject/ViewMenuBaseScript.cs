using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ViewMenuBaseScript : MenuBaseScript
{
    public GridObject SelectedCharacter;

    public GameObject CutsceneViewItem;
    public GameObject ContentScreen;
    public List<GameObject> TriggerList = new List<GameObject>();
    
    public TMP_InputField Name;
    public TextMeshProUGUI Notes;

    public GameObject[,] SourceGrid;

    public void OnEnable()
    {
        UpdateCutsceneList();
    }

    private void ClearList()
    {
        for (int idx = 0; idx < TriggerList.Count; idx++)
        {
            Destroy(TriggerList[idx]);
        }
        TriggerList = new List<GameObject>();
    }

    public void UpdateCutsceneList()
    {
        ClearList();
        foreach ((string label, string triggerType) in GridCrafter.CutsceneDataManager.TriggersContainingObject(SelectedCharacter))
        {
            GameObject triggerInfoItem = Instantiate(CutsceneViewItem);
            triggerInfoItem.GetComponent<CutsceneViewItemScript>().UpdateDisplayData(
                label,
                triggerType,
                GridCrafter.CutsceneDataManager.GetTargetCount(label),
                GridCrafter.CutsceneDataManager.GetTrigger(label).TriggerLimit
                );
            triggerInfoItem.GetComponent<CutsceneViewItemScript>().SourceMenu = this;
            triggerInfoItem.GetComponent<CutsceneViewItemScript>().SourceGrid = SourceGrid;
            triggerInfoItem.transform.SetParent(ContentScreen.transform);

            TriggerList.Add(triggerInfoItem);
        }

        float contentHeight = 100 * TriggerList.Count;
        ContentScreen.GetComponent<RectTransform>().sizeDelta = new Vector2(970, contentHeight);
        for (int idx = 0; idx < TriggerList.Count; idx++)
        {
            TriggerList[idx].GetComponent<RectTransform>().anchoredPosition =
                new Vector3(0, idx * 100f - contentHeight / 2f + 50, 0);
        }

    }

    public void RenameObject()
    {
        string newName = Name.text;
        if (newName.Length == 0) return;
        RenameObjectInfo renameObjectInfo = new RenameObjectInfo();
        renameObjectInfo.Label = newName;
        if (SourceGrid == GridCrafter.characterGrid) renameObjectInfo.GridLayer = "Character";
        if (SourceGrid == GridCrafter.objectGrid) renameObjectInfo.GridLayer = "Object";
        if (SourceGrid == GridCrafter.blockGrid) renameObjectInfo.GridLayer = "Tile";
        List<GridObject> targetObjects = new List<GridObject>() {SelectedCharacter};
        GridCrafter.CutsceneDataManager.AddRenameObjectTrigger(renameObjectInfo, targetObjects);

        UpdateCutsceneList();
    }
}
