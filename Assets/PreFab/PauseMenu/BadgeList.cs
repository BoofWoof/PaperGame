using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BadgeList : MonoBehaviour
{
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

    //Description
    public GameObject itemDescriptions;
    private TextMeshProUGUI descriptionText;

    //Debug
    private List<int> itemList;

    //All Items
    private List<List<GameObject>> gameObjectRow = new List<List<GameObject>>();

    // Start is called before the first frame update
    void OnEnable()
    {
        clearItems();
        generateItems();
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
        itemList = GameDataTracker.playerData.Inventory;

        descriptionText = itemDescriptions.GetComponent<TextMeshProUGUI>();

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
                if (itemIdx >= itemList.Count)
                {
                    NewImage.sprite = ItemMapping.defaultImage;
                }
                else
                {
                    NewImage.sprite = ItemMapping.imageMap[itemList[itemIdx]];
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
        if(itemIdx < itemList.Count)
        {
            string itemName = ItemMapping.nameMap[itemList[itemIdx]];
            string itemDescription = ItemMapping.descriptionMap[itemList[itemIdx]];

            descriptionText.text = itemName + ": " + itemDescription;
            if (Input.GetButton("Fire1"))
            {
                if (movementDelay > 0.25)
                {
                    ItemMapping.actionMap(itemList[itemIdx]).OverWorldUse();
                    GameDataTracker.playerData.Inventory.RemoveAt(itemIdx);
                    clearItems();
                    generateItems();
                    movementDelay = 0;
                }
            }
        } else
        {
            descriptionText.text = "Empty Pocket";
        }

        float xPress = Input.GetAxis("Horizontal");
        float yPress = Input.GetAxis("Vertical");

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

        if (Input.GetKeyDown("d"))
        {
            xcord += 1;
        }
        if (Input.GetKeyDown("a"))
        {
            xcord -= 1;
        }
        if (Input.GetKeyDown("s"))
        {
            ycord += 1;
        }
        if (Input.GetKeyDown("w"))
        {
            ycord -= 1;
        }
    }
}
