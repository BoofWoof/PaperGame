using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterViewMenuScript : MenuBaseScript
{
    public FighterClass SelectedCharacter;

    //Text
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Health;
    public TextMeshProUGUI Power;
    public TextMeshProUGUI Defense;
    public TextMeshProUGUI Notes;

    public GameObject CutsceneViewItem;
    public GameObject ContentScreen;
    public List<GameObject> TriggerList = new List<GameObject>();

    public override void Start()
    {
        base.Start();
        Debug.Log(SelectedCharacter);
        Name.SetText(SelectedCharacter.Name);
        Health.SetText($"Health: {SelectedCharacter.HPMax}");
        Power.SetText($"Power: {SelectedCharacter.Power}");
        Defense.SetText($"Defense: {SelectedCharacter.Defense}");
        Notes.SetText("Note: " + SelectedCharacter.Note);

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
        foreach ((string label, CombatTriggerType triggerType) in GridCrafter.CutsceneDataManager.TriggersContainingObject(SelectedCharacter))
        {
            GameObject triggerInfoItem = Instantiate(CutsceneViewItem);
            triggerInfoItem.GetComponent<CutsceneViewItemScript>().UpdateDisplayData(
                label,
                triggerType.ToString(),
                GridCrafter.CutsceneDataManager.GetTargetCount(label),
                GridCrafter.CutsceneDataManager.GetTrigger(label).TriggerLimit
                );
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
}
