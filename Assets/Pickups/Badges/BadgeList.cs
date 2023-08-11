using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BadgeList : MonoBehaviour
{
    GameControls controls;

    //X Positions
    public float initialXOffset;
    public float itemXOffset;
    public float itemXSize;
    public int visibleColumns;

    //Y Positions
    public float initialYOffset;
    public float itemYOffset;
    public float itemYSize;
    public int visibleRows;

    //Selection
    public GameObject cursor;
    private int topRowIdx = 0;

    private int xcord = 0;
    private int ycord = 0;
    private float movementDelay = 0;
    private float movementDelayTrigger = 0.25f;

    //Description
    public GameObject itemDescriptions;
    private TextMeshProUGUI descriptionText;

    //BadgeEconomy
    public GameObject badgeEconomy;
    private TextMeshProUGUI badgeEconomyText;

    //Debug
    private List<int> badgeList;

    //All Items
    private List<List<GameObject>> gameObjectRow = new List<List<GameObject>>();
    
    private void Awake()
    {
        controls = new GameControls();
    }

    private void OnEnable()
    {
        controls.OverworldControls.Enable();
        clearItems();
        generateItems();
        movementDelay = movementDelayTrigger;
    }

    private void OnDisable()
    {
        controls.OverworldControls.Disable();
        clearItems();
    }

    private void Start()
    {
        badgeEconomyText = badgeEconomy.GetComponent<TextMeshProUGUI>();
        descriptionText = itemDescriptions.GetComponent<TextMeshProUGUI>();
        UpdateEconomyText();
    }

    void UpdateEconomyText()
    {
        badgeEconomyText.text = GameDataTracker.playerData.CurrentBadgePoints.ToString() + "/" + GameDataTracker.playerData.MaxBadgePoints.ToString();
    }

    public void clearItems()
    {
        foreach (List<GameObject> delrows in gameObjectRow)
        {
            foreach (GameObject delitems in delrows)
            {
                Destroy(delitems);
            }
        }
        gameObjectRow = new List<List<GameObject>>();
    }

    public void generateItems()
    {
        GameDataTracker.playerData.UnlockedEquipmentID.Sort();
        badgeList = GameDataTracker.playerData.UnlockedEquipmentID;

        foreach (int debug_value in GameDataTracker.playerData.UnlockedEquipmentID)
        {
            Debug.Log(debug_value);
        }
        foreach (int debug_value in GameDataTracker.playerData.EquipedEquipmentID)
        {
            Debug.Log(debug_value);
        }

        for (int i = 0; i < visibleRows; i++)
        {
            int row = topRowIdx + i;
            List<GameObject> gameObjectCol = new List<GameObject>();
            for (int j = 0; j < visibleColumns; j++)
            {
                GameObject NewObj = new GameObject(); //Create the GameObject
                NewObj.AddComponent<CanvasRenderer>(); //Add the Image Component script
                Image NewImage = NewObj.AddComponent<Image>(); //Add the Image Component script
                int itemIdx = row * visibleColumns + j;
                if (itemIdx >= badgeList.Count)
                {
                    NewImage.sprite = BadgeMapping.defaultImage;
                    NewImage.color = new Color32(255, 255, 255, 255);
                }
                else
                {
                    NewImage.sprite = BadgeMapping.getBadge(badgeList[itemIdx]).GetComponent<BadgeTemplate>().sprite;
                    if (GameDataTracker.playerData.EquipedEquipmentID.Contains(badgeList[itemIdx]))
                    {
                        NewImage.color = new Color32(255, 255, 255, 255);
                    } else
                    {
                        NewImage.color = new Color32(125, 125, 125, 125);
                    }
                }
                NewObj.transform.SetParent(transform, false);
                NewObj.transform.position = transform.position + new Vector3(Screen.width * (itemXOffset * j - initialXOffset), Screen.height * (-itemYOffset * i - initialYOffset), 0);
                NewObj.transform.localScale = new Vector3(itemXSize, itemYSize, 1);
                RectTransform NewObjRect = NewObj.GetComponent<RectTransform>();
                NewObjRect.anchorMin = new Vector2(0, 0);
                NewObjRect.anchorMax = new Vector2(1, 1);
                gameObjectCol.Add(NewObj);
            }
            gameObjectRow.Add(gameObjectCol);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (xcord < 0)
        {
            xcord = 0;
        }
        if (xcord >= visibleColumns)
        {
            xcord = visibleColumns-1;
        }
        if (ycord < 0)
        {
            ycord = 0;
        }
        if (ycord >= visibleRows)
        {
            ycord = visibleRows - 1;
        }
        cursor.transform.position = transform.position + new Vector3(Screen.width * (itemXOffset * xcord - initialXOffset), Screen.height * (-itemYOffset * ycord - initialYOffset), 0);

        int itemIdx = ycord * visibleColumns + xcord;
        if(itemIdx < badgeList.Count)
        {
            BadgeTemplate badge = BadgeMapping.getBadge(badgeList[itemIdx]).GetComponent<BadgeTemplate>();
            string itemName = badge.name;
            string itemDescription = badge.itemDescription;

            descriptionText.text = itemName + ": " + itemDescription;
            if (controls.OverworldControls.MainAction.triggered)
            {
                if (movementDelay > movementDelayTrigger)
                {
                    if (!GameDataTracker.playerData.EquipedEquipmentID.Contains(badgeList[itemIdx]))
                    {
                        if (GameDataTracker.playerData.CurrentBadgePoints > badge.badgeCost)
                        {
                            GameDataTracker.playerData.EquipedEquipmentID.Add(badgeList[itemIdx]);
                            gameObjectRow[ycord][xcord].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
                            badge.OnEquip();
                            GameDataTracker.playerData.CurrentBadgePoints -= badge.badgeCost;
                            UpdateEconomyText();
                        }

                    } else
                    {
                        GameDataTracker.playerData.EquipedEquipmentID.Remove(badgeList[itemIdx]);
                        gameObjectRow[ycord][xcord].GetComponent<Image>().color = new Color32(125, 125, 125, 125);
                        badge.OnUnequip();
                        GameDataTracker.playerData.CurrentBadgePoints += badge.badgeCost;
                        UpdateEconomyText();
                    }
                    movementDelay = 0;
                }
            }
        } else
        {
            descriptionText.text = "Empty Pocket";
        }

        Vector2 thumbstick_values = controls.OverworldControls.Movement.ReadValue<Vector2>();
        float xPress = thumbstick_values[0];
        float yPress = thumbstick_values[1];

        movementDelay += Time.unscaledDeltaTime;
        //Debug.Log(movementDelay);
        //Debug.Log(xPress);
        //Debug.Log(yPress);


        if (movementDelay > 0.25 )
        {
            if (xPress > 0.5)
            {
                xcord += 1;
                movementDelay = 0;
            }
            if (xPress < -0.5)
            {
                xcord -= 1;
                movementDelay = 0;
            }
            if (yPress < -0.5)
            {
                ycord += 1;
                movementDelay = 0;
            }
            if (yPress > 0.5)
            {
                ycord -= 1;
                movementDelay = 0;
            }
        }
    }
}
